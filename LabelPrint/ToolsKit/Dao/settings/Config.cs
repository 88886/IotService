using System;
using System.Configuration;
using System.IO;

namespace PrintX.Dev.Utils.ToolsKit
{
	public sealed class Config
	{
		private const string SectionName = "carpaSettings";

		private string _filename;

        private System.Configuration.Configuration _configuration;

		private FileSystemWatcher _fileSystemWatcher;

		public string Filename
		{
			get
			{
				return this._filename;
			}
			set
			{
				if (this._filename != value)
				{
					this._filename = value;
					this.Clear();
					this.Watch();
				}
			}
		}

        public System.Configuration.Configuration Configuration
		{
			get
			{
                System.Configuration.Configuration configuration;
				lock (this)
				{
					if (this._configuration == null)
					{
						this._configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
						{
							ExeConfigFilename = this.Filename
						}, ConfigurationUserLevel.None);
					}
					configuration = this._configuration;
				}
				return configuration;
			}
		}

		public SktSettingsSection Sections
		{
			get
			{
				SktSettingsSection carpaSettingsSection = this.Configuration.GetSection("carpaSettings") as SktSettingsSection;
				SktSettingsSection result;
				lock (this)
				{
					if (carpaSettingsSection == null)
					{
						carpaSettingsSection = new SktSettingsSection();
						this._configuration.Sections.Add("carpaSettings", carpaSettingsSection);
						carpaSettingsSection.SectionInformation.SetRawXml("<carpaSettings/>");
						carpaSettingsSection.SectionInformation.ForceSave = true;
					}
					result = carpaSettingsSection;
				}
				return result;
			}
		}

		public ConfigSection AppSettings
		{
			get
			{
				return this.Sections["appSettings"];
			}
		}

		public Config(string filename)
		{
			this.Filename = filename;
		}

		private void Watch()
		{
			if ("Web.config".Equals(this._filename, System.StringComparison.OrdinalIgnoreCase))
			{
				if (this._fileSystemWatcher != null)
				{
					this._fileSystemWatcher.EnableRaisingEvents = false;
				}
			}
			else if (System.IO.File.Exists(this._filename))
			{
				this._fileSystemWatcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(this._filename));
				this._fileSystemWatcher.Filter = System.IO.Path.GetFileName(this._filename);
				this._fileSystemWatcher.Changed += new FileSystemEventHandler(this.HandleFileChanged);
				this._fileSystemWatcher.EnableRaisingEvents = true;
			}
		}

		private void HandleFileChanged(object sender, FileSystemEventArgs e)
		{
			if (this._configuration != null)
			{
				this.Clear();
			}
		}

		private void Clear()
		{
			lock (this)
			{
				this._configuration = null;
			}
		}

		public void Save()
		{
			if (this._configuration != null)
			{
				bool flag = System.IO.File.Exists(this._filename);
				this._configuration.Save(ConfigurationSaveMode.Modified);
				if (!flag)
				{
					this.Watch();
				}
			}
		}

		public void Delete()
		{
			System.IO.File.Delete(this._filename);
			this.Clear();
		}
	}
}
