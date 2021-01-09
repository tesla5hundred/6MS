namespace USBCANTest
{
	partial class CalibrationForm
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
			this.groupVoltages = new System.Windows.Forms.GroupBox();
			this.txtVLow = new System.Windows.Forms.TextBox();
			this.txtV5 = new System.Windows.Forms.TextBox();
			this.txtV4 = new System.Windows.Forms.TextBox();
			this.txtV3 = new System.Windows.Forms.TextBox();
			this.txtV2 = new System.Windows.Forms.TextBox();
			this.txtV1 = new System.Windows.Forms.TextBox();
			this.txtV0 = new System.Windows.Forms.TextBox();
			this.cmdCalibrate = new System.Windows.Forms.Button();
			this.txtConsole = new System.Windows.Forms.TextBox();
			this.cmdReadCfg = new System.Windows.Forms.Button();
			this.cmdReadVoltages = new System.Windows.Forms.Button();
			this.chkMaster = new System.Windows.Forms.CheckBox();
			this.cmdShowCfg = new System.Windows.Forms.Button();
			this.groupVoltages.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupVoltages
			// 
			this.groupVoltages.Controls.Add(this.txtVLow);
			this.groupVoltages.Controls.Add(this.txtV5);
			this.groupVoltages.Controls.Add(this.txtV4);
			this.groupVoltages.Controls.Add(this.txtV3);
			this.groupVoltages.Controls.Add(this.txtV2);
			this.groupVoltages.Controls.Add(this.txtV1);
			this.groupVoltages.Controls.Add(this.txtV0);
			this.groupVoltages.Location = new System.Drawing.Point(136, 8);
			this.groupVoltages.Name = "groupVoltages";
			this.groupVoltages.Size = new System.Drawing.Size(256, 160);
			this.groupVoltages.TabIndex = 0;
			this.groupVoltages.TabStop = false;
			this.groupVoltages.Text = "Voltages";
			// 
			// txtVLow
			// 
			this.txtVLow.Location = new System.Drawing.Point(168, 72);
			this.txtVLow.Name = "txtVLow";
			this.txtVLow.Size = new System.Drawing.Size(72, 20);
			this.txtVLow.TabIndex = 6;
			this.txtVLow.Text = "2.0529";
			// 
			// txtV5
			// 
			this.txtV5.Location = new System.Drawing.Point(40, 136);
			this.txtV5.Name = "txtV5";
			this.txtV5.Size = new System.Drawing.Size(72, 20);
			this.txtV5.TabIndex = 5;
			this.txtV5.Text = "4.1142";
			// 
			// txtV4
			// 
			this.txtV4.Location = new System.Drawing.Point(40, 112);
			this.txtV4.Name = "txtV4";
			this.txtV4.Size = new System.Drawing.Size(72, 20);
			this.txtV4.TabIndex = 4;
			this.txtV4.Text = "4.0997";
			// 
			// txtV3
			// 
			this.txtV3.Location = new System.Drawing.Point(40, 88);
			this.txtV3.Name = "txtV3";
			this.txtV3.Size = new System.Drawing.Size(72, 20);
			this.txtV3.TabIndex = 3;
			this.txtV3.Text = "4.1141";
			// 
			// txtV2
			// 
			this.txtV2.Location = new System.Drawing.Point(40, 64);
			this.txtV2.Name = "txtV2";
			this.txtV2.Size = new System.Drawing.Size(72, 20);
			this.txtV2.TabIndex = 2;
			this.txtV2.Text = "4.0978";
			// 
			// txtV1
			// 
			this.txtV1.Location = new System.Drawing.Point(40, 40);
			this.txtV1.Name = "txtV1";
			this.txtV1.Size = new System.Drawing.Size(72, 20);
			this.txtV1.TabIndex = 1;
			this.txtV1.Text = "4.0975";
			// 
			// txtV0
			// 
			this.txtV0.Location = new System.Drawing.Point(40, 16);
			this.txtV0.Name = "txtV0";
			this.txtV0.Size = new System.Drawing.Size(72, 20);
			this.txtV0.TabIndex = 0;
			this.txtV0.Text = "4.1140";
			// 
			// cmdCalibrate
			// 
			this.cmdCalibrate.Location = new System.Drawing.Point(16, 8);
			this.cmdCalibrate.Name = "cmdCalibrate";
			this.cmdCalibrate.Size = new System.Drawing.Size(112, 24);
			this.cmdCalibrate.TabIndex = 1;
			this.cmdCalibrate.Text = "Start";
			this.cmdCalibrate.UseVisualStyleBackColor = true;
			this.cmdCalibrate.Click += new System.EventHandler(this.cmdCalibrate_Click);
			// 
			// txtConsole
			// 
			this.txtConsole.Location = new System.Drawing.Point(8, 176);
			this.txtConsole.Multiline = true;
			this.txtConsole.Name = "txtConsole";
			this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtConsole.Size = new System.Drawing.Size(384, 408);
			this.txtConsole.TabIndex = 2;
			// 
			// cmdReadCfg
			// 
			this.cmdReadCfg.Location = new System.Drawing.Point(16, 40);
			this.cmdReadCfg.Name = "cmdReadCfg";
			this.cmdReadCfg.Size = new System.Drawing.Size(112, 24);
			this.cmdReadCfg.TabIndex = 3;
			this.cmdReadCfg.Text = "Read Config";
			this.cmdReadCfg.UseVisualStyleBackColor = true;
			this.cmdReadCfg.Click += new System.EventHandler(this.cmdReadCfg_Click);
			// 
			// cmdReadVoltages
			// 
			this.cmdReadVoltages.Location = new System.Drawing.Point(16, 72);
			this.cmdReadVoltages.Name = "cmdReadVoltages";
			this.cmdReadVoltages.Size = new System.Drawing.Size(112, 24);
			this.cmdReadVoltages.TabIndex = 4;
			this.cmdReadVoltages.Text = "Read Voltages";
			this.cmdReadVoltages.UseVisualStyleBackColor = true;
			this.cmdReadVoltages.Click += new System.EventHandler(this.cmdReadVoltages_Click);
			// 
			// chkMaster
			// 
			this.chkMaster.AutoSize = true;
			this.chkMaster.Location = new System.Drawing.Point(16, 104);
			this.chkMaster.Name = "chkMaster";
			this.chkMaster.Size = new System.Drawing.Size(58, 17);
			this.chkMaster.TabIndex = 5;
			this.chkMaster.Text = "Master";
			this.chkMaster.UseVisualStyleBackColor = true;
			// 
			// cmdShowCfg
			// 
			this.cmdShowCfg.Location = new System.Drawing.Point(440, 32);
			this.cmdShowCfg.Name = "cmdShowCfg";
			this.cmdShowCfg.Size = new System.Drawing.Size(88, 23);
			this.cmdShowCfg.TabIndex = 6;
			this.cmdShowCfg.Text = "Configuration";
			this.cmdShowCfg.UseVisualStyleBackColor = true;
			this.cmdShowCfg.Click += new System.EventHandler(this.cmdShowCfg_Click);
			// 
			// CalibrationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(588, 592);
			this.Controls.Add(this.cmdShowCfg);
			this.Controls.Add(this.chkMaster);
			this.Controls.Add(this.cmdReadVoltages);
			this.Controls.Add(this.cmdReadCfg);
			this.Controls.Add(this.txtConsole);
			this.Controls.Add(this.cmdCalibrate);
			this.Controls.Add(this.groupVoltages);
			this.Name = "CalibrationForm";
			this.Text = "Calibration";
			this.groupVoltages.ResumeLayout(false);
			this.groupVoltages.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupVoltages;
		private System.Windows.Forms.TextBox txtV0;
		private System.Windows.Forms.TextBox txtVLow;
		private System.Windows.Forms.TextBox txtV5;
		private System.Windows.Forms.TextBox txtV4;
		private System.Windows.Forms.TextBox txtV3;
		private System.Windows.Forms.TextBox txtV2;
		private System.Windows.Forms.TextBox txtV1;
		private System.Windows.Forms.Button cmdCalibrate;
		private System.Windows.Forms.TextBox txtConsole;
		private System.Windows.Forms.Button cmdReadCfg;
		private System.Windows.Forms.Button cmdReadVoltages;
		private System.Windows.Forms.CheckBox chkMaster;
		private System.Windows.Forms.Button cmdShowCfg;
	}
}