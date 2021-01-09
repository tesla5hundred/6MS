namespace USBCANTest
{
    partial class Form1
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
            this.cmdConnect = new System.Windows.Forms.Button();
            this.cmdTx = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colCell = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVoltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoadEn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.cmdSyncPackData = new System.Windows.Forms.Button();
            this.cmdShowCal = new System.Windows.Forms.Button();
            this.cmdLive = new System.Windows.Forms.Button();
            this.tmrLive = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblAmpHours = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblTemp = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCurrent = new System.Windows.Forms.Label();
            this.lblErrorCode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTotalV = new System.Windows.Forms.Label();
            this.lblAvgV = new System.Windows.Forms.Label();
            this.lblMaxV = new System.Windows.Forms.Label();
            this.lblMinV = new System.Windows.Forms.Label();
            this.cmdConn = new System.Windows.Forms.Button();
            this.groupBoxLogging = new System.Windows.Forms.GroupBox();
            this.chkLogging = new System.Windows.Forms.CheckBox();
            this.cmdSetLoggingPath = new System.Windows.Forms.Button();
            this.txtLoggingPath = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBoxLogging.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdConnect
            // 
            this.cmdConnect.Location = new System.Drawing.Point(57, 31);
            this.cmdConnect.Name = "cmdConnect";
            this.cmdConnect.Size = new System.Drawing.Size(99, 33);
            this.cmdConnect.TabIndex = 1;
            this.cmdConnect.Text = "Connect";
            this.cmdConnect.UseVisualStyleBackColor = true;
            this.cmdConnect.Click += new System.EventHandler(this.cmdConnect_Click);
            // 
            // cmdTx
            // 
            this.cmdTx.Location = new System.Drawing.Point(71, 99);
            this.cmdTx.Name = "cmdTx";
            this.cmdTx.Size = new System.Drawing.Size(75, 23);
            this.cmdTx.TabIndex = 2;
            this.cmdTx.Text = "Transmit";
            this.cmdTx.UseVisualStyleBackColor = true;
            this.cmdTx.Click += new System.EventHandler(this.cmdTx_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCell,
            this.colVoltage,
            this.colLoadEn,
            this.colTemp});
            this.dataGridView1.Location = new System.Drawing.Point(544, 24);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(408, 480);
            this.dataGridView1.TabIndex = 3;
            // 
            // colCell
            // 
            this.colCell.HeaderText = "Cell";
            this.colCell.Name = "colCell";
            this.colCell.ReadOnly = true;
            // 
            // colVoltage
            // 
            this.colVoltage.HeaderText = "Voltage";
            this.colVoltage.Name = "colVoltage";
            this.colVoltage.ReadOnly = true;
            // 
            // colLoadEn
            // 
            this.colLoadEn.FalseValue = "false";
            this.colLoadEn.HeaderText = "Load En";
            this.colLoadEn.Name = "colLoadEn";
            this.colLoadEn.ReadOnly = true;
            this.colLoadEn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.colLoadEn.TrueValue = "true";
            this.colLoadEn.Width = 50;
            // 
            // colTemp
            // 
            this.colTemp.HeaderText = "Temp";
            this.colTemp.Name = "colTemp";
            this.colTemp.ReadOnly = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(427, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmdSyncPackData
            // 
            this.cmdSyncPackData.Location = new System.Drawing.Point(250, 99);
            this.cmdSyncPackData.Name = "cmdSyncPackData";
            this.cmdSyncPackData.Size = new System.Drawing.Size(123, 23);
            this.cmdSyncPackData.TabIndex = 5;
            this.cmdSyncPackData.Text = "Sync pack data";
            this.cmdSyncPackData.UseVisualStyleBackColor = true;
            this.cmdSyncPackData.Click += new System.EventHandler(this.cmdSyncPackData_Click);
            // 
            // cmdShowCal
            // 
            this.cmdShowCal.Location = new System.Drawing.Point(56, 264);
            this.cmdShowCal.Name = "cmdShowCal";
            this.cmdShowCal.Size = new System.Drawing.Size(72, 24);
            this.cmdShowCal.TabIndex = 6;
            this.cmdShowCal.Text = "Calibration";
            this.cmdShowCal.UseVisualStyleBackColor = true;
            this.cmdShowCal.Click += new System.EventHandler(this.cmdShowCal_Click);
            // 
            // cmdLive
            // 
            this.cmdLive.Location = new System.Drawing.Point(312, 64);
            this.cmdLive.Name = "cmdLive";
            this.cmdLive.Size = new System.Drawing.Size(104, 23);
            this.cmdLive.TabIndex = 7;
            this.cmdLive.Text = "Start Live Data";
            this.cmdLive.UseVisualStyleBackColor = true;
            this.cmdLive.Click += new System.EventHandler(this.cmdLive_Click);
            // 
            // tmrLive
            // 
            this.tmrLive.Interval = 500;
            this.tmrLive.Tick += new System.EventHandler(this.tmrLive_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lblAmpHours);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblTemp);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lblCurrent);
            this.groupBox1.Controls.Add(this.lblErrorCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lblTotalV);
            this.groupBox1.Controls.Add(this.lblAvgV);
            this.groupBox1.Controls.Add(this.lblMaxV);
            this.groupBox1.Controls.Add(this.lblMinV);
            this.groupBox1.Location = new System.Drawing.Point(224, 256);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 232);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pack Data";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(80, 184);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Ah";
            // 
            // lblAmpHours
            // 
            this.lblAmpHours.AutoSize = true;
            this.lblAmpHours.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAmpHours.Location = new System.Drawing.Point(112, 184);
            this.lblAmpHours.Name = "lblAmpHours";
            this.lblAmpHours.Size = new System.Drawing.Size(37, 15);
            this.lblAmpHours.TabIndex = 13;
            this.lblAmpHours.Text = "label1";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(64, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Temp";
            // 
            // lblTemp
            // 
            this.lblTemp.AutoSize = true;
            this.lblTemp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTemp.Location = new System.Drawing.Point(112, 160);
            this.lblTemp.Name = "lblTemp";
            this.lblTemp.Size = new System.Drawing.Size(37, 15);
            this.lblTemp.TabIndex = 11;
            this.lblTemp.Text = "label1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(64, 136);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Current";
            // 
            // lblCurrent
            // 
            this.lblCurrent.AutoSize = true;
            this.lblCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblCurrent.Location = new System.Drawing.Point(112, 136);
            this.lblCurrent.Name = "lblCurrent";
            this.lblCurrent.Size = new System.Drawing.Size(37, 15);
            this.lblCurrent.TabIndex = 9;
            this.lblCurrent.Text = "label1";
            // 
            // lblErrorCode
            // 
            this.lblErrorCode.AutoSize = true;
            this.lblErrorCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblErrorCode.Location = new System.Drawing.Point(112, 208);
            this.lblErrorCode.MinimumSize = new System.Drawing.Size(80, 2);
            this.lblErrorCode.Name = "lblErrorCode";
            this.lblErrorCode.Size = new System.Drawing.Size(80, 15);
            this.lblErrorCode.TabIndex = 8;
            this.lblErrorCode.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Total Pack Voltage";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Avg Cell Voltage";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Max Cell Voltage";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Min Cell Voltage";
            // 
            // lblTotalV
            // 
            this.lblTotalV.AutoSize = true;
            this.lblTotalV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTotalV.Location = new System.Drawing.Point(112, 112);
            this.lblTotalV.Name = "lblTotalV";
            this.lblTotalV.Size = new System.Drawing.Size(37, 15);
            this.lblTotalV.TabIndex = 3;
            this.lblTotalV.Text = "label1";
            // 
            // lblAvgV
            // 
            this.lblAvgV.AutoSize = true;
            this.lblAvgV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAvgV.Location = new System.Drawing.Point(112, 88);
            this.lblAvgV.Name = "lblAvgV";
            this.lblAvgV.Size = new System.Drawing.Size(37, 15);
            this.lblAvgV.TabIndex = 2;
            this.lblAvgV.Text = "label1";
            // 
            // lblMaxV
            // 
            this.lblMaxV.AutoSize = true;
            this.lblMaxV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMaxV.Location = new System.Drawing.Point(112, 64);
            this.lblMaxV.Name = "lblMaxV";
            this.lblMaxV.Size = new System.Drawing.Size(37, 15);
            this.lblMaxV.TabIndex = 1;
            this.lblMaxV.Text = "label1";
            // 
            // lblMinV
            // 
            this.lblMinV.AutoSize = true;
            this.lblMinV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMinV.Location = new System.Drawing.Point(112, 40);
            this.lblMinV.Name = "lblMinV";
            this.lblMinV.Size = new System.Drawing.Size(37, 15);
            this.lblMinV.TabIndex = 0;
            this.lblMinV.Text = "label1";
            // 
            // cmdConn
            // 
            this.cmdConn.Location = new System.Drawing.Point(208, 32);
            this.cmdConn.Name = "cmdConn";
            this.cmdConn.Size = new System.Drawing.Size(75, 23);
            this.cmdConn.TabIndex = 9;
            this.cmdConn.Text = "Conn";
            this.cmdConn.UseVisualStyleBackColor = true;
            this.cmdConn.Click += new System.EventHandler(this.cmdConn_Click);
            // 
            // groupBoxLogging
            // 
            this.groupBoxLogging.Controls.Add(this.txtLoggingPath);
            this.groupBoxLogging.Controls.Add(this.cmdSetLoggingPath);
            this.groupBoxLogging.Controls.Add(this.chkLogging);
            this.groupBoxLogging.Location = new System.Drawing.Point(129, 141);
            this.groupBoxLogging.Name = "groupBoxLogging";
            this.groupBoxLogging.Size = new System.Drawing.Size(407, 100);
            this.groupBoxLogging.TabIndex = 10;
            this.groupBoxLogging.TabStop = false;
            this.groupBoxLogging.Text = "Logging";
            // 
            // chkLogging
            // 
            this.chkLogging.AutoSize = true;
            this.chkLogging.Location = new System.Drawing.Point(22, 64);
            this.chkLogging.Name = "chkLogging";
            this.chkLogging.Size = new System.Drawing.Size(100, 17);
            this.chkLogging.TabIndex = 0;
            this.chkLogging.Text = "Enable Logging";
            this.chkLogging.UseVisualStyleBackColor = true;
            // 
            // cmdSetLoggingPath
            // 
            this.cmdSetLoggingPath.Location = new System.Drawing.Point(178, 60);
            this.cmdSetLoggingPath.Name = "cmdSetLoggingPath";
            this.cmdSetLoggingPath.Size = new System.Drawing.Size(95, 23);
            this.cmdSetLoggingPath.TabIndex = 1;
            this.cmdSetLoggingPath.Text = "Set Logging File";
            this.cmdSetLoggingPath.UseVisualStyleBackColor = true;
            this.cmdSetLoggingPath.Click += new System.EventHandler(this.cmdSetLoggingPath_Click);
            // 
            // txtLoggingPath
            // 
            this.txtLoggingPath.Location = new System.Drawing.Point(22, 36);
            this.txtLoggingPath.Name = "txtLoggingPath";
            this.txtLoggingPath.Size = new System.Drawing.Size(251, 20);
            this.txtLoggingPath.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 621);
            this.Controls.Add(this.groupBoxLogging);
            this.Controls.Add(this.cmdConn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdLive);
            this.Controls.Add(this.cmdShowCal);
            this.Controls.Add(this.cmdSyncPackData);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmdTx);
            this.Controls.Add(this.cmdConnect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxLogging.ResumeLayout(false);
            this.groupBoxLogging.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.Button cmdConnect;
		private System.Windows.Forms.Button cmdTx;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button cmdSyncPackData;
		private System.Windows.Forms.Button cmdShowCal;
		private System.Windows.Forms.Button cmdLive;
		private System.Windows.Forms.Timer tmrLive;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblTotalV;
		private System.Windows.Forms.Label lblAvgV;
		private System.Windows.Forms.Label lblMaxV;
		private System.Windows.Forms.Label lblMinV;
		private System.Windows.Forms.Label lblErrorCode;
		private System.Windows.Forms.DataGridViewTextBoxColumn colCell;
		private System.Windows.Forms.DataGridViewTextBoxColumn colVoltage;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colLoadEn;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTemp;
		private System.Windows.Forms.Button cmdConn;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblCurrent;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblTemp;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lblAmpHours;
        private System.Windows.Forms.GroupBox groupBoxLogging;
        private System.Windows.Forms.TextBox txtLoggingPath;
        private System.Windows.Forms.Button cmdSetLoggingPath;
        private System.Windows.Forms.CheckBox chkLogging;
    }
}

