/* FCM Manager receive thread
 * 
 * (c) 2012-2014 by Fabian Huslik
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.Windows.Forms;

namespace FCM_Manager
{
    
    class SerPort
    {
        private SerialPort m_ComPort;
        private string m_strPortName;
        //private bool m_Alive = false;
        private Thread m_Thread_Work;
        public delegate void m_RxReadyHandler_t(SerPort port);
        private  m_RxReadyHandler_t m_RXhandler;
        private List<byte> m_byRxBuffer = new List<byte>();
        public string m_VersionString;
        public int m_MenuSize;
        public bool m_IsBootloader=false;
        //public bool m_bSearching = false;

        public enum e_LinkState
        {
            None,
            Searching,
            Connected,
        };

        public enum e_CommState
        {
            None,
            Found,
            ParReading,
            Finished,
        };

        e_LinkState myLinkStatus;
        e_CommState myCommStatus;
        
        public SerPort(string n, m_RxReadyHandler_t RecHandler) 
        {
            myLinkStatus = e_LinkState.Searching;
            if (n.StartsWith("COM"))
            {
                m_strPortName = n;
                m_RXhandler = RecHandler;
                m_Thread_Work = new Thread(new ThreadStart(ThreadProc));
                m_Thread_Work.Name = "thr" + n;
                m_Thread_Work.Start();
            }
            // return to main
        }

        public string GetName()
        {
            return m_strPortName;
        }


        public e_LinkState GetLinkStatus()
        {
            return myLinkStatus;
        }

        public e_CommState GetCommStatus()
        {
            return myCommStatus;
        }

        public void SetCommStatus(e_CommState s)
        {
            if (myLinkStatus == e_LinkState.Connected)
            {
                myCommStatus = s;
            }
        }

        public byte[] GetData() // call from consumer task after event
        {
            byte[] data;
            
            lock (m_byRxBuffer)
            {
                data = m_byRxBuffer.ToArray();
                m_byRxBuffer.Clear();
            }
            return data;
        }

        public void DiscardData()
        {
            m_ComPort.DiscardInBuffer();
            lock (m_byRxBuffer)
            { 
                m_byRxBuffer.Clear();
            }
            Thread.Sleep(33);
        }

       
        public int WriteData(string s)
        {
            if (!m_ComPort.IsOpen == true || bTXbusy)
            { return 1; }

            lock (m_byRxBuffer)
            {
                m_byRxBuffer.Clear();
            }

            try 
            { 
                m_ComPort.DiscardInBuffer();
                m_ComPort.Write(s);
                return 0;
            }
            catch 
            {
                return 1;
            }

        }

        public int WriteData(byte[] ba)
        {
            if (!m_ComPort.IsOpen == true || bTXbusy)
            { 
                return 1; 
            }

            lock (m_byRxBuffer)
            {
                m_byRxBuffer.Clear();
            }
            try
            {
                m_ComPort.DiscardInBuffer();
                m_ComPort.Write(ba,0,ba.Length);
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public void Close()
        {
            // stop thread

            myLinkStatus = e_LinkState.None;
            myCommStatus = e_CommState.None;
            // thread should clean up stuff.-


        }

		public bool asking;

        public string ask(byte[] question, int timeout)
        {
            asking = true;
            string answer = "";
            
			if (WriteData(question) != 0)
            {
                // give up on error
                MessageBox.Show("argl");
            }
            else
            {
                // wait for answer
                Thread.Sleep(timeout);

                // analyse possible answer
                lock (m_byRxBuffer)
                {
                    answer = ByteArrayToString(m_byRxBuffer.ToArray(), 0, m_byRxBuffer.Count);
                }
                //MessageBox.Show(answer);

            }

            asking = false;

            return answer;
        }



        private void ThreadProc()
        {
            Thread.Sleep(30); // get all threads started ;-)

            // try to open port
            try
            {
                m_ComPort = new SerialPort(m_strPortName, 115200, Parity.None, 8, StopBits.One);
                m_ComPort.DiscardNull = false;
                m_ComPort.ReadTimeout = 300;
                m_ComPort.ReceivedBytesThreshold = 10;
                m_ComPort.WriteTimeout = 300;
                m_ComPort.Encoding = Encoding.ASCII; // prevent all upper bytes to look like 0x3f
                m_ComPort.Open();

                m_ComPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.DataReceivedHandler);
            }
            catch (Exception)
            {
                quitPort();
                return; // ends thread
            }

            // send "Hello"
            m_ComPort.DiscardInBuffer();
            m_byRxBuffer.Clear();

            if (WriteData("---FCM~~~") != 0)
            {
                // give up on error
                quitPort();
                return;
            }
            else
            {
                myLinkStatus = e_LinkState.Searching;
                myCommStatus = e_CommState.None;

                // wait for answer
                Thread.Sleep(200);

                // analyse possible answer
                string str = ByteArrayToString(m_byRxBuffer.ToArray(),0,m_byRxBuffer.Count);
                if (str.StartsWith("---FCM"))
                {
                    myLinkStatus = e_LinkState.Connected;
                    myCommStatus = e_CommState.Found;
                    // FCM answer found!
                    // extract data 
                    m_VersionString = m_byRxBuffer[6].ToString() + "." + m_byRxBuffer[7].ToString() + "." + (m_byRxBuffer[8]*256+ m_byRxBuffer[9]).ToString() ;

                    if (m_byRxBuffer[6] >= 100)
                    {
                        m_IsBootloader = true;
                    }
                    else 
                    {
                        if (!ByteArrayToString(m_byRxBuffer.ToArray(), 10, 3).StartsWith("~~~"))
                        {
                            m_MenuSize = (m_byRxBuffer[10] << 8) + m_byRxBuffer[11];
                        }
                        else
                        {
                            m_MenuSize = 0;
                        }

                    }
                }
                else
                {
                    // nothing, just give up.
                    quitPort();
                    return;
                }
            }

            // stay in loop
            while (myLinkStatus == e_LinkState.Connected)
            {
                Thread.Sleep(100);

              
            }

            quitPort();
        }

        private void quitPort()
        {
            myLinkStatus = e_LinkState.None;
            myCommStatus = e_CommState.None;
            m_ComPort.DataReceived -= DataReceivedHandler;
             try
            {
                if (m_ComPort.IsOpen == true)
                {
                    m_ComPort.DiscardInBuffer();
                    m_ComPort.DiscardOutBuffer();
                    m_ComPort.Close();
                }
            }
            catch
            { }
        }


        bool bTXbusy;

        public void SendDataForFlashing(UInt32 startoffset, uint len, byte[] data)
        {
            
            if (!m_ComPort.IsOpen || bTXbusy)
            { return; }
            bTXbusy = true;
            int a = 0;
            int rem = (int)len;
            int chunk = 128 * 4;

            while (rem > 0)
            {
                if (rem < chunk)
                {
                    chunk = rem; // last part
                }

                m_ComPort.Write(data,(int) (a + startoffset), (int)chunk);

                a += chunk;
                rem -= chunk;

                Thread.Sleep(10); // wait 10ms for slave to flash (needs 4 acc. datasheet)
            }
            bTXbusy = false;
        }

        public void SendDataForFlashing2(UInt32 startoffset, uint len, byte[] data)
        {

            if (!m_ComPort.IsOpen || bTXbusy)
            { return; }
         
            m_ComPort.Write(data, (int)(startoffset), (int)len);

         
            Thread.Sleep(10); // wait 10ms for slave to flash (needs 4 acc. datasheet)
         
        }

        DateTime lastrx;
        
        private void DataReceivedHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            // Buffer and process binary data
            
            
            lock (m_byRxBuffer)
            {
                try
                {
                    while (m_ComPort.BytesToRead > 0)
                    {
                        m_byRxBuffer.Add((byte)m_ComPort.ReadByte());
                        //if (i++ >= 100) break; // let us do some work between the data
                        lastrx = DateTime.Now;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

/*                if (m_byRxBuffer.Count > 10000)
                {
                    m_byRxBuffer.Clear();
                }*/
            }

            if(m_RXhandler != null && (myLinkStatus == e_LinkState.Connected) && !asking)
                m_RXhandler(this);

        }

        // fixme auslagern, sind dubletten:
        private byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private string ByteArrayToString(byte[] arr,int idx, int cnt)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string s;
            try 
            { 
                s=enc.GetString(arr,idx,cnt);
            }
            catch
            {
                s = "";
            }

            return s;
        }

    }
}
