/* FCM Manager main file - quick and dirty implementation of FCMCP and user interface using other code for visualisation.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using OSC_DLL;
using System.Linq;
using System.Collections;


namespace FCM_Manager
{
    public partial class MainWindow : Form
    {

        RXData RX_Dat;
        RXQuat RX_Quat;
        ArrayList RX_Parameteritems;

        SimpleOscilloscope sc_acc;
        SimpleOscilloscope sc_gy;
        SimpleOscilloscope sc_mag;
        SimpleOscilloscope sc_gov;
        SimpleOscilloscope sc_RC;
        SimpleOscilloscope sc_ht;

        public MainWindow()
        {
            InitializeComponent();
            RX_Dat = new RXData();
            RX_Quat = new RXQuat();
            RX_Parameteritems = new ArrayList();

            comboBox1.Items.Add("Auto");
            string[] portnames = GetAvailablePorts();
            foreach (string s in portnames)
            {
                comboBox1.Items.Add(s);
            }

            string st = Properties.Settings.Default.LastComPort;

            if (comboBox1.Items.Contains(st))
            {
                comboBox1.Text = st;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

            sc_acc = new SimpleOscilloscope("Accel", "Oscilloscope\\osc_acc.ini");
            sc_gy = new SimpleOscilloscope("Gyro", "Oscilloscope\\osc_gyro.ini");
            sc_mag = new SimpleOscilloscope("Magnet", "Oscilloscope\\osc_mag.ini");
            sc_gov = new SimpleOscilloscope("Governor", "Oscilloscope\\osc_gov.ini");
            sc_RC = new SimpleOscilloscope("RC", "Oscilloscope\\osc_rc.ini");
            sc_ht = new SimpleOscilloscope("h_t", "Oscilloscope\\osc_h_t.ini");

            // only 4 possible ! // fixme why

            form_3Dcuboid = new Form_3Dcuboid();
            form_3Dcuboid.MinimizeInsteadOfClose = true;
            form_3Dcuboid2 = new Form_3Dcuboid();
            form_3Dcuboid2.MinimizeInsteadOfClose = true;
            form_3Dcuboid3 = new Form_3Dcuboid(new string[] { "Form_3Dcuboid/Right.png", "Form_3Dcuboid/Left.png", "Form_3Dcuboid/Back.png", "Form_3Dcuboid/Front.png", "Form_3Dcuboid/Top.png", "Form_3Dcuboid/Bottom.png" }, new float[] { 1.5f, 1, 0.5f }, Form_3Dcuboid.CameraViews.Right, 50.0f);
            form_3Dcuboid3.MinimizeInsteadOfClose = true;
        }


        List<SerPort> PresentCOMPorts = new List<SerPort>();

        SerPort MyActivePort;
        private bool HaveParsActual; // fixme stupid hack - make new - visibility of data only, if connection is live.

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (MyActivePort != null && MyActivePort.GetLinkStatus() == SerPort.e_LinkState.Connected)
            {
                StopStream();
            }

            buttonConnect.Enabled = false;
            labelConnected.Text = "searching...";
            labelConnected.Refresh();

            byte[] dummy = new byte[1000];

            string[] portnames = GetAvailablePorts();
            MyActivePort = null;
            foreach (SerPort sp in PresentCOMPorts)
            {
                sp.Close();
            }

            PresentCOMPorts.Clear();


            if (comboBox1.SelectedItem.ToString() == "Auto")
            {
                // start one thread per port
                foreach (string pn in portnames)
                {
                    SerPort sp = new SerPort(pn, MyNewRXHandler);
                    PresentCOMPorts.Add(sp);
                }
            }
            else
            {
                SerPort sp = new SerPort(comboBox1.SelectedItem.ToString(), MyNewRXHandler);
                PresentCOMPorts.Add(sp);
            }

            HaveParsActual = false;


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {

            }

            Properties.Settings.Default.Save();

            serWrite("---STS~~~");
            Thread.Sleep(100);

            if (MyActivePort != null) MyActivePort.Close();

            e.Cancel = true;

            Thread.Sleep(500); // let some time for the Freds.

            // kill all oszies
            sc_acc.Dispose();
            sc_gy.Dispose();
            sc_mag.Dispose();
            sc_gov.Dispose();
            //sc_RC.Dispose();
            //sc_ht.Dispose();

            e.Cancel = false;


            // now it's over...
        }

        List<byte> bBuffer = new List<byte>();

        void MyNewRXHandler(SerPort port)
        {
            // here we get a complete frame...
            byte[] ba = port.GetData();
            bBuffer.AddRange(ba);
            ProcessBuffer();
        }


        private void ProcessBuffer()
        {
            byte[] ba = new byte[1000];
            int idxStart;
            int idxEnd = -1;
            byte[] searchStart = { (byte)'-', (byte)'-', (byte)'-' };
            //  byte[] searchEnd = { (byte)'=', (byte)'=', (byte)'=' };
            byte[] searchEnd = { (byte)'~', (byte)'~', (byte)'~' };
            //int removecount; // how often the start of the frame was not removed...

            do
            {
                idxStart = IndexOfBytes(bBuffer, searchStart, 0, 0);
                if (idxStart >= 0)
                    idxEnd = IndexOfBytes(bBuffer, searchEnd, idxStart, 0);

                if (idxStart > -1 && idxEnd > -1) // "---" & "~~~" found
                {
                    // this is a "---"

                    if (bBuffer[idxStart + 3] == 'P' && // PAR
                        bBuffer[idxStart + 4] == 'A' &&
                        bBuffer[idxStart + 5] == 'R'
                        )
                    {
                        bBuffer.CopyTo(idxStart, ba, 0, idxEnd - idxStart);
                        bBuffer.RemoveRange(0, idxEnd + 3);
                        AnalyzeParameterItem(ba);


                    }


                    else
                        if (bBuffer[idxStart + 3] == 'D')
                        {
                            try
                            {
                                // this might be a frame.
                                bBuffer.CopyTo(idxStart, ba, 0, 76);
                                bBuffer.RemoveRange(0, idxStart + 76);
                                AnalyzeStdFrame(ba);
                            }
                            catch
                            {
                                bBuffer.Clear();
                            }
                        }
                        else
                            if (bBuffer[idxStart + 3] == 'Q') // fixme check length!
                            {
                                // this might be a frame.
                                bBuffer.CopyTo(idxStart, ba, 0, 75);
                                bBuffer.RemoveRange(0, idxStart + 75);
                                AnalyzeQuatFrame(ba);
                            }
                            else
                                if (bBuffer[idxStart + 3] == 'T' && bBuffer.Count > 8 * 21 + 9)
                                {
                                    // this might be a frame.
                                    bBuffer.CopyTo(idxStart, ba, 0, 8 * 21 + 9);
                                    bBuffer.RemoveRange(0, idxStart + 8 * 21 + 9 + 1);

                                    string s = "";

                                    for (int i = 6; i < ba.Length - 3; i++)
                                    {
                                        if (ba[i] == 0)
                                            ba[i] = 0x20;

                                        s += (char)ba[i];
                                        if ((i - 5) % 21 == 0)
                                        {
                                            s += System.Environment.NewLine;
                                        }

                                    }
                                    MenueSetText(s);
                                }
                                else
                                {
                                    // other frame???
                                    bBuffer.Clear();
                                }
                }
                else
                {
                    // wait for more data

                    if (idxStart > 0 && idxEnd < 0)
                    {
                        // cut away the stuff before the start of frame
                        bBuffer.RemoveRange(0, idxStart);
                    }
                    else
                    {
                        if (bBuffer.Count > 500)
                        {
                            // bBuffer.Clear();
                        }
                    }
                    Thread.Sleep(50);

                }
            } while (idxStart > -1 && idxEnd > -1);

            // Look in the byte array for useful information
            // then remove the useful data from the buffer
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.

        private void MenueSetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBoxMenu.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(MenueSetText);
                try // at ending, sometimes the object is already diposed.
                {
                    this.Invoke(d, new object[] { text });
                }
                catch
                {
                }
            }
            else
            {
                this.textBoxMenu.Text = text;
            }
        }


        int framecounter = 0;

        private void AnalyzeStdFrame(byte[] ba)
        {

            int i = 0;
            RX_Dat.gx = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.gy = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.gz = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.ax = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.ay = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.az = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.mx = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.my = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.mz = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.gov_x = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.gov_y = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.gov_z = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.RC_x = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.RC_y = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.RC_z = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.RC_a = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.h = BitConverter.ToInt32(ba, i += 4);
            RX_Dat.temp0 = BitConverter.ToInt16(ba, i += 2);
            RX_Dat.temp1 = BitConverter.ToInt16(ba, i += 2);


            sc_acc.AddData(RX_Dat.ax, RX_Dat.ay, RX_Dat.az);
            sc_gy.AddData(RX_Dat.gx, RX_Dat.gy, RX_Dat.gz);
            sc_mag.AddData(RX_Dat.mx, RX_Dat.my, RX_Dat.mz);
            sc_gov.AddData(RX_Dat.gov_x, RX_Dat.gov_y, RX_Dat.gov_z);
            //sc_gov.AddData(RX_Dat.h, RX_Dat.temp0, 0);

            framecounter++;

            lbl_FramesSetText(framecounter.ToString());

            sc_RC.AddData(RX_Dat.RC_x, RX_Dat.RC_y, RX_Dat.RC_z);
            sc_ht.AddData(RX_Dat.h, RX_Dat.temp0, 0);

        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);

        private void lbl_FramesSetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.

            if (this.Disposing) return;

            try
            {
                if (this.lbl_Frames.InvokeRequired) // "try catch" , as there are strange errors on close.
                {
                    SetTextCallback d = new SetTextCallback(lbl_FramesSetText);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.lbl_Frames.Text = text;
                }
            }
            catch
            { }
        }




        private Form_3Dcuboid form_3Dcuboid;
        private Form_3Dcuboid form_3Dcuboid2;
        private Form_3Dcuboid form_3Dcuboid3;



        private void AnalyzeQuatFrame(byte[] ba)
        {
            int i = 0;

            RX_Quat.qAct[0] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qAct[1] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qAct[2] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qAct[3] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qSet[0] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qSet[1] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qSet[2] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qSet[3] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qDiff[0] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qDiff[1] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qDiff[2] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.qDiff[3] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.vPos[0] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.vPos[1] = BitConverter.ToSingle(ba, i += 4);
            RX_Quat.vPos[2] = BitConverter.ToSingle(ba, i += 4);

            form_3Dcuboid.RotationMatrix = ConvertToRotationMatrix(RX_Quat.qAct);
            form_3Dcuboid2.RotationMatrix = ConvertToRotationMatrix(RX_Quat.qSet);
            form_3Dcuboid3.RotationMatrix = ConvertToRotationMatrix(RX_Quat.qDiff);

            float[] ftemp = new float[3];
            ftemp[0] = RX_Quat.vPos[1];
            ftemp[1] = -RX_Quat.vPos[0];
            ftemp[2] = RX_Quat.vPos[2]-3.0f; // add the translation! 

            form_3Dcuboid3.TranslationVector = ftemp;// fixme edit angle / camera distance / background image etc.

            sc_gov.AddData(RX_Quat.vPos[0], RX_Quat.vPos[1], RX_Quat.vPos[2]);

            framecounter++;

            lbl_FramesSetText(framecounter.ToString());

        }

        private void AnalyzeParameterItem(byte[] ba)
        {
            int i = 6;
            ParameterItem pi = new ParameterItem();
            pi.ID = ba[i++];
            pi.ParentID = ba[i++];
            pi.Value = BitConverter.ToInt16(ba, i);
            i += 2;
            pi.Upper = BitConverter.ToInt16(ba, i);
            i += 2;
            pi.Lower = BitConverter.ToInt16(ba, i);
            i += 2;
            if (pi.Upper != 0 || pi.Lower != 0)
            {
                pi.IsPar = true;
            }

            pi.text = Encoding.UTF8.GetString(ba, i, 20);
            int rem = pi.text.IndexOf("\0");
            pi.text = pi.text.Remove(rem);

            ParameterItem pf = null;
            foreach (ParameterItem p in RX_Parameteritems)
            {
                if (p.ID == pi.ID)
                {
                    pf = p;
                }
            }
            if (pf == null)
                RX_Parameteritems.Add(pi);
            else
            {
                pf.Value = pi.Value;    // update existing ID
                pf.NeedsUpdate = true;
                UpdateGrid(); // fixme update GUI!
            }

        }

        public float[] ConvertToRotationMatrix(float[] qin)
        {
            float[] Quaternion = new float[4];

            Quaternion[0] = qin[0];
            Quaternion[1] = qin[1];
            Quaternion[2] = qin[2];
            Quaternion[3] = -qin[3];


            float R11 = 2 * Quaternion[0] * Quaternion[0] - 1 + 2 * Quaternion[1] * Quaternion[1];
            float R12 = 2 * (Quaternion[1] * Quaternion[2] + Quaternion[0] * Quaternion[3]);
            float R13 = 2 * (Quaternion[1] * Quaternion[3] - Quaternion[0] * Quaternion[2]);
            float R21 = 2 * (Quaternion[1] * Quaternion[2] - Quaternion[0] * Quaternion[3]);
            float R22 = 2 * Quaternion[0] * Quaternion[0] - 1 + 2 * Quaternion[2] * Quaternion[2];
            float R23 = 2 * (Quaternion[2] * Quaternion[3] + Quaternion[0] * Quaternion[1]);
            float R31 = 2 * (Quaternion[1] * Quaternion[3] + Quaternion[0] * Quaternion[2]);
            float R32 = 2 * (Quaternion[2] * Quaternion[3] - Quaternion[0] * Quaternion[1]);
            float R33 = 2 * Quaternion[0] * Quaternion[0] - 1 + 2 * Quaternion[3] * Quaternion[3];
            return new float[] { R11, R12, R13,
                                 R21, R22, R23,
                                 R31, R32, R33 };
        }

        private void StopStream()
        {
            serWrite("---STS~~~");
            Thread.Sleep(100);
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            serWrite("---STD~~~");
        }

        private void btnStopData_Click(object sender, EventArgs e)
        {
            StopStream();
        }

        private void btnGetQuat_Click(object sender, EventArgs e)
        {
            serWrite("---STQ~~~");
        }

        private void btn_Osc_ACC_Click(object sender, EventArgs e)
        {
            sc_acc.ShowHide();
            //sc_RC.ShowHide();
            //sc_ht.ShowHide();
        }

        private void btn_osc_Gyro_Click(object sender, EventArgs e)
        {
            sc_gy.ShowHide();
        }

        private void btn_osc_mag_Click(object sender, EventArgs e)
        {
            sc_mag.ShowHide();
        }

        private void btn_osc_gov_Click(object sender, EventArgs e)
        {
            sc_gov.ShowHide();
        }


        private void serWrite(string s) // takes a unicode string and sends the lower byte only.
        {
            if (MyActivePort != null)
            {
                MyActivePort.WriteData(s);
            }
        }

        private void serWrite(byte[] s)
        {
            if (MyActivePort != null)
            {
                MyActivePort.WriteData(s);
            }
        }


        private void btn_mnu0_Click(object sender, EventArgs e)
        {
            serWrite("---MNU0~~~");
        }

        private void btn_mnuplus_Click(object sender, EventArgs e)
        {
            serWrite("---MNUp~~~");
        }

        private void btn_mnuEnt_Click(object sender, EventArgs e)
        {
            serWrite("---MNUE~~~");
        }

        private void btn_mnu_minus_Click(object sender, EventArgs e)
        {
            serWrite("---MNUm~~~");
        }


        private void btn_mnuplusplus_Click(object sender, EventArgs e)
        {
            serWrite("---MNUP~~~");
        }

        private void btn_mnu_minusminus_Click(object sender, EventArgs e)
        {
            serWrite("---MNUM~~~");
        }
        private void btn_ShowActual_Click(object sender, EventArgs e)
        {
            float[] q = new float[4] { 1, 0, 0, 0 };
            form_3Dcuboid.RotationMatrix = ConvertToRotationMatrix(q);
            form_3Dcuboid.Text = "Actual 3D";
            form_3Dcuboid.Show();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            float[] q = new float[4] { 1, 0, 0, 0 };
            form_3Dcuboid2.RotationMatrix = ConvertToRotationMatrix(q);
            form_3Dcuboid2.Text = "Setpoint 3D";
            form_3Dcuboid2.Show();
        }

        private void btnErr3D_Click(object sender, EventArgs e)
        {
            float[] q = new float[4] { 1, 0, 0, 0 };
            form_3Dcuboid3.RotationMatrix = ConvertToRotationMatrix(q);
            form_3Dcuboid3.CameraDistance = 50;
            form_3Dcuboid3.Text = "Sim 3D";
            form_3Dcuboid3.Show();
        }


        public int IndexOfBytes(List<byte> array, byte[] pattern, int startIndex, int count)
        {
            if (array == null || array.Count == 0 || pattern == null || pattern.Length == 0/* || count == 0*/)
            {
                return -1;
            }
            int i = startIndex;
            int endIndex = count > 0 ? Math.Min(startIndex + count, array.Count) : array.Count;
            int fidx = 0;
            int lastFidx = 0;

            while (i < endIndex)
            {
                lastFidx = fidx;
                fidx = (array[i] == pattern[fidx]) ? ++fidx : 0;
                if (fidx == pattern.Length)
                {
                    return i - fidx + 1;
                }
                if (lastFidx > 0 && fidx == 0)
                {
                    i = i - lastFidx;
                    lastFidx = 0;
                }
                i++;
            }
            return -1;
        }

        byte[] sendarr = new byte[270000]; // covering a whole chip UC3B0256+x !! Attention, the array size appears again below!
        UInt32 sendarr_startaddress = 0;
        UInt32 sendarr_LastAddress = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            StopStream();

            if (openHexFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AnalyzeFile(ref sendarr_startaddress, ref sendarr_LastAddress);

                /*Thread trd = new Thread(new ThreadStart(this.DownloadFile)); // execute in new thread.
                trd.IsBackground = false;
                trd.Start();*/
                DownloadFile();

            }
            else
            {
                //MessageBox.Show("Cancelled.");
            }

        }

        private void DownloadFile()
        {
            //MessageBox.Show("Offset: 0x" + String.Format("{0:X02}", Offset + 16 * 1024) + "  Len: 0x" + String.Format("{0:X02}", Len - 16 * 1024));

            serWrite("---FCM~~~");
            Thread.Sleep(200);
            // fixme expect message
            serWrite("---BOOT~~~");
            Thread.Sleep(200);
            // fixme expect message
            byte[] s1 = StringToByteArray("---FER");
            byte[] s2 = BitConverter.GetBytes(sendarr_startaddress + 16 * 1024); // fixme get the Start-offset from target!
            byte[] s3 = BitConverter.GetBytes(sendarr_LastAddress - 16 * 1024);
            byte[] s4 = StringToByteArray("~~~");
            byte[] ba = s1.Concat(s2).ToArray();
            ba = ba.Concat(s3).ToArray();
            ba = ba.Concat(s4).ToArray();

            MyActivePort.DiscardData();
            if (!MyActivePort.ask(ba, 2000).StartsWith("---FERSD~~~"))
            {
                MessageBox.Show("FCM not erased!");
                return;
            }

            // fixme todo error
            /*
             welcome to a very dirty hack:
             send more than required, because the transmission fails on some small chunks.
             a) not possible to send in 0x100 chunks at all, no notification of received data on target.
             b) the last chunk misses exactly 0x100, even if it is 0x4c0 in size.
             
             I assume AVR32 / the usb stack as BROKEN.
             */
            
            uint chunklen = 0x500;
            uint padding = chunklen - ((sendarr_LastAddress - 0x4000) % chunklen);

            uint remainingBytes = sendarr_LastAddress - 0x4000+padding;
            uint chunkstart = 0x4000;  // read start address


            while (remainingBytes >0)
            {
                if(remainingBytes < chunklen)
                {
                    chunklen = remainingBytes;
                }
                //

                if (!writechunk(sendarr, chunkstart, chunkstart + 0x80000000, chunklen))
                {
                    MessageBox.Show("Des war nix bei " + chunkstart.ToString());
                    return;
                }

                remainingBytes -= chunklen;
                chunkstart += chunklen;

            }


            MessageBox.Show("Flashing successful!");

            serWrite("---RES~~~"); // reset


        }


        private bool writechunk(byte[] data, uint readoffs,uint flashtrg, uint flashlen)
        {
            byte[] s1 = StringToByteArray("---FWR");
            byte[] s2 = BitConverter.GetBytes(flashtrg); // fixme get the Start-offset from target!
            byte[] s3 = BitConverter.GetBytes(flashlen);
            byte[] s4 = StringToByteArray("~~~");
            byte[] ba;

            ba = s1.Concat(s2).ToArray();
            ba = ba.Concat(s3).ToArray();
            ba = ba.Concat(s4).ToArray();
            
            MyActivePort.DiscardData();
            if (!MyActivePort.ask(ba, 100).StartsWith("---FWROK~~~"))
            {
                MessageBox.Show("FCM not ready to be written!");
                return false;
            }

            MyActivePort.DiscardData();
            MyActivePort.asking = true;
            // flash the whole stuff:
            MyActivePort.SendDataForFlashing2(readoffs, flashlen, data); // fixme BL offset missing
            Thread.Sleep(50);
            string state = ByteArrayToString(MyActivePort.GetData());
            MyActivePort.asking = false;
            if (state.StartsWith("---FDONE~~~"))
            {
                return true;
            }
            else
            {
                MessageBox.Show("SOMETHING WENT WRONG!");
                return false;
            }
        }



        private byte[] StringToByteArray(string str)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetBytes(str);
        }

        private string ByteArrayToString(byte[] arr)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            return enc.GetString(arr);
        }


        private void AnalyzeFile(ref UInt32 InitialOffset, ref UInt32 Len)
        {
            UInt32 Offset = 0;

            // load the file
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(openHexFileDialog.FileName))
            {
                String line;
                UInt32 LastLineAddress = 0;
                UInt32 LastLineByteCount = 0;
                int ln = 0;
                bool firstOffsetDone = false;

                for (int i = 0; i < 270000; i++)
                {
                    sendarr[i] = 0xFF;
                }

                while ((line = sr.ReadLine()) != null)
                {
                    ln++;
                    UInt32 LineByteCount = 0;
                    UInt32 LineAddress = 0;
                    int RecordType = 0;


                    // Process line by line
                    line = line.Trim();
                    // empty line
                    if (line.Length == 0) continue;

                    if (line[0] != ':')
                    {
                        MessageBox.Show("Line does not start with a ':'" + Environment.NewLine + line, "Error during reading HEX file");
                        return;
                    }

                    try
                    {
                        LineByteCount = Convert.ToByte(line.Substring(1, 2), 16);
                        LineAddress = Convert.ToUInt16(line.Substring(3, 4), 16) + Offset;
                        RecordType = Convert.ToByte(line.Substring(7, 2), 16);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        MessageBox.Show("Argument out of Range" + Environment.NewLine + line, "Error during reading HEX file");
                        return;
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Format invalid" + Environment.NewLine + line, "Error during reading HEX file");
                        return;
                    }




                    if (RecordType == 4)
                    {
                        // extract start address offset
                        Offset = Convert.ToUInt32(line.Substring(9, 4), 16) << 16;
                        if (!firstOffsetDone)
                        {
                            InitialOffset = Offset;
                            firstOffsetDone = true;
                        }
                    }
                    else if (RecordType == 0)
                    {
                        // data record
                        Len += LineByteCount;
                        LastLineAddress = LineAddress;
                        LastLineByteCount = LineByteCount;

                        if (LastLineAddress - InitialOffset + LastLineByteCount != Len)
                        {
                            uint diff = LastLineAddress - InitialOffset + LastLineByteCount - Len;
                            //MessageBox.Show("Length mismatch by " + diff.ToString() + Environment.NewLine + "at line " + ln.ToString(), "Error during reading HEX file");
                            if (diff > 0)
                                Len += diff; // correct the length, otherwise we miss the tail!
                            else
                            {
                                MessageBox.Show("Length mismatch by " + diff.ToString() + Environment.NewLine + "at line " + ln.ToString() + Environment.NewLine + "NOT RECOVERABLE !", "Error during reading HEX file");
                                Len = 0;
                                return;
                            }
                        }

                        UInt32 arrpos = LineAddress - InitialOffset;

                        for (int i = 0; i < LineByteCount; i++)
                        {
                            sendarr[arrpos + i] = Convert.ToByte(line.Substring(2 * i + 9, 2), 16);
                        }



                    }
                    else if (RecordType == 1)
                    {
                        // end of records
                        if (LastLineAddress - InitialOffset + LastLineByteCount != Len)
                        {
                            Int64 diff = LastLineAddress - InitialOffset + LastLineByteCount - Len;
                            MessageBox.Show("Length mismatch by " + (diff.ToString()), "Error during reading HEX file");
                        }
                    }


                }
            }
            return;
        }


        private void btnBoot_Click(object sender, EventArgs e)
        {
            serWrite("---BOOT~~~");
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            serWrite("---RES~~~");
        }


        private void node_add(int id, TreeNode ParentNode)
        {
            TreeNode tn = new TreeNode(((ParameterItem)RX_Parameteritems[id]).text);
            tn.Tag = RX_Parameteritems[id];
            for (int i = id + 1; i < RX_Parameteritems.Count; i++)
            {
                if (((ParameterItem)RX_Parameteritems[i]).ParentID == id)
                {
                    node_add(i, tn);
                }
            }
            ParentNode.Nodes.Add(tn);
        }

        private bool CreateParameterView() // return true, if OK
        {
            // create the treeview

            treeView1.Nodes.Clear();
            treeView1.Nodes.Add("root");
            dataGridView1.Rows.Clear();

            if (RX_Parameteritems.Count == 0)
            {
                //MessageBox.Show("No parameters received. Try reconnect");
                return false;
            }
            else
                if (RX_Parameteritems.Count != MyActivePort.m_MenuSize)
                {
                    //MessageBox.Show("Not enough parameters received. Try reconnect");
                    return false;
                }

            // recursive node adding for treeview
            if (RX_Parameteritems.Count > 0)
            {
                node_add(0, treeView1.Nodes[0]);
            }
            else return false;


            TreeNode tn = (TreeNode)treeView1.Nodes[0].Nodes[0].Clone();
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(tn);

            CleanNoChilds(treeView1.Nodes);

            CleanNoChildsII(treeView1.Nodes);// fixme dirty hack, as removing during "foreach" leaves some residue ;-)
            CleanNoChildsII(treeView1.Nodes);// fixme dirty hack, as removing during "foreach" leaves some residue ;-)
            CleanNoChildsII(treeView1.Nodes);// fixme dirty hack, as removing during "foreach" leaves some residue ;-)
            CleanNoChildsII(treeView1.Nodes);// fixme dirty hack, as removing during "foreach" leaves some residue ;-)

            treeView1.ExpandAll();

            return true;
        }

        private void CleanNoChilds(TreeNodeCollection tnc)
        {
            foreach (TreeNode t in tnc)
            {
                if (t.Nodes.Count == 0)
                {
                    t.Text = "";
                }
                else
                {
                    CleanNoChilds(t.Nodes);
                }
            }
        }

        private void CleanNoChildsII(TreeNodeCollection tnc)
        {
            foreach (TreeNode t in tnc)
            {
                if (t.Text == "")
                {
                    t.Remove();
                }
                else
                {
                    CleanNoChildsII(t.Nodes);
                }
            }
        }

        int CurrentTreeID; // the actually shown node in data grid


        // show all parameter childs of this node in the grid view.
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode tn = e.Node;
            ParameterItem pi = (ParameterItem)tn.Tag;
            if (pi != null)
            {
                ShowSubTreePars(pi.ID);
                CurrentTreeID = pi.ID;
            }

        }

        private void UpdateGrid()
        {
            UpdateTree = true; ;
        }




        private void ShowSubTreePars(int id)
        {
            dataGridView1.Rows.Clear();
            for (int i = id; i < RX_Parameteritems.Count; i++)
            {
                ParameterItem pi = (ParameterItem)RX_Parameteritems[i];

                if (pi.ParentID == id && pi.IsPar)
                    AddItemToGrid(i);
            }
        }

        private void AddItemToGrid(int i)
        {

            DataGridViewRow dgr = new DataGridViewRow();
            ParameterItem pi = (ParameterItem)RX_Parameteritems[i];
            if (((ParameterItem)RX_Parameteritems[i]).IsPar)
            {
                dgr.CreateCells(dataGridView1);
                dgr.Cells[0].Value = pi.ID.ToString();
                dgr.Cells[1].Value = pi.text;
                dgr.Tag = pi;
                dgr.Cells[2].Value = pi.Value.ToString();
                dgr.Cells[3].Value = pi.Upper.ToString();
                dgr.Cells[4].Value = pi.Lower.ToString();
            }
            else
            {

            }
            dataGridView1.Rows.Add(dgr);
        }



        private string GetParentString(int id)
        {
            int pid = ((ParameterItem)RX_Parameteritems[id]).ParentID;
            if (pid != 0)
            {
                return GetParentString(pid) + "\\" + ((ParameterItem)RX_Parameteritems[pid]).text;
            }
            else
                return "";
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                ParameterItem pi = (ParameterItem)dataGridView1.Rows[e.RowIndex].Tag;
                try
                {
                    pi.Value = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
                }
                catch
                {
                    MessageBox.Show("No Useful Number: " + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                    dataGridView1.Rows[e.RowIndex].Cells[2].Value = pi.Value; // restore old
                    return;
                }
                //MessageBox.Show(pi.text+" "+pi.Value);

                SendParItem(pi);
            }
        }

        private void SendParItem(ParameterItem pi)
        {
            string str = "---PARVxxx~~~";
            byte[] ba = System.Text.Encoding.ASCII.GetBytes(str);
            ba[7] = (byte)pi.ID;
            ba[8] = (byte)((pi.Value & 0xff00) >> 8);
            ba[9] = (byte)(pi.Value & 0xff);
            serWrite(ba);
        }

        private string[] GetAvailablePorts()
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0) return null;

            for (int i = 0; i < ports.Length; i++)
            {
                if (ports[i].Length > 4 && ports[i][3] != '1')
                {
                    // COM41 etc
                    ports[i] = ports[i].Remove(4);
                }
                if (ports[i].Length > 5)
                {
                    // COM10x etc
                    ports[i] = ports[i].Remove(5);
                }

            }
            return ports;
        }

        private void buttonSaveFile_Click(object sender, EventArgs e)
        {
            if (RX_Parameteritems.Count == 0) return;

            StopStream();

            // populate full path // fixme do this after transmission
            for (int i = 0; i < RX_Parameteritems.Count; i++)
            {
                ParameterItem pi = (ParameterItem)RX_Parameteritems[i];
                pi.fullpath = GetParentString(pi.ID) + "\\" + pi.text;
            }

            // save
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.FCMpar";
            sfd.Filter = "FCM Parameters (*.FCMpar)|*.FCMpar";
            sfd.OverwritePrompt = true;
            sfd.ValidateNames = true;
            DialogResult res = sfd.ShowDialog();
            if (res == DialogResult.OK && sfd.FileName != "")
            {
                // create savefile-text
                StreamWriter sw = File.CreateText(sfd.FileName);
                sw.WriteLine("# FCM Settings " + MyActivePort.m_VersionString);
                for (int i = 0; i < RX_Parameteritems.Count; i++)
                {
                    ParameterItem pi = (ParameterItem)RX_Parameteritems[i];
                    if (pi.IsPar)
                    {
                        sw.WriteLine(pi.fullpath + "\t" + pi.Value.ToString());
                    }
                }
                sw.Close();
            }
        }


        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            if (RX_Parameteritems.Count == 0) return;
            StopStream();

            // populate full path // fixme do this after transmission
            for (int i = 0; i < RX_Parameteritems.Count; i++)
            {
                ParameterItem pi = (ParameterItem)RX_Parameteritems[i];
                pi.fullpath = GetParentString(pi.ID) + "\\" + pi.text;
            }


            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "*.FCMpar";
            ofd.Filter = "FCM Parameters (*.FCMpar)|*.FCMpar";
            ofd.ValidateNames = true;
            DialogResult res = ofd.ShowDialog();
            if (res == DialogResult.OK && ofd.FileName != "")
            {
                StreamReader sr = File.OpenText(ofd.FileName);

                try
                {
                    if (sr.ReadLine().StartsWith("# FCM Settings"))
                    {
                        while (!sr.EndOfStream)
                        {
                            string s = sr.ReadLine();
                            char[] ca = { '\t' };
                            string[] sa = s.Split(ca, 2);

                            for (int i = 0; i < RX_Parameteritems.Count; i++)
                            {
                                ParameterItem pi = (ParameterItem)RX_Parameteritems[i];
                                if (pi.IsPar && pi.fullpath.Equals(sa[0]))
                                {
                                    int v = Convert.ToInt32(sa[1]);
                                    if (v <= pi.Upper && v >= pi.Lower)
                                    {
                                        pi.Value = v;
                                        SendParItem(pi);
                                        Thread.Sleep(10);
                                    }

                                }
                            }
                        }
                        CreateParameterView();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

            }

        }

        public bool UpdateTree;



        private void timerParView_Tick(object sender, EventArgs e)
        {
            if (CurrentTreeID != 0 && UpdateTree)
            {
                ShowSubTreePars(CurrentTreeID);
                UpdateTree = false;
            }
            //timerParView.Stop();

            int searchingNr = 0;

            // update active port connection
            if (PresentCOMPorts.Count == 0)
                searchingNr = -1;

            foreach (SerPort sp in PresentCOMPorts)
            {
                if (sp.GetLinkStatus() == SerPort.e_LinkState.Connected)
                {
                    MyActivePort = sp;
                }
                else if (sp.GetLinkStatus() == SerPort.e_LinkState.Searching)
                {
                    searchingNr++;
                }
            }

            if (MyActivePort != null)
            {
                SerPort.e_CommState cs = MyActivePort.GetCommStatus();

                switch (cs)
                {
                    case SerPort.e_CommState.Finished:
                        buttonConnect.Enabled = true;
                        labelConnected.Text = "FCM " + MyActivePort.m_VersionString;
                        labelConnected.Refresh();
                        break;

                    case SerPort.e_CommState.Found:
                        buttonConnect.Enabled = false;
                        // maybe do auto-connect?
                        comboBox1.Text = MyActivePort.GetName();
                        Properties.Settings.Default.LastComPort = comboBox1.Text;
                        labelConnected.Text = "FCM found";
                        labelConnected.Refresh();

                        if (MyActivePort.m_IsBootloader)
                        {
                            MyActivePort.SetCommStatus(SerPort.e_CommState.Finished);
                            buttonLoadHexFile.Enabled = true;
                            btnReset.Enabled = true;
                            btnBoot.Enabled = false;
                        }
                        else
                        {
                            buttonLoadHexFile.Enabled = false;
                            btnReset.Enabled = false;
                            btnBoot.Enabled = true;
                            if (!HaveParsActual)
                            {
                                RefreshParameters();
                            }
                            else
                            {
                                MyActivePort.SetCommStatus(SerPort.e_CommState.Finished);
                            }
                            buttonConnect.Enabled = true;
                        }
                        break;
                    case SerPort.e_CommState.None:

                        break;
                    case SerPort.e_CommState.ParReading:


                        if (CreateParameterView())
                        {
                            HaveParsActual = true;
                            MyActivePort.SetCommStatus(SerPort.e_CommState.Finished);
                        }
                        else
                        {
                            HaveParsActual = false;
                            MyActivePort.SetCommStatus(SerPort.e_CommState.Found);
                        }
                        break;
                }





            }
            else
            {
                HaveParsActual = false;
                if (searchingNr > 0)
                {
                    buttonConnect.Enabled = false;
                    labelConnected.Text = "searching...";
                    labelConnected.Refresh();
                }
                else if (searchingNr == 0)
                {
                    buttonConnect.Enabled = true;
                    labelConnected.Text = "not connected";
                    labelConnected.Refresh();
                }
                else
                {
                    buttonConnect.Enabled = true;
                    // maybe do auto-connect?
                    labelConnected.Text = "---";
                    labelConnected.Refresh();
                }
            }




            // timerParView.Interval = 2000;
            // timerParView.Start();





        }

        private void RefreshParameters()
        {
            RX_Parameteritems.Clear();
            treeView1.Nodes.Clear();
            if (MyActivePort != null && MyActivePort.GetLinkStatus() == SerPort.e_LinkState.Connected)
            {
                MyActivePort.SetCommStatus(SerPort.e_CommState.ParReading);

                labelConnected.Text = "reading parameters";
                labelConnected.Refresh();

                serWrite("---PARL~~~");
                Thread.Sleep(2000); // enough for 200 Parameters at 10ms each
            }
        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {
            StopStream();
            labelConnected.Text = "not connected";
            if (MyActivePort != null && MyActivePort.GetLinkStatus() == SerPort.e_LinkState.Connected)
            {
                MyActivePort.Close();
            }
        }

        private void buttonRefreshPar_Click(object sender, EventArgs e)
        {
            textBoxMenu.Clear();
            RefreshParameters();
        }

      

    }
}
