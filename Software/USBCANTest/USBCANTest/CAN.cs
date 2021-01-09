using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;


/*
Notes:
Apparently the minimum value for VCI_Receive length is 50
 
CAN Baud rate	Timer0	Timer1 
5Kbps			0xBF	0xFF 
10Kbps			0x31	0x1C 
20Kbps			0x18	0x1C 
40Kbps			0x87	0xFF 
50Kbps			0x09	0x1C 
80Kbps			0x83	0Xff 
100Kbps			0x04	0x1C 
125Kbps			0x03	0x1C 
200Kbps			0x81	0xFA 
250Kbps			0x01	0x1C 
400Kbps			0x80	0xFA 
500Kbps			0x00	0x1C 
666Kbps			0x80	0xB6 
800Kbps			0x00	0x16 
1000Kbps		0x00	0x14 

 */

namespace USBCANTest
{
    public class CAN
    {
		//Adapter types
		public const UInt32 DEV_USBCAN = 3;     //Single channel adapter
		public const UInt32 DEV_USBCAN2 = 4;    //Dual channel adapter

		//Return code values
		public const Int32 STATUS_OK = 1;		//Success
		public const Int32 STATUS_ERR1 = 0;		//Operation failed
		public const Int32 STATUS_ERR2 = -1;	//Device not open


		/*------------------ZLG compatible functions and data types------------------*/

		//USB-CAN adapter information structure
		[StructLayout(LayoutKind.Sequential)]
		public struct VCI_BOARD_INFO
		{
			public ushort hw_Version;
			public ushort fw_Version;
			public ushort dr_Version;
			public ushort in_Version;
			public ushort irq_Num;
			public byte can_Num;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
			public string str_Serial_Num;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 40)]
			public string str_hw_Type;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
			public ushort[] Reserved;
		};

		//CAN message frame stucture
		[StructLayout(LayoutKind.Sequential)]
		public struct VCI_CAN_OBJ
		{
			public uint ID;
			public uint TimeStamp;		//Time stamp
			public byte TimeFlag;		//Use time stamp
			public byte SendType;		//Send flag. Reserved, do not use
			public byte RemoteFlag;		//Remote frame
			public byte ExternFlag;		//Extended Frame
			public byte DataLen;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] Data;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] Reserved;

		};

		//CAN Controller status structure
		[StructLayout(LayoutKind.Sequential)]
		public struct VCI_CAN_STATUS
		{
			public byte ErrInterrupt;
			public byte regMode;
			public byte regStatus;
			public byte regALCapture;
			public byte regECCapture;
			public byte regEWLimit;
			public byte regRECounter;
			public byte regTECounter;
			public UInt32 Reserved;
		};

		//Error info structure
		[StructLayout(LayoutKind.Sequential)]
		public struct VCI_ERR_INFO
		{
			public UInt32 ErrCode;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] Passive_ErrData;
			public byte ArLost_ErrData;
		};

		//CAN controler init info
		[StructLayout(LayoutKind.Sequential)]
		public struct VCI_INIT_CONFIG
		{
			public UInt32 AccCode;
			public UInt32 AccMask;
			public UInt32 InitFlag;
			public byte Filter;			//0,1: receives all frames. 2: standard frame filtering. 3: extended frame filtering.
			public byte Timing0;
			public byte Timing1;
			public byte Mode;			//0: normal mode. 1: listen-only mode. 2: self-test mode
		};

		[DllImport("ControlCAN.dll")]
		public static extern UInt32 VCI_OpenDevice(UInt32 DevType, UInt32 DevIndex, UInt32 Reserved);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32 VCI_CloseDevice(UInt32 DevType, UInt32 DevIndex);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32 VCI_InitCAN(UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, ref VCI_INIT_CONFIG pInitConfig);

		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_ReadBoardInfo (UInt32 DevType, UInt32 DevIndex, ref VCI_BOARD_INFO pInfo);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_ReadErrInfo (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, ref VCI_ERR_INFO pErrInfo);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_ReadCANStatus (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, ref VCI_CAN_STATUS pCANStatus);

		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_GetReference (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, UInt32 Reserved, ref IntPtr pData);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_SetReference (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, UInt32 RefType, ref IntPtr pData);

		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_GetReceiveNum (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_ClearBuffer (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex);

		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_StartCAN (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex);
		[DllImport("ControlCAN.dll")]
		public static extern UInt32  VCI_ResetCAN (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex);

		[DllImport("ControlCAN.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern UInt32 VCI_Receive(UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, [In, Out] VCI_CAN_OBJ[] pReceive, UInt32 Len, Int32 WaitTime);

		//Return value is the actual number of frames sent, -1 means device error
		[DllImport("ControlCAN.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern UInt32  VCI_Transmit (UInt32 DeviceType, UInt32 DeviceInd, UInt32 CANInd, [In] VCI_CAN_OBJ[] pSend, UInt32 Length);

    

	/* ------------ Other functions and data structures --------------------------------- */


	//USB-CAN bus adapter card information, a data type , the type of VCI_FindUsbDevice function return parameters.
	[StructLayout(LayoutKind.Sequential)]
	public struct VCI_BOARD_INFO1
	{
		public ushort hw_Version;
		public ushort fw_Version;
		public ushort dr_Version;
		public ushort in_Version;
		public ushort irq_Num;
		public byte can_Num;
		public byte Reserved;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
		public string str_Serial_Num;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
		public string str_hw_Type;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4*4 )]		//This is a char [4] [4]
		public char [] str_Usb_Serial;
	};

	//Define the general parameters of type
	[StructLayout(LayoutKind.Sequential)]
	public struct VCI_REF_NORMAL
	{
		public byte Mode;		//working mode
		public byte Filter;		//filtering methods
		public UInt32 AccCode;	//Receive filtering acceptance yards
		public UInt32 AccMask;	//Receive filtering mask
		public byte kBaudRate;	//Baud rate index ,0-SelfDefine ,1-5Kbps ( not used ) ,2 -18 were : 10kbps, 20kbps, 40kbps, 50kbps, 80kbps, 100kbps, 125kbps, 200kbps, 250kbps, 400kbps, 500kbps , 666kbps, 800kbps, 1000kbps, 33.33kbps, 66.66kbps, 83.33kbps
		public byte Timing0;
		public byte Timing1;
		public byte CANRX_EN;	//Reserved, do not use
		public byte UARTBAUD;	//Reserved, do not use
	} ;

	//Define the baud rate setting parameter type
	[StructLayout(LayoutKind.Sequential)]
	public struct VCI_BAUD_TYPE
	{
		public UInt32 Baud;		//store the actual value of the baud rate
		public byte SJW;		//synchronization jump width , the value 1-4
		public byte BRP;		//prescaler value , ranging 1-64
		public byte SAM;		//sampling points , ranging from 0 = sampling time, 1 = sampled three times
		public byte PHSEG2_SEL;	//phase segment 2 select bit , the value 0 = Phase Segment 1 time by the decision , 1 = programmable
		public byte PRSEG;		//propagation time range 1-8
		public byte PHSEG1;		//phase segment 1 , the value 1-8
		public byte PHSEG2;		//phase segment 2 , the value 1-8
	};

	//Define Reference parameter types
	[StructLayout(LayoutKind.Sequential)]
	public struct VCI_REF_STRUCT
	{
		VCI_REF_NORMAL RefNormal;
		public byte Reserved;
		VCI_BAUD_TYPE BaudType;
	};


	[DllImport("ControlCAN.dll")]
	public static extern UInt32 VCI_GetReference2 (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, UInt32 Reserved, ref VCI_REF_STRUCT pRefStruct);
	[DllImport("ControlCAN.dll")]
	public static extern UInt32 VCI_SetReference2 (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex, UInt32 RefType, ref byte [] pData);
	[DllImport("ControlCAN.dll")]
	public static extern UInt32 VCI_ResumeConfig (UInt32 DevType, UInt32 DevIndex, UInt32 CANIndex);
	
	[DllImport("ControlCAN.dll")]
	public static extern UInt32 VCI_ConnectDevice (UInt32 DevType, UInt32 DevIndex);
	[DllImport("ControlCAN.dll")]
	public static extern UInt32 VCI_UsbDeviceReset (UInt32 DevType, UInt32 DevIndex, UInt32 Reserved);
	[DllImport("ControlCAN.dll")]
	public static extern UInt32 VCI_FindUsbDevice (ref VCI_BOARD_INFO1 pInfo);

	}
   


}
