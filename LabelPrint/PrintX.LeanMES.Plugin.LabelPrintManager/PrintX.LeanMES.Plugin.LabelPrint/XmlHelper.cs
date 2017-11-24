using System;
using System.Collections;
using System.Xml;

namespace PrintX.LeanMES.Plugin.LabelPrint
{
	public class XmlHelper
	{
		private static XmlDocument xmldoc;

		private static XmlNode xmlnode;

		private static XmlElement xmlelem;

		public bool CreateXmlDocument(string FileName, string RootName, string Encode)
		{
			bool result;
			try
			{
				XmlHelper.xmldoc = new XmlDocument();
				XmlDeclaration newChild = XmlHelper.xmldoc.CreateXmlDeclaration("1.0", Encode, null);
				XmlHelper.xmldoc.AppendChild(newChild);
				XmlHelper.xmlelem = XmlHelper.xmldoc.CreateElement("", RootName, "");
				XmlHelper.xmldoc.AppendChild(XmlHelper.xmlelem);
				XmlHelper.xmldoc.Save(FileName);
				result = true;
			}
			catch (Exception var_1_64)
			{
                Console.WriteLine(var_1_64.Message);
				result = false;
			}
			return result;
		}

		public bool InsertNode(string XmlFile, string NewNodeName, bool HasAttributes, string fatherNode, Hashtable htAtt, Hashtable htSubNode)
		{
			bool result;
			try
			{
				XmlHelper.xmldoc = new XmlDocument();
				XmlHelper.xmldoc.Load(XmlFile);
				XmlNode xmlNode = XmlHelper.xmldoc.SelectSingleNode(fatherNode);
				XmlHelper.xmlelem = XmlHelper.xmldoc.CreateElement(NewNodeName);
				if (htAtt != null && HasAttributes)
				{
					this.SetAttributes(XmlHelper.xmlelem, htAtt);
					this.SetNodes(XmlHelper.xmlelem.Name, XmlHelper.xmldoc, XmlHelper.xmlelem, htSubNode);
				}
				else
				{
					this.SetNodes(XmlHelper.xmlelem.Name, XmlHelper.xmldoc, XmlHelper.xmlelem, htSubNode);
				}
				xmlNode.AppendChild(XmlHelper.xmlelem);
				XmlHelper.xmldoc.Save(XmlFile);
				result = true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return result;
		}

		public bool UpdateNode(string XmlFile, string fatherNode, Hashtable htAtt, Hashtable htSubNode)
		{
			bool result;
			try
			{
				XmlHelper.xmldoc = new XmlDocument();
				XmlHelper.xmldoc.Load(XmlFile);
				XmlNodeList childNodes = XmlHelper.xmldoc.SelectSingleNode(fatherNode).ChildNodes;
				this.UpdateNodes(childNodes, htAtt, htSubNode);
				XmlHelper.xmldoc.Save(XmlFile);
				result = true;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			return result;
		}

		public bool DeleteNodes(string XmlFile, string fatherNode)
		{
			bool result;
			try
			{
				XmlHelper.xmldoc = new XmlDocument();
				XmlHelper.xmldoc.Load(XmlFile);
				XmlHelper.xmlnode = XmlHelper.xmldoc.SelectSingleNode(fatherNode);
				XmlHelper.xmlnode.RemoveAll();
				XmlHelper.xmldoc.Save(XmlFile);
				result = true;
			}
			catch (XmlException ex)
			{
				throw new XmlException(ex.Message);
			}
			return result;
		}

		public bool DeleteXmlNodeByXPath(string xmlFileName, string xpath)
		{
			bool result = false;
			XmlHelper.xmldoc = new XmlDocument();
			try
			{
				XmlHelper.xmldoc.Load(xmlFileName);
				XmlNode xmlNode = XmlHelper.xmldoc.SelectSingleNode(xpath);
				if (xmlNode != null)
				{
					XmlHelper.xmldoc.ParentNode.RemoveChild(xmlNode);
				}
				XmlHelper.xmldoc.Save(xmlFileName);
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public bool DeleteXmlAttributeByXPath(string xmlFileName, string xpath, string xmlAttributeName)
		{
			bool result = false;
			bool flag = false;
			XmlHelper.xmldoc = new XmlDocument();
			try
			{
				XmlHelper.xmldoc.Load(xmlFileName);
				XmlNode xmlNode = XmlHelper.xmldoc.SelectSingleNode(xpath);
				XmlAttribute node = null;
				if (xmlNode != null)
				{
					foreach (XmlAttribute xmlAttribute in xmlNode.Attributes)
					{
						if (xmlAttribute.Name.ToLower() == xmlAttributeName.ToLower())
						{
							node = xmlAttribute;
							flag = true;
							break;
						}
					}
					if (flag)
					{
						xmlNode.Attributes.Remove(node);
					}
				}
				XmlHelper.xmldoc.Save(xmlFileName);
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public bool DeleteAllXmlAttributeByXPath(string xmlFileName, string xpath)
		{
			bool result = false;
			XmlHelper.xmldoc = new XmlDocument();
			try
			{
				XmlHelper.xmldoc.Load(xmlFileName);
				XmlNode xmlNode = XmlHelper.xmldoc.SelectSingleNode(xpath);
				if (xmlNode != null)
				{
					xmlNode.Attributes.RemoveAll();
				}
				XmlHelper.xmldoc.Save(xmlFileName);
				result = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		private void SetAttributes(XmlElement xe, Hashtable htAttribute)
		{
			foreach (DictionaryEntry dictionaryEntry in htAttribute)
			{
				xe.SetAttribute(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
			}
		}

		private void SetNodes(string rootNode, XmlDocument XmlDoc, XmlElement rootXe, Hashtable SubNodes)
		{
			if (SubNodes != null)
			{
				foreach (DictionaryEntry dictionaryEntry in SubNodes)
				{
					XmlHelper.xmlnode = XmlDoc.SelectSingleNode(rootNode);
					XmlElement xmlElement = XmlDoc.CreateElement(dictionaryEntry.Key.ToString());
					xmlElement.InnerText = dictionaryEntry.Value.ToString();
					rootXe.AppendChild(xmlElement);
				}
			}
		}

		private void UpdateNodes(XmlNodeList root, Hashtable htAtt, Hashtable htSubNode)
		{
			foreach (XmlNode xmlNode in root)
			{
				XmlHelper.xmlelem = (XmlElement)xmlNode;
				if (XmlHelper.xmlelem.HasAttributes)
				{
					foreach (DictionaryEntry dictionaryEntry in htAtt)
					{
						if (XmlHelper.xmlelem.HasAttribute(dictionaryEntry.Key.ToString()))
						{
							XmlHelper.xmlelem.SetAttribute(dictionaryEntry.Key.ToString(), dictionaryEntry.Value.ToString());
						}
					}
				}
				if (XmlHelper.xmlelem.HasChildNodes)
				{
					XmlNodeList childNodes = XmlHelper.xmlelem.ChildNodes;
					foreach (XmlNode xmlNode2 in childNodes)
					{
						XmlElement xmlElement = (XmlElement)xmlNode2;
						foreach (DictionaryEntry dictionaryEntry in htSubNode)
						{
							if (xmlElement.Name == dictionaryEntry.Key.ToString())
							{
								xmlElement.InnerText = dictionaryEntry.Value.ToString();
							}
						}
					}
				}
			}
		}

		public static XmlNode GetXmlNodeByXpath(string xmlFileName, string xpath)
		{
			XmlHelper.xmldoc = new XmlDocument();
			XmlNode result;
			try
			{
				XmlHelper.xmldoc.Load(xmlFileName);
				XmlNode xmlNode = XmlHelper.xmldoc.SelectSingleNode(xpath);
				result = xmlNode;
			}
			catch (Exception err)
			{
                Console.WriteLine(err.Message);
				result = null;
			}
			return result;
		}

		public static XmlNodeList GetXmlNodeListByXpath(string xmlFileName, string xpath)
		{
			XmlHelper.xmldoc = new XmlDocument();
			XmlNodeList result;
			try
			{
				XmlHelper.xmldoc.Load(xmlFileName);
				XmlNodeList xmlNodeList = XmlHelper.xmldoc.SelectNodes(xpath);
				result = xmlNodeList;
			}
			catch (Exception err)
			{
                Console.WriteLine(err.Message);
				result = null;
			}
			return result;
		}

		public static XmlAttribute GetXmlAttribute(string xmlFileName, string xpath, string xmlAttributeName)
		{
			string empty = string.Empty;
			XmlHelper.xmldoc = new XmlDocument();
			XmlAttribute result = null;
			try
			{
				XmlHelper.xmldoc.Load(xmlFileName);
				XmlNode xmlNode = XmlHelper.xmldoc.SelectSingleNode(xpath);
				if (xmlNode != null)
				{
					if (xmlNode.Attributes.Count > 0)
					{
						result = xmlNode.Attributes[xmlAttributeName];
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
	}
}
