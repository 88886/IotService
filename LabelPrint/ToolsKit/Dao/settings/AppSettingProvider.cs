using System;
using System.Configuration;

namespace PrintX.Dev.Utils.ToolsKit
{
	public class AppSettingProvider : IConfiguration
	{
		public string GetConfig(string configKey)
		{
			return ConfigurationManager.AppSettings[configKey];
		}
	}
}
