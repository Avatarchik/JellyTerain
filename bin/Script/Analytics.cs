using System;
using System.Collections.Generic;

namespace UnityEngine.Analytics
{
	public static class Analytics
	{
		private static UnityAnalyticsHandler s_UnityAnalyticsHandler;

		public static bool limitUserTracking
		{
			get
			{
				return UnityAnalyticsHandler.limitUserTracking;
			}
			set
			{
				UnityAnalyticsHandler.limitUserTracking = value;
			}
		}

		public static bool deviceStatsEnabled
		{
			get
			{
				return UnityAnalyticsHandler.deviceStatsEnabled;
			}
			set
			{
				UnityAnalyticsHandler.deviceStatsEnabled = value;
			}
		}

		public static bool enabled
		{
			get
			{
				return GetUnityAnalyticsHandler()?.enabled ?? false;
			}
			set
			{
				UnityAnalyticsHandler unityAnalyticsHandler = GetUnityAnalyticsHandler();
				if (unityAnalyticsHandler != null)
				{
					unityAnalyticsHandler.enabled = value;
				}
			}
		}

		internal static UnityAnalyticsHandler GetUnityAnalyticsHandler()
		{
			if (s_UnityAnalyticsHandler == null)
			{
				s_UnityAnalyticsHandler = new UnityAnalyticsHandler();
			}
			return s_UnityAnalyticsHandler;
		}

		public static AnalyticsResult FlushEvents()
		{
			return GetUnityAnalyticsHandler()?.FlushEvents() ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult SetUserId(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				throw new ArgumentException("Cannot set userId to an empty or null string");
			}
			return GetUnityAnalyticsHandler()?.SetUserId(userId) ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult SetUserGender(Gender gender)
		{
			return GetUnityAnalyticsHandler()?.SetUserGender(gender) ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult SetUserBirthYear(int birthYear)
		{
			UnityAnalyticsHandler unityAnalyticsHandler = GetUnityAnalyticsHandler();
			if (s_UnityAnalyticsHandler == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			return unityAnalyticsHandler.SetUserBirthYear(birthYear);
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency)
		{
			return GetUnityAnalyticsHandler()?.Transaction(productId, Convert.ToDouble(amount), currency, null, null) ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
		{
			return GetUnityAnalyticsHandler()?.Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature) ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature, bool usingIAPService)
		{
			return GetUnityAnalyticsHandler()?.Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature, usingIAPService) ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult CustomEvent(string customEventName)
		{
			if (string.IsNullOrEmpty(customEventName))
			{
				throw new ArgumentException("Cannot set custom event name to an empty or null string");
			}
			return GetUnityAnalyticsHandler()?.CustomEvent(customEventName) ?? AnalyticsResult.NotInitialized;
		}

		public static AnalyticsResult CustomEvent(string customEventName, Vector3 position)
		{
			if (string.IsNullOrEmpty(customEventName))
			{
				throw new ArgumentException("Cannot set custom event name to an empty or null string");
			}
			UnityAnalyticsHandler unityAnalyticsHandler = GetUnityAnalyticsHandler();
			if (unityAnalyticsHandler == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			CustomEventData customEventData = new CustomEventData(customEventName);
			customEventData.Add("x", (double)Convert.ToDecimal(position.x));
			customEventData.Add("y", (double)Convert.ToDecimal(position.y));
			customEventData.Add("z", (double)Convert.ToDecimal(position.z));
			return unityAnalyticsHandler.CustomEvent(customEventData);
		}

		public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
		{
			if (string.IsNullOrEmpty(customEventName))
			{
				throw new ArgumentException("Cannot set custom event name to an empty or null string");
			}
			UnityAnalyticsHandler unityAnalyticsHandler = GetUnityAnalyticsHandler();
			if (unityAnalyticsHandler == null)
			{
				return AnalyticsResult.NotInitialized;
			}
			if (eventData == null)
			{
				return unityAnalyticsHandler.CustomEvent(customEventName);
			}
			CustomEventData customEventData = new CustomEventData(customEventName);
			customEventData.Add(eventData);
			return unityAnalyticsHandler.CustomEvent(customEventData);
		}
	}
}
