using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics
{
	[StructLayout(LayoutKind.Sequential)]
	internal class CustomEventData : IDisposable
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private CustomEventData()
		{
		}

		public CustomEventData(string name)
		{
			InternalCreate(name);
		}

		~CustomEventData()
		{
			InternalDestroy();
		}

		public void Dispose()
		{
			InternalDestroy();
			GC.SuppressFinalize(this);
		}

		public bool Add(string key, string value)
		{
			return AddString(key, value);
		}

		public bool Add(string key, bool value)
		{
			return AddBool(key, value);
		}

		public bool Add(string key, char value)
		{
			return AddChar(key, value);
		}

		public bool Add(string key, byte value)
		{
			return AddByte(key, value);
		}

		public bool Add(string key, sbyte value)
		{
			return AddSByte(key, value);
		}

		public bool Add(string key, short value)
		{
			return AddInt16(key, value);
		}

		public bool Add(string key, ushort value)
		{
			return AddUInt16(key, value);
		}

		public bool Add(string key, int value)
		{
			return AddInt32(key, value);
		}

		public bool Add(string key, uint value)
		{
			return AddUInt32(key, value);
		}

		public bool Add(string key, long value)
		{
			return AddInt64(key, value);
		}

		public bool Add(string key, ulong value)
		{
			return AddUInt64(key, value);
		}

		public bool Add(string key, float value)
		{
			return AddDouble(key, (double)Convert.ToDecimal(value));
		}

		public bool Add(string key, double value)
		{
			return AddDouble(key, value);
		}

		public bool Add(string key, decimal value)
		{
			return AddDouble(key, (double)Convert.ToDecimal(value));
		}

		public bool Add(IDictionary<string, object> eventData)
		{
			foreach (KeyValuePair<string, object> eventDatum in eventData)
			{
				string key = eventDatum.Key;
				object value = eventDatum.Value;
				if (value == null)
				{
					Add(key, "null");
				}
				else
				{
					Type type = value.GetType();
					if (type == typeof(string))
					{
						Add(key, (string)value);
					}
					else if (type == typeof(char))
					{
						Add(key, (char)value);
					}
					else if (type == typeof(sbyte))
					{
						Add(key, (sbyte)value);
					}
					else if (type == typeof(byte))
					{
						Add(key, (byte)value);
					}
					else if (type == typeof(short))
					{
						Add(key, (short)value);
					}
					else if (type == typeof(ushort))
					{
						Add(key, (ushort)value);
					}
					else if (type == typeof(int))
					{
						Add(key, (int)value);
					}
					else if (type == typeof(uint))
					{
						Add(eventDatum.Key, (uint)value);
					}
					else if (type == typeof(long))
					{
						Add(key, (long)value);
					}
					else if (type == typeof(ulong))
					{
						Add(key, (ulong)value);
					}
					else if (type == typeof(bool))
					{
						Add(key, (bool)value);
					}
					else if (type == typeof(float))
					{
						Add(key, (float)value);
					}
					else if (type == typeof(double))
					{
						Add(key, (double)value);
					}
					else if (type == typeof(decimal))
					{
						Add(key, (decimal)value);
					}
					else
					{
						if (!type.IsValueType)
						{
							throw new ArgumentException($"Invalid type: {type} passed");
						}
						Add(key, value.ToString());
					}
				}
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		public extern void InternalCreate(string name);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[ThreadAndSerializationSafe]
		[GeneratedByOldBindingsGenerator]
		internal extern void InternalDestroy();

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddString(string key, string value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddBool(string key, bool value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddChar(string key, char value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddByte(string key, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddSByte(string key, sbyte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddInt16(string key, short value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddUInt16(string key, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddInt32(string key, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddUInt32(string key, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddInt64(string key, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddUInt64(string key, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		[GeneratedByOldBindingsGenerator]
		private extern bool AddDouble(string key, double value);
	}
}
