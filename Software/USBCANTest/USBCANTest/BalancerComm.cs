using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace USBCANTest
{
	public class BalancerComm
	{

		public CANCtl ctl;
		CAN.VCI_CAN_OBJ[] receive;

		public BalancerComm()
		{
			ctl = new CANCtl();
			receive = new CAN.VCI_CAN_OBJ[500];
			
			for (int i = 0; i < 500; i++)
			{
				receive[i].Data = new byte[8];
				receive[i].Reserved = new byte[3];
			}

		}

		~BalancerComm()
		{
		}


		public struct PackData {
			public float [] voltages;
			public bool[] loadEn;
			public float[] temperatures;
		};
		public PackData packData;
		public BalData_t balData;
		public Config_t config;
		
		[StructLayout(LayoutKind.Sequential)]
		public struct VoltagesStr
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public ushort [] voltages;
		};

		enum CANCMD : byte
		{
			CAN_CMD_SEND_VOLTAGES = 0,
			CAN_CMD_SEND_LOAD_EN,
			CAN_CMD_SEND_BAL_DATA,
			CAN_CMD_BALBUS_TX,
			CAN_CMD_BALBUS_RX,
			CAN_CMD_SET_BALBUS_PASSTHROUGH,
			CAN_CMD_CLEAR_BALBUS_PASSTHROUGH,
			CAN_CMD_TEST,
			CAN_CMD_READ_CAL,
			CAN_CMD_WRITE_CAL,
			CAN_CMD_GET_LOCAL_VOLTAGES,
			CAN_CMD_SAVE_CAL,
			CAN_CMD_GET_CONFIG,
			CAN_CMD_SET_CONFIG,
			CAN_CMD_SAVE_CONFIG,
			CAN_CMD_GET_IDENTITY,
			CAN_CMD_SET_IDENTITY,
			CAN_CMD_SAVE_IDENTITY,
			CAN_CMD_GET_BAL_DATA
		};

		public enum AFS : byte {
			AFS_ENABLE = 1,
			AFS_POLARITY = 2,
			AFS_FUNCTION = 4,
			AFS_DIGITAL = 8
		};

		public const float VOLTS_PER_COUNT = 0.001f;	//volts per LSB of voltage value
		public const int CELLS_PER_SLAVE = 6;
		public const int MAX_SLAVE_BOARDS = 42;
		public const int MAX_CELLS = ((MAX_SLAVE_BOARDS + 1) * CELLS_PER_SLAVE);

		[StructLayout(LayoutKind.Sequential, Pack=2)]
		public struct Config_t {
			public byte cellCount;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = MAX_SLAVE_BOARDS+1)]
			public byte[] cellEnable;
			public byte slaveCount;
			public UInt32 canID;		//ID of this unit
			public UInt32 canDstID;	//ID of destination packets
			public UInt16 balPollPeriod;	//period at which to poll the balancer units (ms)
			public UInt16 balReplyTimeout;

			//Balancing parameters
			public UInt16 cellVoltageBalance;		//Minimum cell voltage for balancing to occur
			public UInt16 balanceOffset;			//Amount cell has to be above average cell voltage to enable load
			public UInt16 cellVoltageMax;			//Maximum cell voltage. Charge is cut off completely at this voltage
			public UInt16 cellVoltageMin;			//Minimum cell voltage. Discharge is cut off completely at this voltage
			public byte balBusPassthrough;
			public byte chgEnActiveHigh;					//TRUE indicates that charge enable is active high (not grounded)
			public byte analog1FunctionSelect;	//Selects function of analog 1 output
			public byte analog2FunctionSelect;	//Selects function of analog 2 output
													//Bits
													//3: Digital		0: Signal is analog  1: Signal is TTL digital output
													//2: Function		0: discharge limit  1: charge limit
													//1: Polarity		0: lower signal decreases draw  1: higher signal decreases draw
													//0: enable			0: disabled  1: enabled
			public UInt16 maxVFilterMult;			//Typical value ~6000
			public UInt16 minVFilterMult;
			public UInt16 chargeLimitGain; 		//typical value ~300
			public UInt16 dischargeLimitGain;
			public UInt32 currentGain;
			public Int16 currentOffset;
		};

		public struct PackData_t
		{
			public UInt16 [] voltages;
			public bool [] loadEnable;
		};

		[StructLayout(LayoutKind.Sequential, Pack=2)]
		public struct BalData_t {
			public byte voltagesUpdated;
			public UInt16 minV;
			public UInt16 maxV;
			public UInt16 averageV;
			public UInt32 totalV;
			public byte chargeEnableState;		//TRUE = charger enabled
			public UInt16 analog1Value;	// 0xFFFF = max analog value
			public UInt16 analog2Value;
			public Int32 current;				//Measured battery current
			public UInt16 faultCode;
			public Int16 temp;
			public Int64 couloumbCount;
			public Int64 lifetimeColoumbs;
			public UInt32 svICount;
			public UInt32 slICount;
			public UInt32 sdICount;
			public UInt32 svLCount;
			public UInt32 slLCount;
			public UInt32 sdLCount;
		};


		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct Identity_t {
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] model;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] serial;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
			public byte[] version;
		};

		[StructLayout(LayoutKind.Sequential, Pack = 2)]
		public struct Cal_t
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public ushort[] gain;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public short[] offset;
		};

		public bool connect(UInt32 CanID)
		{
			config = getConfig(CanID);

			return true;
		}

		public bool syncPackData(UInt32 CanID)
		{
			CAN.VCI_CAN_OBJ[] receive;
			float[] voltageArr = new float[MAX_CELLS];


			if (!getVoltages(CanID))
				return false;

			if (!getLoadEn(CanID))
				return false;

			if (!getBalData(CanID))
				return false;

			return true;

		}

		public bool getVoltages(UInt32 CanID)
		{
			CAN.VCI_CAN_OBJ [] rx1, rx2 = {}, rx;
			byte[] txBuf = { (byte)CANCMD.CAN_CMD_SEND_VOLTAGES };
			ctl.sendData(txBuf, 0xA, false, true);

			//Get the first can frame and determine the length
			ctl.getCanFrames(out rx1, 1);
			UInt16 cmd = (ushort)(rx1[0].Data[0] | rx1[0].Data[1] << 8);
			UInt16 voltageCount = (ushort)(rx1[0].Data[2] | rx1[0].Data[3] << 8);

			if (cmd != (byte)CANCMD.CAN_CMD_SEND_VOLTAGES)
				return false;

			//Receive any more frames required
			int rxLen = (voltageCount - 2 + 3) / 4;
			if (rxLen > 0)
			{
				ctl.getCanFrames(out rx2, rxLen);
			}

			//Copy both receive arrays into one
			rx = new CAN.VCI_CAN_OBJ[rx1.Length + rx2.Length];
			rx1.CopyTo(rx, 0);
			rx2.CopyTo(rx, rx1.Length);

			UInt16 [] voltages = new UInt16 [voltageCount];
	
			//Get ushort data out of receive array
			for (int i = 0; i < voltageCount; i++)
			{
				voltages[i] = (ushort)(rx[2 * (i + 2) / 8].Data[2 * (i + 2) % 8] | rx[2 * (i + 2) / 8].Data[2 * (i + 2) % 8 + 1] << 8);
			}

			packData.voltages = new float[voltageCount];
			//Convert to float
			for (int i = 0; i < voltageCount; i++)
			{
				packData.voltages[i] = (float)voltages[i] * VOLTS_PER_COUNT;
			}

            if (voltages.Length != config.cellCount)
				return false;

			return true;
		}

		public bool getLoadEn(UInt32 CanID)
		{
			CAN.VCI_CAN_OBJ[] rx1, rx2 = { }, rx;
			byte[] txBuf = { (byte)CANCMD.CAN_CMD_SEND_LOAD_EN };
			ctl.sendData(txBuf, 0xA, false, true);

			//Get the first can frame and determine the length
			ctl.getCanFrames(out rx1, 1);
			UInt16 cmd = (ushort)(rx1[0].Data[0] | rx1[0].Data[1] << 8);
			UInt16 cellCount = (ushort)(rx1[0].Data[2] | rx1[0].Data[3] << 8);

			if (cmd != (byte)CANCMD.CAN_CMD_SEND_LOAD_EN)
				return false;

			UInt16 bytes = (ushort)((cellCount + 7) / 8);	//Number of bytes requred to hold cellCount bits

			//Receive any more frames required
			int rxLen = (bytes - 4 + 7) / 8;
			if (rxLen > 0)
			{
				ctl.getCanFrames(out rx2, rxLen);
			}

			//Copy both receive arrays into one
			rx = new CAN.VCI_CAN_OBJ[rx1.Length + rx2.Length];
			rx1.CopyTo(rx, 0);
			rx2.CopyTo(rx, rx1.Length);

			packData.loadEn = new bool[cellCount];

			//Extract the load enable bool array from the packed binary data received from the BMS, taking into account 4 byte header
			for (int i = 0; i < cellCount; i++)
			{
				packData.loadEn[i] = (rx[(i / 8 + 4) / 8].Data[(i / 8 + 4) % 8] & (1 << (i % 8))) != 0;
			}

            if (packData.loadEn.Length != config.cellCount)
				return false;

			return true;
		}

		public bool getBalData(UInt32 CanID)
		{
			CAN.VCI_CAN_OBJ[] rx;
			byte[] txBuf = { (byte)CANCMD.CAN_CMD_SEND_BAL_DATA };
			ctl.sendData(txBuf, 0xA, false, true);
			int bytes = Marshal.SizeOf(typeof(BalData_t)) + 2;	//Command code plus returned data

			//Get the first can frame and determine the length
			ctl.getCanFrames(out rx, (bytes+7) / 8);
			UInt16 cmd = (ushort)(rx[0].Data[0] | rx[0].Data[1] << 8);

			if (cmd != (byte)CANCMD.CAN_CMD_SEND_BAL_DATA)
				return false;

			byte[] buffer = new byte[Marshal.SizeOf(typeof(BalData_t))];

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = rx[(i+2) / 8].Data[(i+2) % 8];
			}
			balData = StructTools.byteArray2Struct<BalData_t>(buffer);
			return true;
		}

		public bool setPassthrough(UInt32 CanID, bool passthrough)
		{
			UInt32 retval;
			CAN.VCI_CAN_OBJ[] send = new CAN.VCI_CAN_OBJ[1];

			send[0].ID = CanID;
			send[0].TimeFlag = 0;
			send[0].SendType = 0;
			send[0].RemoteFlag = 0;
			send[0].ExternFlag = 1;
			send[0].DataLen = 1;
			send[0].Data = new byte[8];

			send[0].Data[0] = (byte) (passthrough ? CANCMD.CAN_CMD_SET_BALBUS_PASSTHROUGH : CANCMD.CAN_CMD_CLEAR_BALBUS_PASSTHROUGH);

			retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);
			if (retval != CAN.STATUS_OK)
				return false;

			return true;

		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SlaveConfig
		{
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public ushort[] gain;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
			public short[] offset;
		};

		//Get the calibration structure from the master unit
		public SlaveConfig getCalibration(UInt32 CanID)
		{
			SlaveConfig sc = new SlaveConfig();
			UInt32 retval;
			CAN.VCI_CAN_OBJ[] send = new CAN.VCI_CAN_OBJ[1];
			CAN.VCI_CAN_OBJ[] receive;

			send[0].ID = CanID;
			send[0].TimeFlag = 0;
			send[0].SendType = 0;
			send[0].RemoteFlag = 0;
			send[0].ExternFlag = 1;
			send[0].DataLen = 1;
			send[0].Data = new byte[8];

			send[0].Data[0] = (byte)CANCMD.CAN_CMD_READ_CAL;

			retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);
			if (retval != CAN.STATUS_OK)
				return sc;
			
			ctl.getCanFrames(out receive, 3);

			byte[] buffer = new byte[Marshal.SizeOf(typeof(SlaveConfig))];

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = receive[i / 8].Data[i % 8];
			}
			return StructTools.byteArray2Struct<SlaveConfig>(buffer);

		}

		//Get the calibration structure from the master unit
		public bool setCalibration(UInt32 CanID, SlaveConfig sc)
		{
			UInt32 retval;
			CAN.VCI_CAN_OBJ[] send = ctl.allocateCanFrameBuffer(1 + Marshal.SizeOf(typeof(SlaveConfig)) / 8);
			CAN.VCI_CAN_OBJ[] receive;

			send[0].ID = CanID;
			send[0].TimeFlag = 0;
			send[0].SendType = 0;
			send[0].RemoteFlag = 0;
			send[0].ExternFlag = 1;
			send[0].DataLen = 1;

			send[0].Data[0] = (byte)CANCMD.CAN_CMD_WRITE_CAL;

			retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);
			if (retval != CAN.STATUS_OK)
				return false;

			ctl.sendData(StructTools.struct2byteArray<SlaveConfig>(sc), CanID, false, true);

			ctl.getCanFrames(out receive, 1);

			if(receive[0].Data[0] == (byte)CANCMD.CAN_CMD_WRITE_CAL)
				return true;
			else
				return false;

		}

		public ushort[] getLocalVoltages(UInt32 CanID)
		{
			UInt32 retval;
			CAN.VCI_CAN_OBJ[] send = new CAN.VCI_CAN_OBJ[1];
			CAN.VCI_CAN_OBJ[] receive;

			VoltagesStr vStr;

			send[0].ID = CanID;
			send[0].TimeFlag = 0;
			send[0].SendType = 0;
			send[0].RemoteFlag = 0;
			send[0].ExternFlag = 1;
			send[0].DataLen = 1;
			send[0].Data = new byte[8];

			send[0].Data[0] = (byte)CANCMD.CAN_CMD_GET_LOCAL_VOLTAGES;

			retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);
			if (retval != CAN.STATUS_OK)
				return new ushort[0];

			ctl.getCanFrames(out receive, 2);

			byte[] buffer = new byte[Marshal.SizeOf(typeof(VoltagesStr))];

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = receive[i / 8].Data[i % 8];
			}

			vStr = StructTools.byteArray2Struct<VoltagesStr>(buffer);

			return vStr.voltages;
		}

		public bool saveCalibration(UInt32 CanID)
		{
			UInt32 retval;
			CAN.VCI_CAN_OBJ[] send = new CAN.VCI_CAN_OBJ[1];
			CAN.VCI_CAN_OBJ[] receive;

			VoltagesStr vStr;

			send[0].ID = CanID;
			send[0].TimeFlag = 0;
			send[0].SendType = 0;
			send[0].RemoteFlag = 0;
			send[0].ExternFlag = 1;
			send[0].DataLen = 1;
			send[0].Data = new byte[8];

			send[0].Data[0] = (byte)CANCMD.CAN_CMD_SAVE_CAL;

			retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);
			if (retval != CAN.STATUS_OK)
				return false;

			ctl.getCanFrames(out receive, 1);

			if (receive[0].Data[0] == (byte)CANCMD.CAN_CMD_SAVE_CAL)
				return true;
			else
				return false;
		}


		//Get the configuration structure from the master unit
		public Config_t getConfig(UInt32 CanID)
		{
			Config_t config = new Config_t();
			CAN.VCI_CAN_OBJ[] receive;

			byte[] txMsg = {(byte)CANCMD.CAN_CMD_GET_CONFIG};

			if (!ctl.sendData(txMsg, CanID, false, true))
				return config;

			ctl.getCanFrames(out receive, (Marshal.SizeOf(typeof(Config_t)) - 1) / 8 + 1);

			byte[] buffer = new byte[Marshal.SizeOf(typeof(Config_t))];

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = receive[i / 8].Data[i % 8];
			}
			return StructTools.byteArray2Struct<Config_t>(buffer);

		}

		//Send the configuration structure
		public bool setConfig(UInt32 CanID, Config_t config)
		{
			CAN.VCI_CAN_OBJ[] receive;

			byte[] txMsg = { (byte)CANCMD.CAN_CMD_SET_CONFIG };
			ctl.sendData(txMsg, CanID, false, true);

			ctl.sendData(StructTools.struct2byteArray<Config_t>(config), CanID, false, true);

			ctl.getCanFrames(out receive, 1);

			if (receive[0].Data[0] == (byte)CANCMD.CAN_CMD_SET_CONFIG)
				return true;
			else
				return false;

		}

		public bool saveConfig(UInt32 CanID)
		{
			CAN.VCI_CAN_OBJ[] receive;

			byte[] txMsg = { (byte)CANCMD.CAN_CMD_SAVE_CONFIG };
			ctl.sendData(txMsg, CanID, false, true);

			ctl.getCanFrames(out receive, 1);

			if (receive[0].Data[0] == (byte)CANCMD.CAN_CMD_SAVE_CONFIG)
				return true;
			else
				return false;

		}

		public bool balBusTx(UInt32 CanID, byte[] data)
		{
			UInt32 retval;
			CAN.VCI_CAN_OBJ[] send = new CAN.VCI_CAN_OBJ[1];

			send[0].ID = CanID;
			send[0].TimeFlag = 0;
			send[0].SendType = 0;
			send[0].RemoteFlag = 0;
			send[0].ExternFlag = 1;
			send[0].DataLen = 3;
			send[0].Data = new byte[8];

			send[0].Data[0] = (byte)CANCMD.CAN_CMD_BALBUS_TX;
			send[0].Data[1] = 0;

			for (int i = 0; i < data.Length; i++)
			{

				send[0].Data[2] = data[i];

				retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, send, 1);
				if (retval != CAN.STATUS_OK)
					return false;
				System.Threading.Thread.Sleep(10);
			}
			return true;
		}

		public bool balBusRx(out byte[] data, out int bytesReceived, int maxLength, int timeout)
		{
			CAN.VCI_CAN_OBJ receive;
			Stopwatch stopWatch = new Stopwatch();
			int dataCount = 0;

			stopWatch.Start();
			
			data = new byte[maxLength];

			while (dataCount < maxLength && stopWatch.ElapsedMilliseconds < timeout)
			{
				if (ctl.rxFrameCount() > 0)
				{
					ctl.getCanFrame(out receive);

					data[dataCount] = receive.Data[2];
					dataCount++;

				}
			}
			bytesReceived = dataCount;

			return true;
		}

		public bool getPackData()
		{

			return true;
		}
	}
}
