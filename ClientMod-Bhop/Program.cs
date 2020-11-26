using Colorful;
using System.Drawing;
using System.Security.Principal;

namespace ClientMod_Bhop
{
	public static class Program
	{
		private static bool IsElevated => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
		private static Color _purpleColor => ColorTranslator.FromHtml("#FF9332A8");

		private static void UpdateConsole(bool state)
		{
			Console.Clear();
			Console.Write("\n\n\n");
			Console.WriteAscii("   CLIENT MOD", _purpleColor);
			Console.WriteAscii("   BHOP by SP1K3", _purpleColor);
			Console.WriteAscii($"   STATE: {(state ? "ON" : "OFF")}", _purpleColor);
		}

		private static void OnStateChanged(bool state)
		{
			UpdateConsole(state);
		}

		private static void Main()
		{
			if (!IsElevated)
			{
				Console.WriteLine("Run as administrator", Color.MediumVioletRed);
				Console.Read();
				return;
			}

			var clientMod = ClientMod.Instance;
			var initialResult = clientMod.Init();
			if (!initialResult)
			{
				Console.WriteLine("ClientMod or 'client.dll' not found", Color.MediumVioletRed);
				Console.Read();
				return;
			}

			UpdateConsole(true);

			var bunnyHop = new BunnyHop();
			bunnyHop.StateChangeEvent += OnStateChanged;
			bunnyHop.Run();
		}
	}
}
