using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace USBCANTest
{
	static class StructTools
	{

		public static byte[] struct2byteArray<T>(T str)
		{
			byte[] buffer = new byte[Marshal.SizeOf(typeof(T))];
			GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

			try
			{
				Marshal.StructureToPtr(str, gcHandle.AddrOfPinnedObject(), false);
				return buffer;
			}

			finally
			{
				gcHandle.Free();
			}
		}

		public static T byteArray2Struct<T>(byte[] buffer)
		{
			GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

			try
			{
				return (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
			}

			finally
			{
				gcHandle.Free();
			}
		}
	}
}
