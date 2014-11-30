namespace FCM_Manager
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.textBoxMenu = new System.Windows.Forms.TextBox();
            this.btnGetData = new System.Windows.Forms.Button();
            this.btnStopData = new System.Windows.Forms.Button();
            this.btn_Osc_ACC = new System.Windows.Forms.Button();
            this.btn_osc_Gyro = new System.Windows.Forms.Button();
            this.btn_osc_mag = new System.Windows.Forms.Button();
            this.btn_osc_gov = new System.Windows.Forms.Button();
            this.lbl_Frames = new System.Windows.Forms.Label();
            this.btnGetQuat = new System.Windows.Forms.Button();
            this.btn_mnuplus = new System.Windows.Forms.Button();
            this.btn_mnuEnt = new System.Windows.Forms.Button();
            this.btn_mnu_minus = new System.Windows.Forms.Button();
            this.btn_mnu0 = new System.Windows.Forms.Button();
            this.btn_ShowActual = new System.Windows.Forms.Button();
            this.btnSet = new System.Windows.Forms.Button();
            this.btnErr3D = new System.Windows.Forms.Button();
            this.btn_mnuplusplus = new System.Windows.Forms.Button();
            this.btn_mnu_minusminus = new System.Windows.Forms.Button();
            this.openHexFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.buttonLoadHexFile = new System.Windows.Forms.Button();
            this.btnBoot = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColuName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColuValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColUpper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSaveFile = new System.Windows.Forms.Button();
            this.buttonLoadFile = new System.Windows.Forms.Button();
            this.timerParView = new System.Windows.Forms.Timer(this.components);
            this.labelConnected = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.buttonRefreshPar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(12, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 21);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // textBoxMenu
            // 
            this.textBoxMenu.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMenu.Location = new System.Drawing.Point(12, 37);
            this.textBoxMenu.Multiline = true;
            this.textBoxMenu.Name = "textBoxMenu";
            this.textBoxMenu.ReadOnly = true;
            this.textBoxMenu.Size = new System.Drawing.Size(237, 140);
            this.textBoxMenu.TabIndex = 2;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(349, 12);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(75, 48);
            this.btnGetData.TabIndex = 4;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // btnStopData
            // 
            this.btnStopData.Location = new System.Drawing.Point(455, 183);
            this.btnStopData.Name = "btnStopData";
            this.btnStopData.Size = new System.Drawing.Size(75, 21);
            this.btnStopData.TabIndex = 4;
            this.btnStopData.Text = "Stop Data";
            this.btnStopData.UseVisualStyleBackColor = true;
            this.btnStopData.Click += new System.EventHandler(this.btnStopData_Click);
            // 
            // btn_Osc_ACC
            // 
            this.btn_Osc_ACC.Location = new System.Drawing.Point(349, 66);
            this.btn_Osc_ACC.Name = "btn_Osc_ACC";
            this.btn_Osc_ACC.Size = new System.Drawing.Size(75, 21);
            this.btn_Osc_ACC.TabIndex = 5;
            this.btn_Osc_ACC.Text = "ACC";
            this.btn_Osc_ACC.UseVisualStyleBackColor = true;
            this.btn_Osc_ACC.Click += new System.EventHandler(this.btn_Osc_ACC_Click);
            // 
            // btn_osc_Gyro
            // 
            this.btn_osc_Gyro.Location = new System.Drawing.Point(349, 93);
            this.btn_osc_Gyro.Name = "btn_osc_Gyro";
            this.btn_osc_Gyro.Size = new System.Drawing.Size(75, 21);
            this.btn_osc_Gyro.TabIndex = 5;
            this.btn_osc_Gyro.Text = "Gyro";
            this.btn_osc_Gyro.UseVisualStyleBackColor = true;
            this.btn_osc_Gyro.Click += new System.EventHandler(this.btn_osc_Gyro_Click);
            // 
            // btn_osc_mag
            // 
            this.btn_osc_mag.Location = new System.Drawing.Point(349, 120);
            this.btn_osc_mag.Name = "btn_osc_mag";
            this.btn_osc_mag.Size = new System.Drawing.Size(75, 21);
            this.btn_osc_mag.TabIndex = 5;
            this.btn_osc_mag.Text = "MAG";
            this.btn_osc_mag.UseVisualStyleBackColor = true;
            this.btn_osc_mag.Click += new System.EventHandler(this.btn_osc_mag_Click);
            // 
            // btn_osc_gov
            // 
            this.btn_osc_gov.Location = new System.Drawing.Point(349, 147);
            this.btn_osc_gov.Name = "btn_osc_gov";
            this.btn_osc_gov.Size = new System.Drawing.Size(181, 21);
            this.btn_osc_gov.TabIndex = 5;
            this.btn_osc_gov.Text = "GOV";
            this.btn_osc_gov.UseVisualStyleBackColor = true;
            this.btn_osc_gov.Click += new System.EventHandler(this.btn_osc_gov_Click);
            // 
            // lbl_Frames
            // 
            this.lbl_Frames.AutoSize = true;
            this.lbl_Frames.Location = new System.Drawing.Point(346, 187);
            this.lbl_Frames.Name = "lbl_Frames";
            this.lbl_Frames.Size = new System.Drawing.Size(41, 13);
            this.lbl_Frames.TabIndex = 6;
            this.lbl_Frames.Text = "Frames";
            // 
            // btnGetQuat
            // 
            this.btnGetQuat.Location = new System.Drawing.Point(455, 12);
            this.btnGetQuat.Name = "btnGetQuat";
            this.btnGetQuat.Size = new System.Drawing.Size(75, 48);
            this.btnGetQuat.TabIndex = 4;
            this.btnGetQuat.Text = "Get Quaternions";
            this.btnGetQuat.UseVisualStyleBackColor = true;
            this.btnGetQuat.Click += new System.EventHandler(this.btnGetQuat_Click);
            // 
            // btn_mnuplus
            // 
            this.btn_mnuplus.Location = new System.Drawing.Point(129, 183);
            this.btn_mnuplus.Name = "btn_mnuplus";
            this.btn_mnuplus.Size = new System.Drawing.Size(30, 21);
            this.btn_mnuplus.TabIndex = 7;
            this.btn_mnuplus.Text = "+";
            this.btn_mnuplus.UseVisualStyleBackColor = true;
            this.btn_mnuplus.Click += new System.EventHandler(this.btn_mnuplus_Click);
            // 
            // btn_mnuEnt
            // 
            this.btn_mnuEnt.Location = new System.Drawing.Point(84, 183);
            this.btn_mnuEnt.Name = "btn_mnuEnt";
            this.btn_mnuEnt.Size = new System.Drawing.Size(39, 21);
            this.btn_mnuEnt.TabIndex = 7;
            this.btn_mnuEnt.Text = "ENT";
            this.btn_mnuEnt.UseVisualStyleBackColor = true;
            this.btn_mnuEnt.Click += new System.EventHandler(this.btn_mnuEnt_Click);
            // 
            // btn_mnu_minus
            // 
            this.btn_mnu_minus.Location = new System.Drawing.Point(48, 183);
            this.btn_mnu_minus.Name = "btn_mnu_minus";
            this.btn_mnu_minus.Size = new System.Drawing.Size(30, 21);
            this.btn_mnu_minus.TabIndex = 7;
            this.btn_mnu_minus.Text = "-";
            this.btn_mnu_minus.UseVisualStyleBackColor = true;
            this.btn_mnu_minus.Click += new System.EventHandler(this.btn_mnu_minus_Click);
            // 
            // btn_mnu0
            // 
            this.btn_mnu0.Location = new System.Drawing.Point(207, 183);
            this.btn_mnu0.Name = "btn_mnu0";
            this.btn_mnu0.Size = new System.Drawing.Size(42, 21);
            this.btn_mnu0.TabIndex = 7;
            this.btn_mnu0.Text = "MNU";
            this.btn_mnu0.UseVisualStyleBackColor = true;
            this.btn_mnu0.Click += new System.EventHandler(this.btn_mnu0_Click);
            // 
            // btn_ShowActual
            // 
            this.btn_ShowActual.Location = new System.Drawing.Point(455, 66);
            this.btn_ShowActual.Name = "btn_ShowActual";
            this.btn_ShowActual.Size = new System.Drawing.Size(75, 21);
            this.btn_ShowActual.TabIndex = 5;
            this.btn_ShowActual.Text = "Actual 3D";
            this.btn_ShowActual.UseVisualStyleBackColor = true;
            this.btn_ShowActual.Click += new System.EventHandler(this.btn_ShowActual_Click);
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(455, 93);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 21);
            this.btnSet.TabIndex = 5;
            this.btnSet.Text = "Setpoint 3D";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnErr3D
            // 
            this.btnErr3D.Location = new System.Drawing.Point(455, 120);
            this.btnErr3D.Name = "btnErr3D";
            this.btnErr3D.Size = new System.Drawing.Size(75, 21);
            this.btnErr3D.TabIndex = 5;
            this.btnErr3D.Text = "Sim 3D";
            this.btnErr3D.UseVisualStyleBackColor = true;
            this.btnErr3D.Click += new System.EventHandler(this.btnErr3D_Click);
            // 
            // btn_mnuplusplus
            // 
            this.btn_mnuplusplus.Location = new System.Drawing.Point(165, 183);
            this.btn_mnuplusplus.Name = "btn_mnuplusplus";
            this.btn_mnuplusplus.Size = new System.Drawing.Size(30, 21);
            this.btn_mnuplusplus.TabIndex = 7;
            this.btn_mnuplusplus.Text = "++";
            this.btn_mnuplusplus.UseVisualStyleBackColor = true;
            this.btn_mnuplusplus.Click += new System.EventHandler(this.btn_mnuplusplus_Click);
            // 
            // btn_mnu_minusminus
            // 
            this.btn_mnu_minusminus.Location = new System.Drawing.Point(12, 183);
            this.btn_mnu_minusminus.Name = "btn_mnu_minusminus";
            this.btn_mnu_minusminus.Size = new System.Drawing.Size(30, 21);
            this.btn_mnu_minusminus.TabIndex = 7;
            this.btn_mnu_minusminus.Text = "--";
            this.btn_mnu_minusminus.UseVisualStyleBackColor = true;
            this.btn_mnu_minusminus.Click += new System.EventHandler(this.btn_mnu_minusminus_Click);
            // 
            // openHexFileDialog
            // 
            this.openHexFileDialog.Filter = "\"Intel Hex\"|*.hex";
            this.openHexFileDialog.SupportMultiDottedExtensions = true;
            this.openHexFileDialog.Title = "Open Hex File";
            // 
            // buttonLoadHexFile
            // 
            this.buttonLoadHexFile.Enabled = false;
            this.buttonLoadHexFile.Location = new System.Drawing.Point(255, 93);
            this.buttonLoadHexFile.Name = "buttonLoadHexFile";
            this.buttonLoadHexFile.Size = new System.Drawing.Size(75, 48);
            this.buttonLoadHexFile.TabIndex = 8;
            this.buttonLoadHexFile.Text = "load HEX file";
            this.buttonLoadHexFile.UseVisualStyleBackColor = true;
            this.buttonLoadHexFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnBoot
            // 
            this.btnBoot.Enabled = false;
            this.btnBoot.Location = new System.Drawing.Point(255, 39);
            this.btnBoot.Name = "btnBoot";
            this.btnBoot.Size = new System.Drawing.Size(75, 21);
            this.btnBoot.TabIndex = 5;
            this.btnBoot.Text = "Bootloader";
            this.btnBoot.UseVisualStyleBackColor = true;
            this.btnBoot.Click += new System.EventHandler(this.btnBoot_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 210);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(236, 229);
            this.treeView1.TabIndex = 9;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColID,
            this.ColuName,
            this.ColuValue,
            this.ColUpper,
            this.ColLower});
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(255, 210);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(316, 256);
            this.dataGridView1.TabIndex = 10;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // ColID
            // 
            this.ColID.FillWeight = 76.14214F;
            this.ColID.HeaderText = "ID";
            this.ColID.Name = "ColID";
            this.ColID.ReadOnly = true;
            // 
            // ColuName
            // 
            this.ColuName.FillWeight = 105.9645F;
            this.ColuName.HeaderText = "Name";
            this.ColuName.Name = "ColuName";
            this.ColuName.ReadOnly = true;
            // 
            // ColuValue
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColuValue.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColuValue.FillWeight = 105.9645F;
            this.ColuValue.HeaderText = "Value";
            this.ColuValue.Name = "ColuValue";
            // 
            // ColUpper
            // 
            this.ColUpper.FillWeight = 105.9645F;
            this.ColUpper.HeaderText = "Upper";
            this.ColUpper.Name = "ColUpper";
            this.ColUpper.ReadOnly = true;
            // 
            // ColLower
            // 
            this.ColLower.FillWeight = 105.9645F;
            this.ColLower.HeaderText = "Lower";
            this.ColLower.Name = "ColLower";
            this.ColLower.ReadOnly = true;
            // 
            // buttonSaveFile
            // 
            this.buttonSaveFile.Location = new System.Drawing.Point(95, 445);
            this.buttonSaveFile.Name = "buttonSaveFile";
            this.buttonSaveFile.Size = new System.Drawing.Size(75, 21);
            this.buttonSaveFile.TabIndex = 5;
            this.buttonSaveFile.Text = "Save Par";
            this.buttonSaveFile.UseVisualStyleBackColor = true;
            this.buttonSaveFile.Click += new System.EventHandler(this.buttonSaveFile_Click);
            // 
            // buttonLoadFile
            // 
            this.buttonLoadFile.Location = new System.Drawing.Point(176, 445);
            this.buttonLoadFile.Name = "buttonLoadFile";
            this.buttonLoadFile.Size = new System.Drawing.Size(75, 21);
            this.buttonLoadFile.TabIndex = 5;
            this.buttonLoadFile.Text = "Load Par";
            this.buttonLoadFile.UseVisualStyleBackColor = true;
            this.buttonLoadFile.Click += new System.EventHandler(this.buttonLoadFile_Click);
            // 
            // timerParView
            // 
            this.timerParView.Enabled = true;
            this.timerParView.Interval = 250;
            this.timerParView.Tick += new System.EventHandler(this.timerParView_Tick);
            // 
            // labelConnected
            // 
            this.labelConnected.AutoSize = true;
            this.labelConnected.Location = new System.Drawing.Point(92, 16);
            this.labelConnected.Name = "labelConnected";
            this.labelConnected.Size = new System.Drawing.Size(16, 13);
            this.labelConnected.TabIndex = 11;
            this.labelConnected.Text = "---";
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(192, 10);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(57, 21);
            this.comboBox1.TabIndex = 12;
            // 
            // btnReset
            // 
            this.btnReset.Enabled = false;
            this.btnReset.Location = new System.Drawing.Point(255, 66);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 21);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // button_disconnect
            // 
            this.button_disconnect.Location = new System.Drawing.Point(255, 12);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(75, 21);
            this.button_disconnect.TabIndex = 0;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.button_disconnect_Click);
            // 
            // buttonRefreshPar
            // 
            this.buttonRefreshPar.Location = new System.Drawing.Point(12, 445);
            this.buttonRefreshPar.Name = "buttonRefreshPar";
            this.buttonRefreshPar.Size = new System.Drawing.Size(75, 21);
            this.buttonRefreshPar.TabIndex = 5;
            this.buttonRefreshPar.Text = "Refresh Par";
            this.buttonRefreshPar.UseVisualStyleBackColor = true;
            this.buttonRefreshPar.Click += new System.EventHandler(this.buttonRefreshPar_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 478);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelConnected);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.buttonLoadHexFile);
            this.Controls.Add(this.btn_mnu_minusminus);
            this.Controls.Add(this.btn_mnu_minus);
            this.Controls.Add(this.btn_mnuEnt);
            this.Controls.Add(this.btn_mnu0);
            this.Controls.Add(this.btn_mnuplusplus);
            this.Controls.Add(this.btn_mnuplus);
            this.Controls.Add(this.lbl_Frames);
            this.Controls.Add(this.btn_osc_gov);
            this.Controls.Add(this.btn_osc_mag);
            this.Controls.Add(this.buttonLoadFile);
            this.Controls.Add(this.buttonRefreshPar);
            this.Controls.Add(this.buttonSaveFile);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnBoot);
            this.Controls.Add(this.btn_osc_Gyro);
            this.Controls.Add(this.btnErr3D);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.btn_ShowActual);
            this.Controls.Add(this.btn_Osc_ACC);
            this.Controls.Add(this.btnStopData);
            this.Controls.Add(this.btnGetQuat);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.textBoxMenu);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.buttonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.Text = "FCM Manager 2.1.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.TextBox textBoxMenu;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.Button btnStopData;
        private System.Windows.Forms.Button btn_Osc_ACC;
        private System.Windows.Forms.Button btn_osc_Gyro;
        private System.Windows.Forms.Button btn_osc_mag;
        private System.Windows.Forms.Button btn_osc_gov;
        private System.Windows.Forms.Label lbl_Frames;
        private System.Windows.Forms.Button btnGetQuat;
        private System.Windows.Forms.Button btn_mnuplus;
        private System.Windows.Forms.Button btn_mnuEnt;
        private System.Windows.Forms.Button btn_mnu_minus;
        private System.Windows.Forms.Button btn_mnu0;
        private System.Windows.Forms.Button btn_ShowActual;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Button btnErr3D;
        private System.Windows.Forms.Button btn_mnuplusplus;
        private System.Windows.Forms.Button btn_mnu_minusminus;
        private System.Windows.Forms.OpenFileDialog openHexFileDialog;
        private System.Windows.Forms.Button buttonLoadHexFile;
        private System.Windows.Forms.Button btnBoot;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonSaveFile;
        private System.Windows.Forms.Button buttonLoadFile;
        private System.Windows.Forms.Timer timerParView;
        private System.Windows.Forms.Label labelConnected;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.Button buttonRefreshPar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColuName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColuValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColUpper;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLower;
    }
}

