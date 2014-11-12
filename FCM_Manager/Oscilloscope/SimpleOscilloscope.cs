using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSC_DLL;
using System.IO;
using System.Windows.Forms;

namespace FCM_Manager
{
    /// <summary>
    /// SimpleOscilloscope class.
    /// </summary>
    /// <remarks>
    /// Requires Osc_DLL.dll version 2.8.2, see http://www.oscilloscope-lib.com/.
    /// </remarks> 
    public class SimpleOscilloscope
    {
        /// <summary>
        /// Private Oscilloscope.
        /// </summary>
        private Oscilloscope oscilloscope;

        /// <summary>
        /// Private caption text.
        /// </summary>
        private string privCaption;

        /// <summary>
        /// Gets or sets the Oscilloscope caption text.
        /// </summary>
        public string Caption
        {
            get
            {
                return privCaption;
            }
            set
            {
                if (value != privCaption)
                {
                    privCaption = value;
                    //oscilloscope.Caption = privCaption;
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SimpleOscilloscope"/> class.
        /// </summary>
        public SimpleOscilloscope(string caption, string scopeSettingsFileName)
        {
            if (!File.Exists(scopeSettingsFileName))
                MessageBox.Show("File not Found: " + scopeSettingsFileName); 
            oscilloscope = Oscilloscope.Create(scopeSettingsFileName, "");

            Caption = caption;
        }

        /// <summary>
        /// Shows the Oscilloscope.
        /// </summary>
        public void Show()
        {
            if (oscilloscope == null) return;
            oscilloscope.Show();
        }

        /// <summary>
        /// Hides the Oscilloscope.
        /// </summary>
        public void Hide()
        {
            if (oscilloscope == null) return;
            oscilloscope.Hide();
        }


        /// <summary>
        /// Show/Hides the Oscilloscope.
        /// </summary>
        public void ShowHide()
        {
            if (oscilloscope == null) return;
            if (oscilloscope.visible)
                oscilloscope.Hide();
            else
                oscilloscope.Show();
        }

        /// <summary>
        /// Adds trace data to the Oscilloscope.
        /// </summary>
        public void AddData(double beam1, double beam2, double beam3)
        {
            if (oscilloscope == null) return;
            oscilloscope.AddData(beam1, beam2, beam3);
        }

        public void Dispose()
        {
            if (oscilloscope != null)
                oscilloscope.Dispose();
        }

    }
}
