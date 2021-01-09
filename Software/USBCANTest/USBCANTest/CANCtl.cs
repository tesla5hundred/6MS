using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace USBCANTest
{
	public class CANCtl
	{
		const int BUFFER_SIZE = 500;
		const int RX_BUF_SIZE = 500;
		private CAN.VCI_CAN_OBJ[] receive;
		public volatile CAN.VCI_CAN_OBJ[] rxBuffer;

		private static volatile uint rxBufferWPos, rxBufferRPos;
		private Thread t;
		private volatile bool running;

		public CANCtl()
		{
			running = false;
			receive = new CAN.VCI_CAN_OBJ[BUFFER_SIZE];
			rxBuffer = new CAN.VCI_CAN_OBJ[RX_BUF_SIZE];

			for (int i = 0; i < BUFFER_SIZE; i++)
			{
				receive[i].Data = new byte[8];
				receive[i].Reserved = new byte[3];
			}

			for (int i = 0; i < RX_BUF_SIZE; i++)
			{
				rxBuffer[i].Data = new byte[8];
				rxBuffer[i].Reserved = new byte[3];
			}

		}

		~CANCtl()
		{
			close();
		}

		public bool open()
		{
			t = new Thread(new ThreadStart(rxThread));
			running = true;
			t.Start();
			return true;
		}

		public bool close()
		{
			if (running)
			{
				running = false;
				t.Join(100);
				if (t.IsAlive)
					t.Abort();
			}
			return true;
		}

		public uint rxFrameCount()
		{
			//Correct for rollover of write position
			uint wpos = rxBufferWPos;
			if (rxBufferWPos < rxBufferRPos)
				wpos += RX_BUF_SIZE;
			//Return the data count
			return wpos - rxBufferRPos;
		}

		//Remove any data from the receive buffer
		public void flush()
		{
			rxBufferRPos = rxBufferWPos;
		}

		public bool getCanFrame(out CAN.VCI_CAN_OBJ frame)
		{
			if (rxFrameCount() > 0)
			{
				frame = rxBuffer[rxBufferRPos];
				rxBufferRPos = (rxBufferRPos + 1) % RX_BUF_SIZE;
				return true;
			}
			else
			{
				frame = new CAN.VCI_CAN_OBJ();
				return false;
			}
		}

		public bool getCanFrames(out CAN.VCI_CAN_OBJ [] frames, int length, int timeout = 1000)
		{
			Stopwatch rxSw = new Stopwatch();
			int count = 0;
			CAN.VCI_CAN_OBJ frame;
			frames = new CAN.VCI_CAN_OBJ[length];
			rxSw.Start();
			do
			{
				while (rxFrameCount() == 0)
				{
					System.Threading.Thread.Sleep(1);	//Wait for data
					if (rxSw.ElapsedMilliseconds >= timeout)
						return false;
				}

				getCanFrame(out frame);
				frames[count] = frame;
				count++;
			} while (count < length && rxSw.ElapsedMilliseconds < timeout);

			rxSw.Stop();

			if (count < length)
				return false;	//Not all data received
			else
				return true;	//All data received
		}

		public CAN.VCI_CAN_OBJ allocateCanFrame()
		{
			CAN.VCI_CAN_OBJ frame = new CAN.VCI_CAN_OBJ();

			frame.Data = new byte[8];
			frame.Reserved = new byte[3];

			return frame;
		}

		public CAN.VCI_CAN_OBJ[] allocateCanFrameBuffer(int length)
		{
			CAN.VCI_CAN_OBJ [] frames = new CAN.VCI_CAN_OBJ[length];

			for (int i = 0; i < length; i++)
			{
				frames[i].Data = new byte[8];
				frames[i].Reserved = new byte[3];
			}

			return frames;
		}

		public bool sendData(byte[] buffer, uint ID, bool remote, bool extended)
		{
			//Allocate enough frames for the data buffer length
			int bufCount = (buffer.Length - 1) / 8 + 1;
			CAN.VCI_CAN_OBJ[] buf = allocateCanFrameBuffer(bufCount);

			int txSize = buffer.Length;

			for(int i = 0; i < bufCount; i++)
			{
				buf[i].ID = ID;
				buf[i].TimeFlag = 0;
				buf[i].SendType = 0;
				buf[i].RemoteFlag = remote ? (byte)1 : (byte)0;
				buf[i].ExternFlag = extended ? (byte)1 : (byte)0;
				buf[i].DataLen = (byte)(txSize > 8 ? 8 : txSize);	//Transmit full packets unless the amount of data calls for a final packet that's smaller

				//Copy the data
				for(int j = 0; j < buf[i].DataLen; j++)
				{
					buf[i].Data[j] = buffer[i * 8 + j];
				}
				txSize -= 8;
			}
			CAN.VCI_CAN_OBJ[] tempBuf = allocateCanFrameBuffer(1);

			for(int i = 0; i < bufCount; i++)
			{
				tempBuf[0] = buf[i];
				System.Threading.Thread.Sleep(5);
				UInt32 retval = CAN.VCI_Transmit(CAN.DEV_USBCAN2, 0, 0, tempBuf, (uint)1/*bufCount*/);
				if (retval != CAN.STATUS_OK)
					return false;
				
			}
			//else
				return true;

		}
		public uint ID;
			public uint TimeStamp;		//Time stamp
			public byte TimeFlag;		//Use time stamp
			public byte SendType;		//Send flag. Reserved, do not use
			public byte RemoteFlag;		//Remote frame
			public byte ExternFlag;		//Extended Frame
			public byte DataLen;
		private void rxThread()
		{
			uint temp = 0;
			uint len;

			while (running)
			{
				len = CAN.VCI_Receive(CAN.DEV_USBCAN2, 0, 0, receive, 500, 10);

				if (len == 0xFFFFFFFF)	//Return value of -1 indicates failure
					len = 0;

				if (len > 20)
					temp++;

				for (int pkt = 0; pkt < len; pkt++)
				{
					rxBuffer[rxBufferWPos] = receive[pkt];
					rxBufferWPos = (rxBufferWPos + 1) % RX_BUF_SIZE;

				}

				System.Threading.Thread.Sleep(1);
			}
		}



	}
}
