using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace USBCANTest
{
	public partial class CalibrationForm : Form
	{
		enum WizardState
		{
			START = 0,
			GET_FULL_V,
			GET_VLOW,
			WRITE_RES
		};

		WizardState wizardState = WizardState.START;

		public BalancerComm bc { get; set; }

		public CalibrationForm()
		{
			InitializeComponent();
			wizardState = WizardState.START;

			//bc.ctl.open();
		}


		~CalibrationForm()
		{
			//bc.ctl.close();
		}


		enum SlaveCodes {
			CMD_GET_BAL_INFO = 0x1,
			CMD_READ_EEPROM,
			CMD_WRITE_EEPROM,
			CMD_READ_CFG,
			CMD_WRITE_CFG,
			CMD_SAVE_CFG,
			CMD_SET_ADDR
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct SlaveConfig
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public ushort[] gain;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public short[] offset;
		};

		[StructLayout(LayoutKind.Sequential)]
		public struct BalPktSingle
		{
			public byte cmdCode;
			public byte hopCount;
			public byte balCount;
			public byte loadEnable;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public ushort [] voltages;
		};

		


		private byte getData<T>(T str, byte address)
		{
			byte[] buffer;

			buffer = StructTools.struct2byteArray<T>(str);
			return buffer[address];
		}

		int vLowIndex = 0;
		ushort[] fullVoltages = new ushort[6];
		ushort[] halfVoltages = new ushort[6];

		private void cmdCalibrate_Click(object sender, EventArgs e)
		{
			if (chkMaster.Checked)
			{
				masterCalStep();
			}
			else
			{
				slaveCalStep();
			}
		}

		private void consolePrint(string s)
		{
			txtConsole.Text += s;
		}

		private void cmdReadCfg_Click(object sender, EventArgs e)
		{
			byte address;
			int len;
			byte[] data;
			byte[] dOut;

			if (chkMaster.Checked)
			{
				BalancerComm.SlaveConfig sc = new BalancerComm.SlaveConfig();
				sc.gain = new ushort[6];
				sc.offset = new short[6];
			
				sc = bc.getCalibration(0xA);

				consolePrint("Calibration data:\r\nGains: ");

				for (int i = 0; i < 6; i++)
				{
					consolePrint(" " + sc.gain[i].ToString());
				}

				consolePrint("\r\nOffsets: ");

				for (int i = 0; i < 6; i++)
				{
					consolePrint(" " + sc.offset[i].ToString());
				}

				consolePrint("\r\n");
			}
			else
			{
				for (address = 0; address < Marshal.SizeOf(typeof(SlaveConfig)); address++)
				{
					//Set address in config struct to modify
					data = new byte[] { (byte)SlaveCodes.CMD_SET_ADDR, address };
					bc.balBusTx(0xA, data);

					//Send data for above address
					data = new byte[] { (byte)SlaveCodes.CMD_READ_CFG };
					bc.balBusTx(0xA, data);

					bc.balBusRx(out dOut, out len, 1, 100);
					consolePrint(dOut[0].ToString() + " ");
				}
				consolePrint("\r\n");
			}
		}

		private void cmdReadVoltages_Click(object sender, EventArgs e)
		{
			byte [] data;
			byte [] dOut;
			int len;
			BalPktSingle balPkt;

			if (chkMaster.Checked)
			{
				ushort [] voltages = bc.getLocalVoltages(0xA);
				consolePrint("Voltages:");
				for (int i = 0; i < 6; i++)
					consolePrint(" " + Convert.ToString(Convert.ToDouble(voltages[i]) / 1000));
				consolePrint("\r\n\n");
			}
			else
			{

				data = new byte[] { (byte)SlaveCodes.CMD_GET_BAL_INFO, 0x00 /*hop count*/, 0x01 /*balancer count*/, 0x00 /*load enable*/ };
				bc.balBusTx(0xA, data);

				bc.balBusRx(out dOut, out len, 4 + 12, 100);

				balPkt = StructTools.byteArray2Struct<BalPktSingle>(dOut);

				consolePrint("Voltages:");
				for (int i = 0; i < 6; i++)
					consolePrint(" " + Convert.ToString(Convert.ToDouble(balPkt.voltages[i]) / 1000));
				consolePrint("\r\n\n");
			}
		}

		private void slaveCalStep()
		{
			byte[] data = new byte[] {0x01, 0x00, 0x01, 0x3F};
			byte[] dOut;
			int len;
			byte address;

			SlaveConfig slaveConfig;
			BalPktSingle balPkt;

			switch(wizardState)
			{
				case WizardState.START:
					bc.setPassthrough(0xA, true);
					System.Threading.Thread.Sleep(100);
					bc.ctl.flush();
					wizardState = WizardState.GET_FULL_V;
					consolePrint("Set the source to all full voltage \r\n");
					cmdCalibrate.Text = "Next";
				break;

				case WizardState.GET_FULL_V:

					slaveConfig.gain = new ushort[6];
					slaveConfig.offset = new short[6];
					//Set the calibration structure to default
					for(int i = 0; i < 6; i++)
					{
						slaveConfig.gain[i] = 5105;
						slaveConfig.offset[i] = 0;
					}
					slaveConfig.gain[1] *= 2;

					//
					for (address = 0; address < Marshal.SizeOf(slaveConfig); address++ )
					{
						//Set address in config struct to modify
						data = new byte[] { (byte)SlaveCodes.CMD_SET_ADDR, address };
						bc.balBusTx(0xA, data);

						//Send data for above address
						data = new byte[] { (byte)SlaveCodes.CMD_WRITE_CFG, getData<SlaveConfig>(slaveConfig, address) };
						bc.balBusTx(0xA, data);
					}

					//Do a measurement with the new parameters
					data = new byte[] { (byte)SlaveCodes.CMD_GET_BAL_INFO, 0x00 /*hop count*/, 0x01 /*balancer count*/, 0x00 /*load enable*/ };
					bc.balBusTx(0xA, data);

					bc.balBusRx(out dOut, out len, 4 + 12, 100);

					balPkt = StructTools.byteArray2Struct<BalPktSingle>(dOut);

					fullVoltages = balPkt.voltages;

					for(int i = 0; i < len; i++)
					{
						consolePrint(dOut[i].ToString() + " ");
					}
					consolePrint("\r\n");

					consolePrint("Voltages:");
					for (int i = 0; i < 6; i++)
						consolePrint(" " + Convert.ToString(Convert.ToDouble(balPkt.voltages[i]) / 1000));
					consolePrint("\r\n\n");
					consolePrint("Set voltage 0 to half level\r\n");
					//bc.setPassthrough(0xA, false);
					wizardState = WizardState.GET_VLOW;
					vLowIndex = 0;
					cmdCalibrate.Text = "Next";
				break;

				case WizardState.GET_VLOW:
					data = new byte[] { (byte)SlaveCodes.CMD_GET_BAL_INFO, 0x00 /*hop count*/, 0x01 /*balancer count*/, 0x00 /*load enable*/ };
					bc.balBusTx(0xA, data);

					bc.balBusRx(out dOut, out len, 4 + 12, 100);

					balPkt = StructTools.byteArray2Struct<BalPktSingle>(dOut);

					halfVoltages[vLowIndex] = balPkt.voltages[vLowIndex];

					consolePrint("Read half voltage of " + Convert.ToString((double)halfVoltages[vLowIndex] / 1000) + "\r\n");
					vLowIndex++;

					if (vLowIndex >= 6)
					{
						wizardState = WizardState.WRITE_RES;
					}

					if (vLowIndex < 6)
						consolePrint("Set voltage " + vLowIndex.ToString() + " to half level\r\n");
					else
						consolePrint("Ready to write results\r\n");
					break;

				case WizardState.WRITE_RES:

					wizardState = WizardState.START;
					double [] actualHigh = new double[6];
					double [] actualLow = new double[6];

					actualHigh[0] = Convert.ToDouble(txtV0.Text);
					actualHigh[1] = Convert.ToDouble(txtV1.Text);
					actualHigh[2] = Convert.ToDouble(txtV2.Text);
					actualHigh[3] = Convert.ToDouble(txtV3.Text);
					actualHigh[4] = Convert.ToDouble(txtV4.Text);
					actualHigh[5] = Convert.ToDouble(txtV5.Text);

					actualLow[0] = Convert.ToDouble(txtVLow.Text);
					actualLow[1] = actualLow[0];
					actualLow[2] = actualLow[0];
					actualLow[3] = actualLow[0];
					actualLow[4] = actualLow[0];
					actualLow[5] = actualLow[0];

					SlaveConfig newConfig;
					newConfig.gain = new ushort[6];
					newConfig.offset = new short[6];

					for (int i = 0; i < 6; i++)
					{
						if (i != 1)
						{
							double adcHigh = (double)fullVoltages[i] / 5105 * 65536;
							double adcLow = (double)halfVoltages[i] / 5105 * 65536;

							newConfig.gain[i] = (ushort)((actualHigh[i] - actualLow[i]) / (double)(adcHigh - adcLow) * 65536 * 1000);
							newConfig.offset[i] = (short)(adcLow * (double)newConfig.gain[i] / 65536 - actualLow[i] * 1000.0);
						}
						else
						{
							double adcHigh = ((double)fullVoltages[i] + (double)fullVoltages[0]) / (5105 * 2) * 65536;
							double adcLow = ((double)halfVoltages[i] + (double)fullVoltages[0]) / (5105 * 2) * 65536;

							newConfig.gain[i] = (ushort)((actualHigh[i] - actualLow[i]) / (adcHigh - adcLow) * 65536 * 1000);
							newConfig.offset[i] = (short)(adcLow * (double)newConfig.gain[i] / 65536 - actualHigh[0] * 1000 - actualLow[i] * 1000);
						}
					}

					//Write the new cal values to the balancer module
					for (address = 0; address < Marshal.SizeOf(typeof(SlaveConfig)); address++)
					{
						//Set address in config struct to modify
						data = new byte[] { (byte)SlaveCodes.CMD_SET_ADDR, address };
						bc.balBusTx(0xA, data);

						//Send data for above address
						data = new byte[] { (byte)SlaveCodes.CMD_WRITE_CFG, getData<SlaveConfig>(newConfig, address) };
						bc.balBusTx(0xA, data);
					}

					data = new byte[] { (byte)SlaveCodes.CMD_SAVE_CFG };
					bc.balBusTx(0xA, data);
					bc.balBusRx(out dOut, out len, 100, 5000);

					if ((byte)SlaveCodes.CMD_SAVE_CFG == dOut[0] && len == 1)
					{
						consolePrint("Succsessfully stored results\r\n");
					}
					else
					{
						consolePrint("Failed to store results! Received\r\n");
						for (int i = 0; i < len; i++)
							consolePrint(" " + dOut[i].ToString());
						consolePrint("\r\n");
					}

					break;

			}
		}

		private void masterCalStep()
		{
			BalancerComm.SlaveConfig slaveConfig;

			switch (wizardState)
			{
				case WizardState.START:
					bc.ctl.flush();

					slaveConfig.gain = new ushort[6];
					slaveConfig.offset = new short[6];

					//Set the calibration structure to default
					for (int i = 0; i < 6; i++)
					{
						slaveConfig.gain[i] = 5105;
						slaveConfig.offset[i] = 0;
					}
					slaveConfig.gain[1] *= 2;

					bc.setCalibration(0xA, slaveConfig);

					wizardState = WizardState.GET_FULL_V;
					consolePrint("Set the source to all full voltage \r\n");
					cmdCalibrate.Text = "Next";
					break;

				case WizardState.GET_FULL_V:
					//Do a measurement of the voltages
					fullVoltages = bc.getLocalVoltages(0xA);

					consolePrint("Voltages:");
					for (int i = 0; i < 6; i++)
						consolePrint(" " + Convert.ToString(Convert.ToDouble(fullVoltages[i]) / 1000));
					consolePrint("\r\n\n");
					
					wizardState = WizardState.GET_VLOW;
					vLowIndex = 0;
					consolePrint("Set voltage 0 to half level\r\n");
					cmdCalibrate.Text = "Next";
					break;

				case WizardState.GET_VLOW:
					halfVoltages[vLowIndex] = bc.getLocalVoltages(0xA)[vLowIndex];

					consolePrint("Read half voltage of " + Convert.ToString((double)halfVoltages[vLowIndex] / 1000) + "\r\n");
					vLowIndex++;

					if (vLowIndex >= 6)
					{
						wizardState = WizardState.WRITE_RES;
					}

					if (vLowIndex < 6)
						consolePrint("Set voltage " + vLowIndex.ToString() + " to half level\r\n");
					else
						consolePrint("Ready to write results\r\n");
					break;

				case WizardState.WRITE_RES:
					double[] actualHigh = new double[6];
					double[] actualLow = new double[6];

					actualHigh[0] = Convert.ToDouble(txtV0.Text);
					actualHigh[1] = Convert.ToDouble(txtV1.Text);
					actualHigh[2] = Convert.ToDouble(txtV2.Text);
					actualHigh[3] = Convert.ToDouble(txtV3.Text);
					actualHigh[4] = Convert.ToDouble(txtV4.Text);
					actualHigh[5] = Convert.ToDouble(txtV5.Text);

					actualLow[0] = Convert.ToDouble(txtVLow.Text);
					actualLow[1] = actualLow[0];
					actualLow[2] = actualLow[0];
					actualLow[3] = actualLow[0];
					actualLow[4] = actualLow[0];
					actualLow[5] = actualLow[0];

					BalancerComm.SlaveConfig newConfig;
					newConfig.gain = new ushort[6];
					newConfig.offset = new short[6];

					for (int i = 0; i < 6; i++)
					{
						if (i != 1)
						{
							double adcHigh = (double)fullVoltages[i] / 5105 * 65536;
							double adcLow = (double)halfVoltages[i] / 5105 * 65536;

							newConfig.gain[i] = (ushort)((actualHigh[i] - actualLow[i]) / (double)(adcHigh - adcLow) * 65536 * 1000);
							newConfig.offset[i] = (short)(adcLow * (double)newConfig.gain[i] / 65536 - actualLow[i] * 1000.0);
						}
						else
						{
							double adcHigh = ((double)fullVoltages[i] + (double)fullVoltages[0]) / (5105 * 2) * 65536;
							double adcLow = ((double)halfVoltages[i] + (double)fullVoltages[0]) / (5105 * 2) * 65536;

							newConfig.gain[i] = (ushort)((actualHigh[i] - actualLow[i]) / (adcHigh - adcLow) * 65536 * 1000);
							newConfig.offset[i] = (short)(adcLow * (double)newConfig.gain[i] / 65536 - actualHigh[0] * 1000 - actualLow[i] * 1000);
						}
					}

					//Write the new cal values to the balancer module
					if (bc.setCalibration(0xA, newConfig))
					{
						consolePrint("Succsessfully stored results\r\n");
						if (bc.saveCalibration(0xA))
						{
							consolePrint("Succsessfully saved results to EEPROM\r\n");
						}
						else
						{
							consolePrint("Failed to save results to EEPROM!\r\n");
						}
					}
					else
					{
						consolePrint("Failed to store results!\r\n");
					}

					wizardState = WizardState.START;

					break;
			}
		}

		private void cmdShowCfg_Click(object sender, EventArgs e)
		{
			ConfigForm frmCfg = new ConfigForm();
			frmCfg.bc = bc;
			frmCfg.Show();
		}
	}
}
