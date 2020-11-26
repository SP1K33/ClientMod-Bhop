using System.Collections.Specialized;
using System.Numerics;

namespace ClientMod_Bhop
{
	public class Player
	{
		private static ClientMod _clientMod => ClientMod.Instance;

		public static int GetJumpState()
		{
			return _clientMod.ReadMemory<int>(_clientMod.ClientDLL + Offsets.ForceJump);
		}

		public static int GetFlag()
		{
			return _clientMod.ReadMemory<int>(GetLocalPlayer() + Offsets.Flag);
		}

		private static int GetLocalPlayer()
		{
			return _clientMod.ReadMemory<int>(_clientMod.ClientDLL + Offsets.LocalPlayer);
		}

		public static int GetHealth()
		{
			return _clientMod.ReadMemory<int>(GetLocalPlayer() + Offsets.Health);
		}

		public static void SetJumpState(int state)
		{
			_clientMod.WriteMemory(_clientMod.ClientDLL + Offsets.ForceJump, state);
		}

		public static float GetVelocity()
		{
			return _clientMod.ReadMemory<float>(_clientMod.ClientDLL + Offsets.Velocity);
		}
	}
}