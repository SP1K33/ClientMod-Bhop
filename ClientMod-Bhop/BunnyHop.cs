using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ClientMod_Bhop
{
	public class BunnyHop
	{
		public event Action<bool> StateChangeEvent;
		private bool _state = true;

		public void Run()
		{
			while (true)
			{
				if (_state && HoldingKey(VK_SPACE) && Player.GetHealth() > 0)
				{
					int jumpState = (Player.GetFlag() & (1 << 0)) > 0 ? 5 : 4;
					if (Player.GetJumpState() != jumpState)
					{
						Player.SetJumpState(jumpState);
					}
				}

				if (HoldingKey(VK_DELETE))
				{
					_state = !_state;
					StateChangeEvent?.Invoke(_state);
					Thread.Sleep(300);
				}

				Thread.Sleep(1);
			}
		}

		private bool HoldingKey(int keyID)
		{
			return (GetAsyncKeyState(keyID) & 0x8000) > 0;
		}

		private const int VK_SPACE = 0x20;
		public const int VK_DELETE = 0x2E;
		[DllImport("user32.dll")] private static extern short GetAsyncKeyState(int vKey);
	}
}