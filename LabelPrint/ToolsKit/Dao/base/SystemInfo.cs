using System;
using System.IO;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class SystemInfo
	{
		public static string AppBasePath
		{
			get
			{
				string text = System.AppDomain.CurrentDomain.BaseDirectory;
				if (text[text.Length - 1] != System.IO.Path.DirectorySeparatorChar)
				{
					text += System.IO.Path.DirectorySeparatorChar;
				}
				return text;
			}
		}
	}
}
