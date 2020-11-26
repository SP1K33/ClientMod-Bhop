using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ClientMod_Bhop
{
	public class ClientMod
	{
		#region Singleton
		private ClientMod() { }
		private static ClientMod _instance;
		public static ClientMod Instance => _instance ?? (_instance = new ClientMod());
		#endregion

		private Process _process { get; set; }
		public int ClientDLL { get; private set; }

		public bool Init()
		{
			_process = TryGetProccess();
			if (_process == null)
			{
				return false;
			}

			ClientDLL = GetClientDLL();
			if (ClientDLL == 0)
			{
				return false;
			}

			return true;
		}

		private Process TryGetProccess()
		{
			var processes = Process.GetProcessesByName("CMLauncher");
			return Array.Find(processes, m => m.MainWindowTitle == "CS:S v34 ClientMod");
		}

		private int GetClientDLL()
		{
			foreach (ProcessModule module in _process.Modules)
			{
				if (module.ModuleName == "client.dll")
				{
					return (int)module.BaseAddress;
				}
			}
			return 0;
		}

		public T ReadMemory<T>(int address) where T : struct
		{
			int byteSize = Marshal.SizeOf(typeof(T));
			byte[] buffer = new byte[byteSize];
			ReadProcessMemory(_process.Handle, new IntPtr(address), buffer, buffer.Length, out _);
			var structure = ByteArrayToStructure<T>(buffer);
			return structure;
		}

		public void WriteMemory(int address, object value, bool needConvertToStructure = true)
		{
			var buffer = needConvertToStructure ? StructureToByteArray(value) : (byte[])value;
			WriteProcessMemory(_process.Handle, new IntPtr(address), buffer, buffer.Length, 0);
		}

		private T ByteArrayToStructure<T>(byte[] bytes) where T : struct
		{
			var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
			try
			{
				return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			}
			finally
			{
				handle.Free();
			}
		}

		private byte[] StructureToByteArray(object obj)
		{
			int length = Marshal.SizeOf(obj);
			var ptr = Marshal.AllocHGlobal(length);
			Marshal.StructureToPtr(obj, ptr, true);

			byte[] byteArray = new byte[length];
			Marshal.Copy(ptr, byteArray, 0, length);
			Marshal.FreeHGlobal(ptr);
			return byteArray;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		[DllImport("kernel32.dll")]
		private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);
	}
}