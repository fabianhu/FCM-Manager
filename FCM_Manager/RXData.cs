using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FCM_Manager
{
    class RXData
    {
       // public fixed char hdr[4];			// ---D
        public Int32 gx, gy, gz;				// Gyro
        public Int32 ax, ay, az;				// Accelerometer
        public Int32 mx, my, mz;				// Magneto
        public Int32 gov_x, gov_y, gov_z;	// Governor out
        public Int32 RC_x, RC_y, RC_z, RC_a;	// RC command
        public Int32 h;					// height in mm (barometric measurement)
        public Int16 temp0;		// Temperature in 10th °C
        public Int16 temp1;		// Temperature in 10th °C
       // public fixed char footer[4];		// ~~~0
    }
    class RXQuat
    {
        public float[] qAct, qSet, qSim;
        public float[] vPos;
        public Int32 h;					// height in mm (barometric measurement)
        public Int16 temp0;		// Temperature in 10th °C
        public Int16 temp1;		// Temperature in 10th °C

        public RXQuat()
        {
            qAct = new float[4];
            qSet = new float[4];
            qSim = new float[4];
            vPos = new float[3];
        }
    }
    class ParameterItem
    {
        public int ID;
        public int ParentID;
        public int Value;
        public int Upper;
        public int Lower;
        public string text;
        public bool IsPar;
        public string fullpath;
        public bool NeedsUpdate; // unused
    }
}
