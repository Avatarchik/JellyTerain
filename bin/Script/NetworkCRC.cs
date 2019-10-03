using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Networking.NetworkSystem;

namespace UnityEngine.Networking
{
	public class NetworkCRC
	{
		internal static NetworkCRC s_Singleton;

		private Dictionary<string, int> m_Scripts = new Dictionary<string, int>();

		private bool m_ScriptCRCCheck;

		internal static NetworkCRC singleton
		{
			get
			{
				if (s_Singleton == null)
				{
					s_Singleton = new NetworkCRC();
				}
				return s_Singleton;
			}
		}

		public Dictionary<string, int> scripts => m_Scripts;

		public static bool scriptCRCCheck
		{
			get
			{
				return singleton.m_ScriptCRCCheck;
			}
			set
			{
				singleton.m_ScriptCRCCheck = value;
			}
		}

		public static void ReinitializeScriptCRCs(Assembly callingAssembly)
		{
			singleton.m_Scripts.Clear();
			Type[] types = callingAssembly.GetTypes();
			foreach (Type type in types)
			{
				if (type.GetBaseType() == typeof(NetworkBehaviour))
				{
					type.GetMethod(".cctor", BindingFlags.Static)?.Invoke(null, new object[0]);
				}
			}
		}

		public static void RegisterBehaviour(string name, int channel)
		{
			singleton.m_Scripts[name] = channel;
		}

		internal static bool Validate(CRCMessageEntry[] scripts, int numChannels)
		{
			return singleton.ValidateInternal(scripts, numChannels);
		}

		private bool ValidateInternal(CRCMessageEntry[] remoteScripts, int numChannels)
		{
			if (m_Scripts.Count != remoteScripts.Length)
			{
				if (LogFilter.logWarn)
				{
					Debug.LogWarning("Network configuration mismatch detected. The number of networked scripts on the client does not match the number of networked scripts on the server. This could be caused by lazy loading of scripts on the client. This warning can be disabled by the checkbox in NetworkManager Script CRC Check.");
				}
				Dump(remoteScripts);
				return false;
			}
			for (int i = 0; i < remoteScripts.Length; i++)
			{
				CRCMessageEntry cRCMessageEntry = remoteScripts[i];
				if (LogFilter.logDebug)
				{
					Debug.Log("Script: " + cRCMessageEntry.name + " Channel: " + cRCMessageEntry.channel);
				}
				if (m_Scripts.ContainsKey(cRCMessageEntry.name))
				{
					int num = m_Scripts[cRCMessageEntry.name];
					if (num != cRCMessageEntry.channel)
					{
						if (LogFilter.logError)
						{
							Debug.LogError("HLAPI CRC Channel Mismatch. Script: " + cRCMessageEntry.name + " LocalChannel: " + num + " RemoteChannel: " + cRCMessageEntry.channel);
						}
						Dump(remoteScripts);
						return false;
					}
				}
				if (cRCMessageEntry.channel >= numChannels)
				{
					if (LogFilter.logError)
					{
						Debug.LogError("HLAPI CRC channel out of range! Script: " + cRCMessageEntry.name + " Channel: " + cRCMessageEntry.channel);
					}
					Dump(remoteScripts);
					return false;
				}
			}
			return true;
		}

		private void Dump(CRCMessageEntry[] remoteScripts)
		{
			foreach (string key in m_Scripts.Keys)
			{
				Debug.Log("CRC Local Dump " + key + " : " + m_Scripts[key]);
			}
			for (int i = 0; i < remoteScripts.Length; i++)
			{
				CRCMessageEntry cRCMessageEntry = remoteScripts[i];
				Debug.Log("CRC Remote Dump " + cRCMessageEntry.name + " : " + cRCMessageEntry.channel);
			}
		}
	}
}
