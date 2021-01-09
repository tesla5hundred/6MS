namespace USBCANTest
{
	partial class ConfigForm
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
			this.txtCellCount = new System.Windows.Forms.TextBox();
			this.lblCellCount = new System.Windows.Forms.Label();
			this.updnBoardCount = new System.Windows.Forms.NumericUpDown();
			this.txtBoardCANID = new System.Windows.Forms.TextBox();
			this.txtCANDstID = new System.Windows.Forms.TextBox();
			this.txtBalPollPeriod = new System.Windows.Forms.TextBox();
			this.txtBalReplyTimeout = new System.Windows.Forms.TextBox();
			this.txtCellVoltageBalance = new System.Windows.Forms.TextBox();
			this.txtBalanceOffset = new System.Windows.Forms.TextBox();
			this.txtCellVoltageMax = new System.Windows.Forms.TextBox();
			this.txtCellVoltageMin = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.cmdRead = new System.Windows.Forms.Button();
			this.cmdWrite = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.radioChgEnActHigh = new System.Windows.Forms.RadioButton();
			this.radioChgEnActLow = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioAFS1Digital = new System.Windows.Forms.RadioButton();
			this.radioAFS1Analog = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.radioAFS1PolarityLower = new System.Windows.Forms.RadioButton();
			this.radioAFS1PolarityHigher = new System.Windows.Forms.RadioButton();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.radioAFS1DischargeLim = new System.Windows.Forms.RadioButton();
			this.radioAFS1ChargeLim = new System.Windows.Forms.RadioButton();
			this.chkAFS1Enable = new System.Windows.Forms.CheckBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.chkAFS2Enable = new System.Windows.Forms.CheckBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.radioAFS2PolarityLower = new System.Windows.Forms.RadioButton();
			this.radioAFS2PolarityHigher = new System.Windows.Forms.RadioButton();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.radioAFS2DischargeLim = new System.Windows.Forms.RadioButton();
			this.radioAFS2ChargeLim = new System.Windows.Forms.RadioButton();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.radioAFS2Digital = new System.Windows.Forms.RadioButton();
			this.radioAFS2Analog = new System.Windows.Forms.RadioButton();
			this.txtMaxVFilterFreq = new System.Windows.Forms.TextBox();
			this.txtMinVFilterFreq = new System.Windows.Forms.TextBox();
			this.txtDischargeLimPGain = new System.Windows.Forms.TextBox();
			this.txtChargeLimPGain = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.colBalancer = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colCell1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colCell2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colCell3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colCell4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colCell5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.colCell6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.cmdDefault = new System.Windows.Forms.Button();
			this.txtCurrentGain = new System.Windows.Forms.TextBox();
			this.txtCurrentOffset = new System.Windows.Forms.TextBox();
			this.label16 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.updnBoardCount)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.groupBox7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtCellCount
			// 
			this.txtCellCount.Location = new System.Drawing.Point(200, 40);
			this.txtCellCount.Name = "txtCellCount";
			this.txtCellCount.Size = new System.Drawing.Size(64, 20);
			this.txtCellCount.TabIndex = 0;
			this.txtCellCount.Text = "12";
			// 
			// lblCellCount
			// 
			this.lblCellCount.AutoSize = true;
			this.lblCellCount.Location = new System.Drawing.Point(48, 40);
			this.lblCellCount.Name = "lblCellCount";
			this.lblCellCount.Size = new System.Drawing.Size(97, 13);
			this.lblCellCount.TabIndex = 1;
			this.lblCellCount.Text = "Cell Count (3 - 255)";
			// 
			// updnBoardCount
			// 
			this.updnBoardCount.Location = new System.Drawing.Point(200, 72);
			this.updnBoardCount.Maximum = new decimal(new int[] {
            42,
            0,
            0,
            0});
			this.updnBoardCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.updnBoardCount.Name = "updnBoardCount";
			this.updnBoardCount.Size = new System.Drawing.Size(64, 20);
			this.updnBoardCount.TabIndex = 4;
			this.updnBoardCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.updnBoardCount.ValueChanged += new System.EventHandler(this.updnBoardCount_ValueChanged);
			// 
			// txtBoardCANID
			// 
			this.txtBoardCANID.Location = new System.Drawing.Point(200, 152);
			this.txtBoardCANID.Name = "txtBoardCANID";
			this.txtBoardCANID.Size = new System.Drawing.Size(72, 20);
			this.txtBoardCANID.TabIndex = 5;
			this.txtBoardCANID.Text = "0000000A";
			// 
			// txtCANDstID
			// 
			this.txtCANDstID.Location = new System.Drawing.Point(200, 176);
			this.txtCANDstID.Name = "txtCANDstID";
			this.txtCANDstID.Size = new System.Drawing.Size(72, 20);
			this.txtCANDstID.TabIndex = 6;
			this.txtCANDstID.Text = "0F000000";
			// 
			// txtBalPollPeriod
			// 
			this.txtBalPollPeriod.Location = new System.Drawing.Point(200, 208);
			this.txtBalPollPeriod.Name = "txtBalPollPeriod";
			this.txtBalPollPeriod.Size = new System.Drawing.Size(56, 20);
			this.txtBalPollPeriod.TabIndex = 7;
			this.txtBalPollPeriod.Text = "1000";
			// 
			// txtBalReplyTimeout
			// 
			this.txtBalReplyTimeout.Location = new System.Drawing.Point(200, 232);
			this.txtBalReplyTimeout.Name = "txtBalReplyTimeout";
			this.txtBalReplyTimeout.Size = new System.Drawing.Size(56, 20);
			this.txtBalReplyTimeout.TabIndex = 8;
			this.txtBalReplyTimeout.Text = "3000";
			// 
			// txtCellVoltageBalance
			// 
			this.txtCellVoltageBalance.Location = new System.Drawing.Point(200, 264);
			this.txtCellVoltageBalance.Name = "txtCellVoltageBalance";
			this.txtCellVoltageBalance.Size = new System.Drawing.Size(56, 20);
			this.txtCellVoltageBalance.TabIndex = 9;
			// 
			// txtBalanceOffset
			// 
			this.txtBalanceOffset.Location = new System.Drawing.Point(200, 288);
			this.txtBalanceOffset.Name = "txtBalanceOffset";
			this.txtBalanceOffset.Size = new System.Drawing.Size(56, 20);
			this.txtBalanceOffset.TabIndex = 10;
			// 
			// txtCellVoltageMax
			// 
			this.txtCellVoltageMax.Location = new System.Drawing.Point(200, 312);
			this.txtCellVoltageMax.Name = "txtCellVoltageMax";
			this.txtCellVoltageMax.Size = new System.Drawing.Size(56, 20);
			this.txtCellVoltageMax.TabIndex = 11;
			// 
			// txtCellVoltageMin
			// 
			this.txtCellVoltageMin.Location = new System.Drawing.Point(200, 336);
			this.txtCellVoltageMin.Name = "txtCellVoltageMin";
			this.txtCellVoltageMin.Size = new System.Drawing.Size(56, 20);
			this.txtCellVoltageMin.TabIndex = 12;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(48, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(96, 13);
			this.label1.TabIndex = 13;
			this.label1.Text = "Cell Boards (1 - 42)";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(48, 152);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(74, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "Board CAN ID";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(48, 176);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Remote CAN ID";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(48, 208);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(124, 13);
			this.label4.TabIndex = 16;
			this.label4.Text = "Balancer Poll Period (ms)";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(48, 232);
			this.label5.Name = "label5";
			this.label5.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label5.Size = new System.Drawing.Size(142, 13);
			this.label5.TabIndex = 17;
			this.label5.Text = "Balancer Reply Timeout (ms)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(48, 264);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(127, 13);
			this.label6.TabIndex = 18;
			this.label6.Text = "Minimum balance voltage";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(48, 288);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(77, 13);
			this.label7.TabIndex = 19;
			this.label7.Text = "Balance Offset";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(48, 312);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(109, 13);
			this.label8.TabIndex = 20;
			this.label8.Text = "Maximum cell Voltage";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(48, 336);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(107, 13);
			this.label9.TabIndex = 21;
			this.label9.Text = "Minimum Cell Voltage";
			// 
			// cmdRead
			// 
			this.cmdRead.Location = new System.Drawing.Point(8, 8);
			this.cmdRead.Name = "cmdRead";
			this.cmdRead.Size = new System.Drawing.Size(75, 23);
			this.cmdRead.TabIndex = 22;
			this.cmdRead.Text = "Read";
			this.cmdRead.UseVisualStyleBackColor = true;
			this.cmdRead.Click += new System.EventHandler(this.cmdRead_Click);
			// 
			// cmdWrite
			// 
			this.cmdWrite.Location = new System.Drawing.Point(88, 8);
			this.cmdWrite.Name = "cmdWrite";
			this.cmdWrite.Size = new System.Drawing.Size(75, 23);
			this.cmdWrite.TabIndex = 23;
			this.cmdWrite.Text = "Write";
			this.cmdWrite.UseVisualStyleBackColor = true;
			this.cmdWrite.Click += new System.EventHandler(this.cmdWrite_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.radioChgEnActHigh);
			this.groupBox1.Controls.Add(this.radioChgEnActLow);
			this.groupBox1.Location = new System.Drawing.Point(200, 360);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(120, 56);
			this.groupBox1.TabIndex = 24;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Charge Enable";
			// 
			// radioChgEnActHigh
			// 
			this.radioChgEnActHigh.AutoSize = true;
			this.radioChgEnActHigh.Location = new System.Drawing.Point(8, 32);
			this.radioChgEnActHigh.Name = "radioChgEnActHigh";
			this.radioChgEnActHigh.Size = new System.Drawing.Size(95, 17);
			this.radioChgEnActHigh.TabIndex = 1;
			this.radioChgEnActHigh.TabStop = true;
			this.radioChgEnActHigh.Text = "Float to enable";
			this.radioChgEnActHigh.UseVisualStyleBackColor = true;
			// 
			// radioChgEnActLow
			// 
			this.radioChgEnActLow.AutoSize = true;
			this.radioChgEnActLow.Location = new System.Drawing.Point(8, 16);
			this.radioChgEnActLow.Name = "radioChgEnActLow";
			this.radioChgEnActLow.Size = new System.Drawing.Size(107, 17);
			this.radioChgEnActLow.TabIndex = 0;
			this.radioChgEnActLow.TabStop = true;
			this.radioChgEnActLow.Text = "Ground to enable";
			this.radioChgEnActLow.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.radioAFS1Digital);
			this.groupBox2.Controls.Add(this.radioAFS1Analog);
			this.groupBox2.Location = new System.Drawing.Point(264, 432);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(96, 64);
			this.groupBox2.TabIndex = 25;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Output mode";
			// 
			// radioAFS1Digital
			// 
			this.radioAFS1Digital.AutoSize = true;
			this.radioAFS1Digital.Location = new System.Drawing.Point(8, 32);
			this.radioAFS1Digital.Name = "radioAFS1Digital";
			this.radioAFS1Digital.Size = new System.Drawing.Size(77, 17);
			this.radioAFS1Digital.TabIndex = 1;
			this.radioAFS1Digital.TabStop = true;
			this.radioAFS1Digital.Text = "TTL Digital";
			this.radioAFS1Digital.UseVisualStyleBackColor = true;
			// 
			// radioAFS1Analog
			// 
			this.radioAFS1Analog.AutoSize = true;
			this.radioAFS1Analog.Location = new System.Drawing.Point(8, 16);
			this.radioAFS1Analog.Name = "radioAFS1Analog";
			this.radioAFS1Analog.Size = new System.Drawing.Size(58, 17);
			this.radioAFS1Analog.TabIndex = 0;
			this.radioAFS1Analog.TabStop = true;
			this.radioAFS1Analog.Text = "Analog";
			this.radioAFS1Analog.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.radioAFS1PolarityLower);
			this.groupBox3.Controls.Add(this.radioAFS1PolarityHigher);
			this.groupBox3.Location = new System.Drawing.Point(360, 432);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(176, 64);
			this.groupBox3.TabIndex = 26;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Polarity";
			// 
			// radioAFS1PolarityLower
			// 
			this.radioAFS1PolarityLower.AutoSize = true;
			this.radioAFS1PolarityLower.Location = new System.Drawing.Point(8, 32);
			this.radioAFS1PolarityLower.Name = "radioAFS1PolarityLower";
			this.radioAFS1PolarityLower.Size = new System.Drawing.Size(161, 17);
			this.radioAFS1PolarityLower.TabIndex = 1;
			this.radioAFS1PolarityLower.TabStop = true;
			this.radioAFS1PolarityLower.Text = "Lower signal decreases |Ibat|";
			this.radioAFS1PolarityLower.UseVisualStyleBackColor = true;
			// 
			// radioAFS1PolarityHigher
			// 
			this.radioAFS1PolarityHigher.AutoSize = true;
			this.radioAFS1PolarityHigher.Location = new System.Drawing.Point(8, 16);
			this.radioAFS1PolarityHigher.Name = "radioAFS1PolarityHigher";
			this.radioAFS1PolarityHigher.Size = new System.Drawing.Size(162, 17);
			this.radioAFS1PolarityHigher.TabIndex = 0;
			this.radioAFS1PolarityHigher.TabStop = true;
			this.radioAFS1PolarityHigher.Text = "Larger signal decreases |Ibat|";
			this.radioAFS1PolarityHigher.UseVisualStyleBackColor = true;
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.radioAFS1DischargeLim);
			this.groupBox5.Controls.Add(this.radioAFS1ChargeLim);
			this.groupBox5.Location = new System.Drawing.Point(536, 432);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(112, 64);
			this.groupBox5.TabIndex = 27;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Function";
			// 
			// radioAFS1DischargeLim
			// 
			this.radioAFS1DischargeLim.AutoSize = true;
			this.radioAFS1DischargeLim.Location = new System.Drawing.Point(8, 32);
			this.radioAFS1DischargeLim.Name = "radioAFS1DischargeLim";
			this.radioAFS1DischargeLim.Size = new System.Drawing.Size(97, 17);
			this.radioAFS1DischargeLim.TabIndex = 1;
			this.radioAFS1DischargeLim.TabStop = true;
			this.radioAFS1DischargeLim.Text = "Discharge Limit";
			this.radioAFS1DischargeLim.UseVisualStyleBackColor = true;
			// 
			// radioAFS1ChargeLim
			// 
			this.radioAFS1ChargeLim.AutoSize = true;
			this.radioAFS1ChargeLim.Location = new System.Drawing.Point(8, 16);
			this.radioAFS1ChargeLim.Name = "radioAFS1ChargeLim";
			this.radioAFS1ChargeLim.Size = new System.Drawing.Size(83, 17);
			this.radioAFS1ChargeLim.TabIndex = 0;
			this.radioAFS1ChargeLim.TabStop = true;
			this.radioAFS1ChargeLim.Text = "Charge Limit";
			this.radioAFS1ChargeLim.UseVisualStyleBackColor = true;
			// 
			// chkAFS1Enable
			// 
			this.chkAFS1Enable.AutoSize = true;
			this.chkAFS1Enable.Location = new System.Drawing.Point(200, 456);
			this.chkAFS1Enable.Name = "chkAFS1Enable";
			this.chkAFS1Enable.Size = new System.Drawing.Size(59, 17);
			this.chkAFS1Enable.TabIndex = 29;
			this.chkAFS1Enable.Text = "Enable";
			this.chkAFS1Enable.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(48, 456);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(49, 13);
			this.label10.TabIndex = 30;
			this.label10.Text = "Analog 1";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(48, 528);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(49, 13);
			this.label11.TabIndex = 31;
			this.label11.Text = "Analog 2";
			// 
			// chkAFS2Enable
			// 
			this.chkAFS2Enable.AutoSize = true;
			this.chkAFS2Enable.Location = new System.Drawing.Point(200, 520);
			this.chkAFS2Enable.Name = "chkAFS2Enable";
			this.chkAFS2Enable.Size = new System.Drawing.Size(59, 17);
			this.chkAFS2Enable.TabIndex = 35;
			this.chkAFS2Enable.Text = "Enable";
			this.chkAFS2Enable.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.radioAFS2PolarityLower);
			this.groupBox4.Controls.Add(this.radioAFS2PolarityHigher);
			this.groupBox4.Location = new System.Drawing.Point(360, 496);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(176, 64);
			this.groupBox4.TabIndex = 33;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Polarity";
			// 
			// radioAFS2PolarityLower
			// 
			this.radioAFS2PolarityLower.AutoSize = true;
			this.radioAFS2PolarityLower.Location = new System.Drawing.Point(8, 32);
			this.radioAFS2PolarityLower.Name = "radioAFS2PolarityLower";
			this.radioAFS2PolarityLower.Size = new System.Drawing.Size(161, 17);
			this.radioAFS2PolarityLower.TabIndex = 1;
			this.radioAFS2PolarityLower.TabStop = true;
			this.radioAFS2PolarityLower.Text = "Lower signal decreases |Ibat|";
			this.radioAFS2PolarityLower.UseVisualStyleBackColor = true;
			// 
			// radioAFS2PolarityHigher
			// 
			this.radioAFS2PolarityHigher.AutoSize = true;
			this.radioAFS2PolarityHigher.Location = new System.Drawing.Point(8, 16);
			this.radioAFS2PolarityHigher.Name = "radioAFS2PolarityHigher";
			this.radioAFS2PolarityHigher.Size = new System.Drawing.Size(162, 17);
			this.radioAFS2PolarityHigher.TabIndex = 0;
			this.radioAFS2PolarityHigher.TabStop = true;
			this.radioAFS2PolarityHigher.Text = "Larger signal decreases |Ibat|";
			this.radioAFS2PolarityHigher.UseVisualStyleBackColor = true;
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.radioAFS2DischargeLim);
			this.groupBox6.Controls.Add(this.radioAFS2ChargeLim);
			this.groupBox6.Location = new System.Drawing.Point(536, 496);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(112, 64);
			this.groupBox6.TabIndex = 34;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Function";
			// 
			// radioAFS2DischargeLim
			// 
			this.radioAFS2DischargeLim.AutoSize = true;
			this.radioAFS2DischargeLim.Location = new System.Drawing.Point(8, 32);
			this.radioAFS2DischargeLim.Name = "radioAFS2DischargeLim";
			this.radioAFS2DischargeLim.Size = new System.Drawing.Size(97, 17);
			this.radioAFS2DischargeLim.TabIndex = 1;
			this.radioAFS2DischargeLim.TabStop = true;
			this.radioAFS2DischargeLim.Text = "Discharge Limit";
			this.radioAFS2DischargeLim.UseVisualStyleBackColor = true;
			// 
			// radioAFS2ChargeLim
			// 
			this.radioAFS2ChargeLim.AutoSize = true;
			this.radioAFS2ChargeLim.Location = new System.Drawing.Point(8, 16);
			this.radioAFS2ChargeLim.Name = "radioAFS2ChargeLim";
			this.radioAFS2ChargeLim.Size = new System.Drawing.Size(83, 17);
			this.radioAFS2ChargeLim.TabIndex = 0;
			this.radioAFS2ChargeLim.TabStop = true;
			this.radioAFS2ChargeLim.Text = "Charge Limit";
			this.radioAFS2ChargeLim.UseVisualStyleBackColor = true;
			// 
			// groupBox7
			// 
			this.groupBox7.Controls.Add(this.radioAFS2Digital);
			this.groupBox7.Controls.Add(this.radioAFS2Analog);
			this.groupBox7.Location = new System.Drawing.Point(264, 496);
			this.groupBox7.Name = "groupBox7";
			this.groupBox7.Size = new System.Drawing.Size(96, 64);
			this.groupBox7.TabIndex = 32;
			this.groupBox7.TabStop = false;
			this.groupBox7.Text = "Output mode";
			// 
			// radioAFS2Digital
			// 
			this.radioAFS2Digital.AutoSize = true;
			this.radioAFS2Digital.Location = new System.Drawing.Point(8, 32);
			this.radioAFS2Digital.Name = "radioAFS2Digital";
			this.radioAFS2Digital.Size = new System.Drawing.Size(77, 17);
			this.radioAFS2Digital.TabIndex = 1;
			this.radioAFS2Digital.TabStop = true;
			this.radioAFS2Digital.Text = "TTL Digital";
			this.radioAFS2Digital.UseVisualStyleBackColor = true;
			// 
			// radioAFS2Analog
			// 
			this.radioAFS2Analog.AutoSize = true;
			this.radioAFS2Analog.Location = new System.Drawing.Point(8, 16);
			this.radioAFS2Analog.Name = "radioAFS2Analog";
			this.radioAFS2Analog.Size = new System.Drawing.Size(58, 17);
			this.radioAFS2Analog.TabIndex = 0;
			this.radioAFS2Analog.TabStop = true;
			this.radioAFS2Analog.Text = "Analog";
			this.radioAFS2Analog.UseVisualStyleBackColor = true;
			// 
			// txtMaxVFilterFreq
			// 
			this.txtMaxVFilterFreq.Location = new System.Drawing.Point(200, 568);
			this.txtMaxVFilterFreq.Name = "txtMaxVFilterFreq";
			this.txtMaxVFilterFreq.Size = new System.Drawing.Size(56, 20);
			this.txtMaxVFilterFreq.TabIndex = 36;
			this.txtMaxVFilterFreq.Text = "6000";
			// 
			// txtMinVFilterFreq
			// 
			this.txtMinVFilterFreq.Location = new System.Drawing.Point(200, 592);
			this.txtMinVFilterFreq.Name = "txtMinVFilterFreq";
			this.txtMinVFilterFreq.Size = new System.Drawing.Size(56, 20);
			this.txtMinVFilterFreq.TabIndex = 37;
			this.txtMinVFilterFreq.Text = "6000";
			// 
			// txtDischargeLimPGain
			// 
			this.txtDischargeLimPGain.Location = new System.Drawing.Point(200, 640);
			this.txtDischargeLimPGain.Name = "txtDischargeLimPGain";
			this.txtDischargeLimPGain.Size = new System.Drawing.Size(56, 20);
			this.txtDischargeLimPGain.TabIndex = 39;
			this.txtDischargeLimPGain.Text = "300";
			// 
			// txtChargeLimPGain
			// 
			this.txtChargeLimPGain.Location = new System.Drawing.Point(200, 616);
			this.txtChargeLimPGain.Name = "txtChargeLimPGain";
			this.txtChargeLimPGain.Size = new System.Drawing.Size(56, 20);
			this.txtChargeLimPGain.TabIndex = 38;
			this.txtChargeLimPGain.Text = "300";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(48, 568);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(111, 13);
			this.label12.TabIndex = 40;
			this.label12.Text = "Charge Limit Filter freq";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(48, 592);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(125, 13);
			this.label13.TabIndex = 41;
			this.label13.Text = "Discharge Limit Filter freq";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(48, 640);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(114, 13);
			this.label14.TabIndex = 42;
			this.label14.Text = "Discharge Limit P Gain";
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(48, 616);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(100, 13);
			this.label15.TabIndex = 43;
			this.label15.Text = "Charge Limit P Gain";
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBalancer,
            this.colCell1,
            this.colCell2,
            this.colCell3,
            this.colCell4,
            this.colCell5,
            this.colCell6});
			this.dataGridView1.Location = new System.Drawing.Point(288, 8);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dataGridView1.Size = new System.Drawing.Size(440, 344);
			this.dataGridView1.TabIndex = 46;
			this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
			this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
			// 
			// colBalancer
			// 
			this.colBalancer.HeaderText = "Balancer";
			this.colBalancer.Name = "colBalancer";
			this.colBalancer.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colBalancer.Width = 75;
			// 
			// colCell1
			// 
			this.colCell1.FalseValue = "false";
			this.colCell1.HeaderText = "Cell 1";
			this.colCell1.Name = "colCell1";
			this.colCell1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCell1.TrueValue = "true";
			this.colCell1.Width = 50;
			// 
			// colCell2
			// 
			this.colCell2.FalseValue = "false";
			this.colCell2.HeaderText = "Cell 2";
			this.colCell2.Name = "colCell2";
			this.colCell2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCell2.TrueValue = "true";
			this.colCell2.Width = 50;
			// 
			// colCell3
			// 
			this.colCell3.FalseValue = "false";
			this.colCell3.HeaderText = "Cell 3";
			this.colCell3.Name = "colCell3";
			this.colCell3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCell3.TrueValue = "true";
			this.colCell3.Width = 50;
			// 
			// colCell4
			// 
			this.colCell4.FalseValue = "false";
			this.colCell4.HeaderText = "Cell 4";
			this.colCell4.Name = "colCell4";
			this.colCell4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCell4.TrueValue = "true";
			this.colCell4.Width = 50;
			// 
			// colCell5
			// 
			this.colCell5.FalseValue = "false";
			this.colCell5.HeaderText = "Cell 5";
			this.colCell5.Name = "colCell5";
			this.colCell5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCell5.TrueValue = "true";
			this.colCell5.Width = 50;
			// 
			// colCell6
			// 
			this.colCell6.FalseValue = "false";
			this.colCell6.HeaderText = "Cell 6";
			this.colCell6.Name = "colCell6";
			this.colCell6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colCell6.TrueValue = "true";
			this.colCell6.Width = 50;
			// 
			// cmdDefault
			// 
			this.cmdDefault.Location = new System.Drawing.Point(168, 8);
			this.cmdDefault.Name = "cmdDefault";
			this.cmdDefault.Size = new System.Drawing.Size(96, 23);
			this.cmdDefault.TabIndex = 47;
			this.cmdDefault.Text = "Reset to Default";
			this.cmdDefault.UseVisualStyleBackColor = true;
			this.cmdDefault.Click += new System.EventHandler(this.cmdDefault_Click);
			// 
			// txtCurrentGain
			// 
			this.txtCurrentGain.Location = new System.Drawing.Point(200, 664);
			this.txtCurrentGain.Name = "txtCurrentGain";
			this.txtCurrentGain.Size = new System.Drawing.Size(56, 20);
			this.txtCurrentGain.TabIndex = 48;
			this.txtCurrentGain.Text = "300";
			// 
			// txtCurrentOffset
			// 
			this.txtCurrentOffset.Location = new System.Drawing.Point(200, 688);
			this.txtCurrentOffset.Name = "txtCurrentOffset";
			this.txtCurrentOffset.Size = new System.Drawing.Size(56, 20);
			this.txtCurrentOffset.TabIndex = 49;
			this.txtCurrentOffset.Text = "300";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Location = new System.Drawing.Point(48, 664);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(66, 13);
			this.label16.TabIndex = 50;
			this.label16.Text = "Current Gain";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Location = new System.Drawing.Point(48, 688);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(72, 13);
			this.label17.TabIndex = 51;
			this.label17.Text = "Current Offset";
			// 
			// ConfigForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(911, 750);
			this.Controls.Add(this.label17);
			this.Controls.Add(this.label16);
			this.Controls.Add(this.txtCurrentOffset);
			this.Controls.Add(this.txtCurrentGain);
			this.Controls.Add(this.cmdDefault);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label15);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.label13);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.txtDischargeLimPGain);
			this.Controls.Add(this.txtChargeLimPGain);
			this.Controls.Add(this.txtMinVFilterFreq);
			this.Controls.Add(this.txtMaxVFilterFreq);
			this.Controls.Add(this.chkAFS2Enable);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox6);
			this.Controls.Add(this.groupBox7);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.chkAFS1Enable);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox5);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cmdWrite);
			this.Controls.Add(this.cmdRead);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtCellVoltageMin);
			this.Controls.Add(this.txtCellVoltageMax);
			this.Controls.Add(this.txtBalanceOffset);
			this.Controls.Add(this.txtCellVoltageBalance);
			this.Controls.Add(this.txtBalReplyTimeout);
			this.Controls.Add(this.txtBalPollPeriod);
			this.Controls.Add(this.txtCANDstID);
			this.Controls.Add(this.txtBoardCANID);
			this.Controls.Add(this.updnBoardCount);
			this.Controls.Add(this.lblCellCount);
			this.Controls.Add(this.txtCellCount);
			this.Name = "ConfigForm";
			this.Text = "ConfigForm";
			this.Load += new System.EventHandler(this.ConfigForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.updnBoardCount)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox6.ResumeLayout(false);
			this.groupBox6.PerformLayout();
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtCellCount;
		private System.Windows.Forms.Label lblCellCount;
		private System.Windows.Forms.NumericUpDown updnBoardCount;
		private System.Windows.Forms.TextBox txtBoardCANID;
		private System.Windows.Forms.TextBox txtCANDstID;
		private System.Windows.Forms.TextBox txtBalPollPeriod;
		private System.Windows.Forms.TextBox txtBalReplyTimeout;
		private System.Windows.Forms.TextBox txtCellVoltageBalance;
		private System.Windows.Forms.TextBox txtBalanceOffset;
		private System.Windows.Forms.TextBox txtCellVoltageMax;
		private System.Windows.Forms.TextBox txtCellVoltageMin;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button cmdRead;
		private System.Windows.Forms.Button cmdWrite;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton radioChgEnActHigh;
		private System.Windows.Forms.RadioButton radioChgEnActLow;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.RadioButton radioAFS1Digital;
		private System.Windows.Forms.RadioButton radioAFS1Analog;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.RadioButton radioAFS1PolarityLower;
		private System.Windows.Forms.RadioButton radioAFS1PolarityHigher;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.RadioButton radioAFS1DischargeLim;
		private System.Windows.Forms.RadioButton radioAFS1ChargeLim;
		private System.Windows.Forms.CheckBox chkAFS1Enable;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.CheckBox chkAFS2Enable;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.RadioButton radioAFS2PolarityLower;
		private System.Windows.Forms.RadioButton radioAFS2PolarityHigher;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.RadioButton radioAFS2DischargeLim;
		private System.Windows.Forms.RadioButton radioAFS2ChargeLim;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.RadioButton radioAFS2Digital;
		private System.Windows.Forms.RadioButton radioAFS2Analog;
		private System.Windows.Forms.TextBox txtMaxVFilterFreq;
		private System.Windows.Forms.TextBox txtMinVFilterFreq;
		private System.Windows.Forms.TextBox txtDischargeLimPGain;
		private System.Windows.Forms.TextBox txtChargeLimPGain;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewTextBoxColumn colBalancer;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCell1;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCell2;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCell3;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCell4;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCell5;
		private System.Windows.Forms.DataGridViewCheckBoxColumn colCell6;
		private System.Windows.Forms.Button cmdDefault;
		private System.Windows.Forms.TextBox txtCurrentGain;
		private System.Windows.Forms.TextBox txtCurrentOffset;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label17;
	}
}