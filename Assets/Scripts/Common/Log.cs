using UnityEngine;
using System;

namespace RuzikOdyssey.Common
{
	public static class Log
	{
		private static void LogMessage(LogMessageType messageType, string message)
		{
			UnityEngine.Debug.Log(String.Format("[{0}] - {1}", EnumUtility.GetName(messageType), message));
		}

		public static void Info(string format, params object[] args)
		{
			LogMessage(LogMessageType.Info, String.Format(format, args));
		}

		public static void Debug(string format, params object[] args)
		{
			LogMessage(LogMessageType.Debug, String.Format(format, args));
		}

		public static void Warning(string format, params object[] args)
		{
			LogMessage(LogMessageType.Warning, String.Format(format, args));
		}

		public static void Error(string format, params object[] args)
		{
			UnityEngine.Debug.LogError(String.Format(format, args));
		}
	}
}
