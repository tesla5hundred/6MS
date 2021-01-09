using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace USBCANTest
{
    public partial class Form1 : Form
    {
		private BalancerComm bc = new BalancerComm();

		bool CANConnected;
        public Form1()
        {
            InitializeComponent();

			CANConnected = false;
        }

		~Form1()
		{
			bc.ctl.close();
		}

		private void cmdConnect_Click(object sender, EventArgs e)
		{
			uint retcode;

			if (CANConnected)
			{
				retcode = CAN.VCI_CloseDevice(CAN.DEV_USBCAN2, 0);
				if (retcode != CAN.STATUS_OK)
				{
					MessageBox.Show("Couldn't close CAN device, error " + retcode.ToString());
					return;
				}

				CANConnected = false;
				cmdConnect.Text = "Connect";

			}
			else
			{
				retcode = CAN.VCI_OpenDevice(CAN.DEV_USBCAN2, 0, 0);
				if (retcode != CAN.STATUS_OK)
				{
					MessageBox.Show("Couldn't open CAN device, error " + retcode.ToString());
					return;
				}

				CAN.VCI_INIT_CONFIG canCFG;

				canCFG.AccMask = 0xFFFFFFFF;
				canCFG.AccCode = 0x0F000000;
				canCFG.InitFlag = 0;
				canCFG.Filter = 1;	//Receive all frames
				canCFG.Timing0 = 0;
				canCFG.Timing1 = 0x14;
				canCFG.Mode = 0;	//Normal mode

				retcode = CAN.VCI_InitCAN(CAN.DEV_USBCAN2, 0, 0, ref canCFG);
				if (retcode != CAN.STATUS_OK)
				{
					MessageBox.Show("Couldn't Init CAN, error " + retcode.ToString());
					return;
				}

				retcode = CAN.VCI_StartCAN(CAN.DEV_USBCAN2, 0, 0);
				if (retcode != CAN.STATUS_OK)
				{
					MessageBox.Show("Couldn't start CAN, error " + retcode.ToString());
					return;
				}

				bc.ctl.open();

				CANConnected = true;



				cmdConnect.Text = "Disconnect";

			}
		}

		private void cmdTx_Click(object sender, EventArgs e)
		{
			UInt32 retval;
			if (CANConnected)
			{
				CAN.VCI_CAN_OBJ[] send = new CAN.VCI_CAN_OBJ[1];
				CAN.VCI_CAN_OBJ[] receive = new CAN.VCI_CAN_OBJ[3];

				send[0].ID = 0xA;
				send[0].TimeFlag = 0;
				send[0].SendType = 0;
				send[0].RemoteFlag = 0;
				send[0].ExternFlag = 1;
				send[0].DataLen = 1;
				send[0].Data = new byte[8];

				send[0].Data[0] = 0;

				retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);

				receive[0].Data = new byte[8];
				receive[1].Data = new byte[8];
				receive[2].Data = new byte[8];

				receive[0].Reserved = new byte[3];
				receive[1].Reserved = new byte[3];
				receive[2].Reserved = new byte[3];

				retval = CAN.VCI_Receive(CAN.DEV_USBCAN2, 0, 0, receive, 50, 1000);
				//retval = CAN.VCI_Receive(CAN.DEV_USBCAN2, 0, 0, receive, 1, 1000);
				//retval = CAN.VCI_Receive(CAN.DEV_USBCAN2, 0, 0, receive, 1, 1000);

				int a;
				a = 0;
			}
		}

		class ViewerGridData
		{
			public float Voltage { get; set; }
			public bool LoadEn { get; set; }
		}

		private void button1_Click(object sender, EventArgs e)
		{
			/*TestObject test1 = new TestObject()
			{
				OneValue = 2,
				TwoValue = 3
			};
			TestObject test2 = new TestObject()
			{
				OneValue = 4,
				TwoValue = 5
			};
			List<TestObject> list = new List<TestObject>();
			list.Add(test1);
			list.Add(test2);
			dataGridView1.DataSource = list;*/
		}
		private void cmdSyncPackData_Click(object sender, EventArgs e)
		{
			bc.syncPackData(0xA);

			for (int i = 0; i < bc.config.cellCount; i++)
			{
				dataGridView1.Rows[i].Cells["colCell"].Value = (i + 1).ToString();
				dataGridView1.Rows[i].Cells["colVoltage"].Value = bc.packData.voltages[i].ToString("F3");
				dataGridView1.Rows[i].Cells["colLoadEn"].Value = bc.packData.loadEn[i];
				dataGridView1.Rows[i].Cells["colTemp"].Value = "-";
			}

			lblMinV.Text = ((float)bc.balData.minV / 1000.0f).ToString("F2");
			lblMaxV.Text = ((float)bc.balData.maxV / 1000.0f).ToString("F2");
			lblAvgV.Text = ((float)bc.balData.averageV / 1000.0f).ToString("F2");
			lblTotalV.Text = ((float)bc.balData.totalV / 1000.0f).ToString("F2");
			lblCurrent.Text = ((float)bc.balData.current / 1000.0f).ToString("F3");
			lblErrorCode.Text = bc.balData.faultCode.ToString("X4");
			lblTemp.Text = ((float)bc.balData.temp / 10.0f).ToString("F1");
			lblAmpHours.Text = ((float)bc.balData.couloumbCount / 3600000.0f).ToString("F4");

            if (loggingOpen)
            {
                String cellString = "";

                for (int i = 0; i < bc.config.cellCount; i++)
                    cellString += bc.packData.voltages[i].ToString("F3") + ",";

                cellString.Remove(cellString.Length - 2, 1);    //Remove the last comma

                writer.WriteLine(((float)bc.balData.minV / 1000.0f).ToString("F2") + "," +
                                    ((float)bc.balData.maxV / 1000.0f).ToString("F2") + "," +
                                    ((float)bc.balData.averageV / 1000.0f).ToString("F2") + "," +
                                    ((float)bc.balData.totalV / 1000.0f).ToString("F2") + "," +
                                    ((float)bc.balData.current / 1000.0f).ToString("F3") + "," +
                                    ((float)bc.balData.couloumbCount / 3600000.0f).ToString("F4") + "," + 
                                    ((float)bc.balData.temp / 10.0f).ToString("F1") + "," + cellString);
            }

			return;
			/*
			ViewerGridData [] vg = new ViewerGridData[300];

			for (int i = 0; i < bc.packData.voltages.Length; i++)
			{
				vg[i] = new ViewerGridData();
				vg[i].Voltage = bc.packData.voltages[i];
				vg[i].LoadEn = bc.packData.loadEn[i];
				list.Add(vg[i]);
			}
			dataGridView1.DataSource = list;


			*/
		}

		private void cmdShowCal_Click(object sender, EventArgs e)
		{
			CalibrationForm cf = new CalibrationForm();
			cf.bc = bc;
			cf.Show();
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			bc.ctl.close();
		}

        private bool loggingOpen = false;
        StreamWriter writer;

		private void cmdLive_Click(object sender, EventArgs e)
		{

  
			if (tmrLive.Enabled)
			{
				tmrLive.Enabled = false;
				cmdLive.Text = "Start Live Data";
                if (loggingOpen)
                {
                    writer.Close();
                    loggingOpen = false;
                }
                
			}
			else
			{

                if (chkLogging.Checked)
                {
                    if (File.Exists(txtLoggingPath.Text))
                    {
                        DialogResult dr = MessageBox.Show("File already exists, overwrite?", "Overwrite?", MessageBoxButtons.YesNo);
                        if (DialogResult.No == dr)
                            return;
                    }
                
                    try
                    {
                        writer = new StreamWriter(txtLoggingPath.Text);
                    }
                    catch (IOException exception)
                    {
                        MessageBox.Show(exception.Message, "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    String cellString = "";

                    for (int i = 0; i < bc.config.cellCount; i++)
                        cellString += "Cell " + (i + 1).ToString() + ",";
                    cellString.Remove(cellString.Length - 2, 1);    //Remove the last comma

                    writer.WriteLine("Min V,Max V,Avg V,Total V,Current,Ah,Temp (C)," + cellString);

                    loggingOpen = true;
                }
				tmrLive.Enabled = true;
				cmdLive.Text = "Stop Live Data";
			}
		}

		private void tmrLive_Tick(object sender, EventArgs e)
		{
			cmdSyncPackData_Click(sender, e);
		}

		private void loadCellGrid(int rows)
		{
			dataGridView1.Rows.Clear();

			for (int i = 0; i < rows; i++)
			{
				dataGridView1.Rows.Add();
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["colCell"].Value = (i + 1).ToString();
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["colVoltage"].Value = "-";
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["colLoadEn"].Value = false;
				dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["colTemp"].Value = "-";
			}

		}

		private void cmdConn_Click(object sender, EventArgs e)
		{
			bc.connect(0xA);

			loadCellGrid(bc.config.cellCount);
		}

        private void cmdSetLoggingPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();

            sf.Filter = "CSV File|*.csv";
            sf.Title = "Select logging CSV file";
            sf.ShowDialog();

            txtLoggingPath.Text = sf.FileName;
        }

    }
}
