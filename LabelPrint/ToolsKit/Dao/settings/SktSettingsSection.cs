using System;
using System.Configuration;
using System.Xml;

namespace PrintX.Dev.Utils.ToolsKit
{
    public class SktSettingsSection : ConfigurationSection
	{
		private XmlNode sectionsNode;

		private bool isModified;

		public new ConfigSection this[string sectionName]
		{
			get
			{
				return new ConfigSection(this, this.sectionsNode, this.sectionsNode.SelectSingleNode(sectionName), sectionName);
			}
		}

		protected override void DeserializeSection(XmlReader reader)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
			this.sectionsNode = xmlDocument.DocumentElement;
		}

		protected override string SerializeSection(ConfigurationElement parentSection, string name, ConfigurationSaveMode saveMode)
		{
			return this.sectionsNode.OuterXml;
		}

		protected override bool IsModified()
		{
			return this.isModified;
		}

		protected override void Reset(ConfigurationElement parentSection)
		{
			this.isModified = false;
		}

		internal void Modified()
		{
			this.isModified = true;
		}
	}
}
