using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace USBCANTest
{
	public partial class ConfigForm : Form
	{
		private bool loaded = false;
		public BalancerComm bc { get; set; }

		BalancerComm.Config_t config;

		public ConfigForm()
		{
			InitializeComponent();
			loadCellGrid();
			loaded = true;
		}

		private void cmdRead_Click(object sender, EventArgs e)
		{
			config = bc.getConfig(0xA);
			updateUI(config);
			
		}

		private void cmdWrite_Click(object sender, EventArgs e)
		{
			updateConfig(ref config);
			if(!bc.setConfig(0xA, config))
			{
				MessageBox.Show("Failed to set configuraiton!");
				return;
			}
			if (!bc.saveConfig(0xA))
			{
				MessageBox.Show("Failed to write configuraiton to EEPROM!");
				return;
			}
		}

		void updateUI(BalancerComm.Config_t config)
		{
			txtCellCount.Text = Convert.ToString(config.cellCount);
			updnBoardCount.Value = config.slaveCount + 1;
			txtBoardCANID.Text = config.canID.ToString("X8");
			txtCANDstID.Text = config.canDstID.ToString("X8");
			txtBalPollPeriod.Text = config.balPollPeriod.ToString();
			txtBalReplyTimeout.Text = config.balReplyTimeout.ToString();
			txtCellVoltageBalance.Text = ((double)config.cellVoltageBalance / 1000).ToString();
			txtBalanceOffset.Text = ((double)config.balanceOffset / 1000).ToString();
			txtCellVoltageMax.Text = ((double)config.cellVoltageMax / 1000).ToString();
			txtCellVoltageMin.Text = ((double)config.cellVoltageMin / 1000).ToString();

			radioChgEnActHigh.Checked = config.chgEnActiveHigh > 0 ? true : false;
			radioChgEnActLow.Checked = !radioChgEnActLow.Checked;

			chkAFS1Enable.Checked = (config.analog1FunctionSelect & (byte)BalancerComm.AFS.AFS_ENABLE) > 0;
			radioAFS1Digital.Checked = (config.analog1FunctionSelect & (byte)BalancerComm.AFS.AFS_DIGITAL) > 0;
			radioAFS1Analog.Checked = !radioAFS1Digital.Checked;
			radioAFS1PolarityHigher.Checked = (config.analog1FunctionSelect & (byte)BalancerComm.AFS.AFS_POLARITY) > 0;
			radioAFS1PolarityLower.Checked = !radioAFS1PolarityHigher.Checked;
			radioAFS1ChargeLim.Checked = (config.analog1FunctionSelect & (byte)BalancerComm.AFS.AFS_FUNCTION) > 0;
			radioAFS1DischargeLim.Checked = !radioAFS1ChargeLim.Checked;

			chkAFS2Enable.Checked = (config.analog2FunctionSelect & (byte)BalancerComm.AFS.AFS_ENABLE) > 0;
			radioAFS2Digital.Checked = (config.analog2FunctionSelect & (byte)BalancerComm.AFS.AFS_DIGITAL) > 0;
			radioAFS2Analog.Checked = !radioAFS2Digital.Checked;
			radioAFS2PolarityHigher.Checked = (config.analog2FunctionSelect & (byte)BalancerComm.AFS.AFS_POLARITY) > 0;
			radioAFS2PolarityLower.Checked = !radioAFS2PolarityHigher.Checked;
			radioAFS2ChargeLim.Checked = (config.analog2FunctionSelect & (byte)BalancerComm.AFS.AFS_FUNCTION) > 0;
			radioAFS2DischargeLim.Checked = !radioAFS2ChargeLim.Checked;

			txtMaxVFilterFreq.Text = config.maxVFilterMult.ToString();
			txtMinVFilterFreq.Text = config.minVFilterMult.ToString();

			txtChargeLimPGain.Text = config.chargeLimitGain.ToString();
			txtDischargeLimPGain.Text = config.dischargeLimitGain.ToString();

			setCellGridCount(config.slaveCount + 1);
			setCellGrid(config.cellEnable);

			txtCurrentGain.Text = config.currentGain.ToString();
			txtCurrentOffset.Text = config.currentOffset.ToString();
		}

		void updateConfig(ref BalancerComm.Config_t config)
		{
			config.slaveCount = (byte)(Convert.ToByte(updnBoardCount.Value) - 1);
			config.cellEnable = getCellGridData();
			config.cellCount = (byte)getCellCount(config);

			config.canID = Convert.ToUInt32(txtBoardCANID.Text, 16);
			config.canDstID = Convert.ToUInt32(txtCANDstID.Text, 16);
			config.balPollPeriod = Convert.ToUInt16(txtBalPollPeriod.Text);
			config.balReplyTimeout = Convert.ToUInt16(txtBalReplyTimeout.Text);

			config.cellVoltageBalance = (UInt16)(Convert.ToDouble(txtCellVoltageBalance.Text) * 1000);
			config.balanceOffset = (UInt16)(Convert.ToDouble(txtBalanceOffset.Text) * 1000);
			config.cellVoltageMax = (UInt16)(Convert.ToDouble(txtCellVoltageMax.Text) * 1000);
			config.cellVoltageMin = (UInt16)(Convert.ToDouble(txtCellVoltageMin.Text) * 1000);


			config.chgEnActiveHigh = (byte)(radioChgEnActHigh.Checked ? 1 : 0);

			config.analog1FunctionSelect = (byte)((chkAFS1Enable.Checked ? (byte)BalancerComm.AFS.AFS_ENABLE : 0) |
				(radioAFS1Digital.Checked ? (byte)BalancerComm.AFS.AFS_DIGITAL : 0) |
				(radioAFS1PolarityHigher.Checked ? (byte)BalancerComm.AFS.AFS_POLARITY : 0) |
				(radioAFS1ChargeLim.Checked ? (byte)BalancerComm.AFS.AFS_FUNCTION : 0));

			config.analog2FunctionSelect = (byte)((chkAFS2Enable.Checked ? (byte)BalancerComm.AFS.AFS_ENABLE : 0) |
				(radioAFS2Digital.Checked ? (byte)BalancerComm.AFS.AFS_DIGITAL : 0) |
				(radioAFS2PolarityHigher.Checked ? (byte)BalancerComm.AFS.AFS_POLARITY : 0) |
				(radioAFS2ChargeLim.Checked ? (byte)BalancerComm.AFS.AFS_FUNCTION : 0));

			config.maxVFilterMult = Convert.ToUInt16(txtMaxVFilterFreq.Text);
			config.minVFilterMult = Convert.ToUInt16(txtMinVFilterFreq.Text);

			config.chargeLimitGain = Convert.ToUInt16(txtChargeLimPGain.Text);
			config.dischargeLimitGain = Convert.ToUInt16(txtDischargeLimPGain.Text);

			config.currentGain = Convert.ToUInt32(txtCurrentGain.Text);
			config.currentOffset = Convert.ToInt16(txtCurrentOffset.Text);


			
		}

		private void ConfigForm_Load(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			dataGridView1.Rows.Add();
			dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell1"].Value = 1;
			dataGridView1.Rows[dataGridView1.Rows.Count - 1].Visible = false;
		}

		//Load the cell grid with data for all possible rows
		private void loadCellGrid()
		{
			dataGridView1.Rows.Clear();

			for(int i = 0; i < BalancerComm.MAX_SLAVE_BOARDS + 1; i++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colBalancer"].Value = (i == 0 ? "(M) " : "(S) ") + (i + 1).ToString();
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell1"].Value = true;
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell2"].Value = 1;
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell3"].Value = 1;
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell4"].Value = 1;
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell5"].Value = 1;
				dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["colCell6"].Value = 1;
			}

		}

		//Enable the first rows cells
		private void setCellGridCount(int rows)
		{

			for (int i = 0; i < BalancerComm.MAX_SLAVE_BOARDS + 1; i++)
			{
				dataGridView1.Rows[i].Visible = i < rows;
			}

		}

		//Set the enable values in the cell grid
		private void setCellGrid(byte [] enables)
		{

			for (int i = 0; i < BalancerComm.MAX_SLAVE_BOARDS + 1; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["colCell" + (j+1).ToString()].Value = (enables[i] & (1 << j)) != 0;
				}
			}

		}

		byte[] getCellGridData()
		{
			byte[] data = new byte[BalancerComm.MAX_SLAVE_BOARDS + 1];
			

			for (int i = 0; i < BalancerComm.MAX_SLAVE_BOARDS + 1; i++)
			{
				data[i] = 0;
				for (int j = 0; j < 6; j++)
				{
					bool b = Convert.ToBoolean(dataGridView1.Rows[i].Cells["colCell" + (j + 1).ToString()].EditedFormattedValue);

					data[i] |= (byte)((b ? 1 : 0) << j);
				}
			}
			return data;
		}

		//Returns the number of cells enabled within the enabled boards
		int getCellCount(BalancerComm.Config_t config)
		{
			int count = 0;

			for (int i = 0; i < config.slaveCount + 1; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					if ((config.cellEnable[i] & (1 << j)) != 0)
						count++;
				}
			}
			return count;
		}

		private void updnBoardCount_ValueChanged(object sender, EventArgs e)
		{
			setCellGridCount((int)updnBoardCount.Value);
			config.slaveCount = (byte)(Convert.ToByte(updnBoardCount.Value) - 1);
			config.cellEnable = getCellGridData();
			config.cellCount = (byte)getCellCount(config);
			txtCellCount.Text = Convert.ToString(config.cellCount);
		}

		private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			int count = 0;

			//if (!loaded)
				return;

			for (int i = 0; i < updnBoardCount.Value; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					bool b = Convert.ToBoolean(dataGridView1.Rows[i].Cells["colCell" + (j + 1).ToString()].Value);
					if (b)
						count++;
				}
			}

			txtCellCount.Text = count.ToString();
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			int count = 0;

			if (!loaded)
				return;

			for (int i = 0; i < updnBoardCount.Value; i++)
			{
				for (int j = 0; j < 6; j++)
				{
					bool b = Convert.ToBoolean(dataGridView1.Rows[i].Cells["colCell" + (j + 1).ToString()].EditedFormattedValue);
					if (b)
						count++;
				}
			}

			txtCellCount.Text = count.ToString();
		}

		private void cmdDefault_Click(object sender, EventArgs e)
		{
			DialogResult dr;

			dr = MessageBox.Show("Reset all settings to default?", "Reset to Default", MessageBoxButtons.OKCancel);

			if (dr == DialogResult.OK)
			{

				config.cellCount = 12;
				config.cellEnable = new byte[BalancerComm.MAX_SLAVE_BOARDS + 1];
				for (int i = 0; i < BalancerComm.MAX_SLAVE_BOARDS + 1; i++)
					config.cellEnable[i] = (byte)(i < 2 ? 0x3F : 0);
				config.slaveCount = 1;
				config.canID = 0xA;
				config.canDstID = 0x0F000000;
				config.balPollPeriod = 1000;
				config.balReplyTimeout = 100;
				config.cellVoltageBalance = 3900;
				config.balanceOffset = 5;
				config.cellVoltageMax = 4200;
				config.cellVoltageMin = 2700;
				config.balBusPassthrough = 0;
				config.chgEnActiveHigh = 0;
				config.analog1FunctionSelect = 0;
				config.analog2FunctionSelect = 0;
				config.maxVFilterMult = 6000;
				config.minVFilterMult = 6000;
				config.dischargeLimitGain = 300;
				config.chargeLimitGain = 300;
				config.currentGain = 1;
				config.currentOffset = 0;

				updateUI(config);
			}
		}


	}
}
