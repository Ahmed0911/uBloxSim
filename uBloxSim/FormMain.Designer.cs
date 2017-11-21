namespace uBloxSim
{
    partial class FormMain
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
            this.pictureBoxMap = new System.Windows.Forms.PictureBox();
            this.timer20Hz = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxLatitude = new System.Windows.Forms.TextBox();
            this.textBoxLongitude = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxLongitudeMovement = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxLatitudeMovement = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBoxDirection = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxSpeed = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.comboBoxPorts = new System.Windows.Forms.ComboBox();
            this.checkBoxSendHNRPVT = new System.Windows.Forms.CheckBox();
            this.serialPortSim = new System.IO.Ports.SerialPort(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDirection)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxMap
            // 
            this.pictureBoxMap.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxMap.Name = "pictureBoxMap";
            this.pictureBoxMap.Size = new System.Drawing.Size(1800, 1395);
            this.pictureBoxMap.TabIndex = 45;
            this.pictureBoxMap.TabStop = false;
            this.pictureBoxMap.Click += new System.EventHandler(this.pictureBoxMap_Click);
            // 
            // timer20Hz
            // 
            this.timer20Hz.Enabled = true;
            this.timer20Hz.Interval = 50;
            this.timer20Hz.Tick += new System.EventHandler(this.timer20Hz_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxLongitude);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxLatitude);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(1837, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 127);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Latitude:";
            // 
            // textBoxLatitude
            // 
            this.textBoxLatitude.Location = new System.Drawing.Point(109, 31);
            this.textBoxLatitude.Name = "textBoxLatitude";
            this.textBoxLatitude.ReadOnly = true;
            this.textBoxLatitude.Size = new System.Drawing.Size(149, 26);
            this.textBoxLatitude.TabIndex = 1;
            // 
            // textBoxLongitude
            // 
            this.textBoxLongitude.Location = new System.Drawing.Point(109, 70);
            this.textBoxLongitude.Name = "textBoxLongitude";
            this.textBoxLongitude.ReadOnly = true;
            this.textBoxLongitude.Size = new System.Drawing.Size(149, 26);
            this.textBoxLongitude.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Longitude:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxSpeed);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.pictureBoxDirection);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.textBoxLongitudeMovement);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxLatitudeMovement);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(1837, 160);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 407);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Movement";
            // 
            // textBoxLongitudeMovement
            // 
            this.textBoxLongitudeMovement.Location = new System.Drawing.Point(109, 73);
            this.textBoxLongitudeMovement.Name = "textBoxLongitudeMovement";
            this.textBoxLongitudeMovement.ReadOnly = true;
            this.textBoxLongitudeMovement.Size = new System.Drawing.Size(149, 26);
            this.textBoxLongitudeMovement.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Longitude:";
            // 
            // textBoxLatitudeMovement
            // 
            this.textBoxLatitudeMovement.Location = new System.Drawing.Point(109, 34);
            this.textBoxLatitudeMovement.Name = "textBoxLatitudeMovement";
            this.textBoxLatitudeMovement.ReadOnly = true;
            this.textBoxLatitudeMovement.Size = new System.Drawing.Size(149, 26);
            this.textBoxLatitudeMovement.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Latitude:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Speed:";
            // 
            // pictureBoxDirection
            // 
            this.pictureBoxDirection.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxDirection.Location = new System.Drawing.Point(35, 188);
            this.pictureBoxDirection.Name = "pictureBoxDirection";
            this.pictureBoxDirection.Size = new System.Drawing.Size(200, 200);
            this.pictureBoxDirection.TabIndex = 5;
            this.pictureBoxDirection.TabStop = false;
            this.pictureBoxDirection.Click += new System.EventHandler(this.pictureBoxDirection_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 20);
            this.label6.TabIndex = 6;
            this.label6.Text = "Direction:";
            // 
            // comboBoxSpeed
            // 
            this.comboBoxSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSpeed.FormattingEnabled = true;
            this.comboBoxSpeed.Items.AddRange(new object[] {
            "Stop",
            "10 m/s",
            "100 m/s",
            "1000 m/s",
            "10k m/s",
            "100k m/s",
            "Warp"});
            this.comboBoxSpeed.Location = new System.Drawing.Point(85, 113);
            this.comboBoxSpeed.Name = "comboBoxSpeed";
            this.comboBoxSpeed.Size = new System.Drawing.Size(173, 28);
            this.comboBoxSpeed.TabIndex = 7;
            this.comboBoxSpeed.SelectedIndexChanged += new System.EventHandler(this.comboBoxSpeed_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxSendHNRPVT);
            this.groupBox3.Controls.Add(this.buttonOpen);
            this.groupBox3.Controls.Add(this.comboBoxPorts);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(1837, 613);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 160);
            this.groupBox3.TabIndex = 48;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Serial Comm";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(19, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 20);
            this.label8.TabIndex = 0;
            this.label8.Text = "Port:";
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(188, 37);
            this.buttonOpen.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(70, 35);
            this.buttonOpen.TabIndex = 4;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // comboBoxPorts
            // 
            this.comboBoxPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPorts.FormattingEnabled = true;
            this.comboBoxPorts.Location = new System.Drawing.Point(68, 41);
            this.comboBoxPorts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxPorts.Name = "comboBoxPorts";
            this.comboBoxPorts.Size = new System.Drawing.Size(103, 28);
            this.comboBoxPorts.TabIndex = 3;
            // 
            // checkBoxSendHNRPVT
            // 
            this.checkBoxSendHNRPVT.AutoSize = true;
            this.checkBoxSendHNRPVT.Location = new System.Drawing.Point(23, 91);
            this.checkBoxSendHNRPVT.Name = "checkBoxSendHNRPVT";
            this.checkBoxSendHNRPVT.Size = new System.Drawing.Size(105, 24);
            this.checkBoxSendHNRPVT.TabIndex = 5;
            this.checkBoxSendHNRPVT.Text = "HNR-PVT";
            this.checkBoxSendHNRPVT.UseVisualStyleBackColor = true;
            // 
            // serialPortSim
            // 
            this.serialPortSim.BaudRate = 115200;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2160, 1456);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBoxMap);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormMain";
            this.Text = "uBloxSim";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMap)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDirection)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBoxMap;
        private System.Windows.Forms.Timer timer20Hz;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxLongitude;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLatitude;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBoxDirection;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxLongitudeMovement;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxLatitudeMovement;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBoxSendHNRPVT;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.ComboBox comboBoxPorts;
        private System.Windows.Forms.Label label8;
        private System.IO.Ports.SerialPort serialPortSim;
    }
}

