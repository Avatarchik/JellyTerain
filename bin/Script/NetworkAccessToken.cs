using System;

namespace UnityEngine.Networking.Types
{
	public class NetworkAccessToken
	{
		private const int NETWORK_ACCESS_TOKEN_SIZE = 64;

		public byte[] array;

		public NetworkAccessToken()
		{
			array = new byte[64];
		}

		public NetworkAccessToken(byte[] array)
		{
			this.array = array;
		}

		public NetworkAccessToken(string strArray)
		{
			try
			{
				array = Convert.FromBase64String(strArray);
			}
			catch (Exception)
			{
				array = new byte[64];
			}
		}

		public string GetByteString()
		{
			return Convert.ToBase64String(array);
		}

		public bool IsValid()
		{
			if (this.array == null || this.array.Length != 64)
			{
				return false;
			}
			bool result = false;
			byte[] array = this.array;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}
	}
}
