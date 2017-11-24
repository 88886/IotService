using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace PrintX.LeanMES.Plugin.LabelPrint
{
	public class Printer
	{
		private static PrintDocument fPrintDocument = new PrintDocument();

		public static string DefaultPrinter
		{
			get
			{
				return Printer.fPrintDocument.PrinterSettings.PrinterName;
			}
		}

		public static List<string> GetLocalPrinters()
		{
			List<string> list = new List<string>();
			list.Add(Printer.DefaultPrinter);
			foreach (string item in PrinterSettings.InstalledPrinters)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static bool VerifyPrinter(string printer)
		{
            return  Printer.GetLocalPrinters().Contains(printer);
         
		}
	}
}
