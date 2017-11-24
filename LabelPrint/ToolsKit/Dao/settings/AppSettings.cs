using System;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class AppSettings
	{
		private static IConfiguration _provider;

		public static IConfiguration Provider
		{
			get
			{
				IConfiguration arg_15_0;
				if ((arg_15_0 = AppSettings._provider) == null)
				{
					arg_15_0 = (AppSettings._provider = new AppSettingProvider());
				}
				return arg_15_0;
			}
			set
			{
				AppSettings._provider = value;
			}
		}

		public static string GetString(string keyName)
		{
			string config = AppSettings.Provider.GetConfig(keyName);
			return (!string.IsNullOrEmpty(config)) ? config : null;
		}

		public static string GetString(string keyName, string defaultValue)
		{
			string @string = AppSettings.GetString(keyName);
			return (@string != null) ? @string : defaultValue;
		}

		public static string DecryptGetString(string keyName)
		{
			string @string = AppSettings.GetString(keyName);
			return (@string != null) ? StringUtils.Decrypt(@string) : string.Empty;
		}

		public static int GetInt(string keyName)
		{
			return AppSettings.GetInt(keyName, 0);
		}

		public static int GetInt(string keyName, int defaultValue)
		{
			string @string = AppSettings.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : int.Parse(@string);
		}

		public static bool GetBool(string keyName)
		{
			return AppSettings.GetBool(keyName, false);
		}

		public static bool GetBool(string keyName, bool defaultValue)
		{
			string @string = AppSettings.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : bool.Parse(@string);
		}

		public static double GetDouble(string keyName)
		{
			return AppSettings.GetDouble(keyName, 0.0);
		}

		public static double GetDouble(string keyName, double defaultValue)
		{
			string @string = AppSettings.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : double.Parse(@string);
		}

		public static System.DateTime GetDateTime(string keyName)
		{
			return AppSettings.GetDateTime(keyName, System.DateTime.MinValue);
		}

		public static System.DateTime GetDateTime(string keyName, System.DateTime defaultValue)
		{
			string @string = AppSettings.GetString(keyName);
			return string.IsNullOrEmpty(@string) ? defaultValue : System.DateTime.Parse(@string);
		}
	}
}
