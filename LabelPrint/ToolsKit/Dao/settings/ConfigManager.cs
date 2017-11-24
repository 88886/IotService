using System;
using System.IO;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class ConfigManager
	{
		private static readonly object ConfigLock = new object();

		private static Config _sysConfig;

		private static Config _userConfig;

		public static Config SysConfig
		{
			get
			{
				lock (ConfigManager.ConfigLock)
				{
					if (ConfigManager._sysConfig == null)
					{
						ConfigManager._sysConfig = new Config(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
					}
				}
				return ConfigManager._sysConfig;
			}
		}

		public static Config UserConfig
		{
			get
			{
				lock (ConfigManager.ConfigLock)
				{
					if (ConfigManager._userConfig == null)
					{
						ConfigManager._userConfig = new Config(ConfigManager.UserConfigFilename);
					}
				}
				return ConfigManager._userConfig;
			}
		}

		private static string UserConfigFilename
		{
			get
			{
				System.IO.FileInfo fileInfo = new System.IO.FileInfo(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				string text = fileInfo.Name;
				if (text.EndsWith(".vshost.exe.config", System.StringComparison.OrdinalIgnoreCase))
				{
					text = ConfigManager.ChangeExtension(text, 3);
				}
				else if (text.EndsWith(".exe.config", System.StringComparison.OrdinalIgnoreCase))
				{
					text = ConfigManager.ChangeExtension(text, 2);
				}
				else
				{
					text = ConfigManager.ChangeExtension(text, 1);
				}
				return System.IO.Path.Combine(fileInfo.DirectoryName, "data\\" + text);
			}
		}

		private static string ChangeExtension(string name, int stripCount)
		{
			for (int i = 0; i < stripCount; i++)
			{
				name = System.IO.Path.GetFileNameWithoutExtension(name);
			}
			return name + ".user.config";
		}
	}
}
