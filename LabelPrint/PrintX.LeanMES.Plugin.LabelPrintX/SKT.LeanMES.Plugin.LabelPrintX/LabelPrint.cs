using PrintX.LeanMES.Plugin.LabelPrint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;


namespace PrintX.LeanMES.Plugin.LabelPrintX
{
	[Guid("8542E25D-C6AE-47A9-8986-40ADBC2EAF7D"), SecuritySafeCritical]
   
	public class LabelPrint : UserControl, IObjectSafety
	{
		private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";

		private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";

		private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";

		private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";

		private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

		private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 1;

		private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 2;

		private const int S_OK = 0;

		private const int E_FAIL = -2147467259;

		private const int E_NOINTERFACE = -2147467262;

		private bool _fSafeForScripting = true;

		private bool _fSafeForInitializing = true;

		private IContainer components = null;

		public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
		{
			string text = riid.ToString("B");
			pdwSupportedOptions = 3;
			string text2 = text;
			int result;
			if (text2 != null)
			{
				if (text2 == "{00020400-0000-0000-C000-000000000046}" || text2 == "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}")
				{
					result = 0;
					pdwEnabledOptions = 0;
					if (this._fSafeForScripting)
					{
						pdwEnabledOptions = 1;
					}
					return result;
				}
				if (text2 == "{0000010A-0000-0000-C000-000000000046}" || text2 == "{00000109-0000-0000-C000-000000000046}" || text2 == "{37D84F60-42CB-11CE-8135-00AA004BB851}")
				{
					result = 0;
					pdwEnabledOptions = 0;
					if (this._fSafeForInitializing)
					{
						pdwEnabledOptions = 2;
					}
					return result;
				}
			}
			result = -2147467262;
			return result;
		}

		public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
		{
			int result = -2147467259;
			string text = riid.ToString("B");
			string text2 = text;
			if (text2 != null)
			{
				if (text2 == "{00020400-0000-0000-C000-000000000046}" || text2 == "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}")
				{
					if ((dwEnabledOptions & dwOptionSetMask) == 1 && this._fSafeForScripting)
					{
						result = 0;
					}
					return result;
				}
				if (text2 == "{0000010A-0000-0000-C000-000000000046}" || text2 == "{00000109-0000-0000-C000-000000000046}" || text2 == "{37D84F60-42CB-11CE-8135-00AA004BB851}")
				{
					if ((dwEnabledOptions & dwOptionSetMask) == 2 && this._fSafeForInitializing)
					{
						result = 0;
					}
					return result;
				}
			}
			result = -2147467262;
			return result;
		}

		public LabelPrint()
		{
			this.InitializeComponent();
		}

		[SecuritySafeCritical]
		public void PrintLabel(string labelFilePath, string labelValue, string lang, string printerName)
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = "cn";
			}
			this.PrintLabelUseCodeSoft(labelFilePath, labelValue, 1, lang, printerName);
		}

		[SecuritySafeCritical]
		public void PrintLabelWithLab(string labelFilePath, string labelValue, string lang, string printerName)
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = "cn";
			}
			this.PrintLabelUseCodeSoft(labelFilePath, labelValue, 1, lang, printerName);
		}

		[SecuritySafeCritical]
		public void PrintMultipleLabel(string labelFilePath, string labelValue, int copies, string lang, string printerName)
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = "cn";
			}
			this.PrintLabelUseCodeSoft(labelFilePath, labelValue, copies, lang, printerName);
		}

		[SecuritySafeCritical]
		public void PrintMultipleLabelWithLab(string labelFilePath, string labelValue, int copies, string lang, string printerName)
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = "cn";
			}
			this.PrintLabelUseCodeSoft(labelFilePath, labelValue, copies, lang, printerName);
		}

		private void PrintLabelUseCodeSoft(string labelFilePath, string labelValue, int copies,
            string lang, string printerName)
		{
			PrintHlper printHlper = new PrintHlper();
			printHlper.PrintLabelUseCodeSoft(labelFilePath, labelValue, copies, lang, typeof(LabelPrint), printerName);
		}


        public void PrintLabelUseCodeSoft9(string labelFilePath, string labelValue, int copies,
        string lang, string printerName, ref int errorCode, ref String errorMessage, int linkFLag)
        {
            PrintHlper printHlper = null;
            try
            {
                 printHlper = new PrintHlper();

            }catch(Exception err)
            {
                throw new Exception("codesoft安装不正确---"+ err.Message);
            }
            printHlper.PrintLabelUseCodeSoft9(labelFilePath, labelValue, copies, lang, 
                typeof(LabelPrint), printerName,
                ref  errorCode,ref  errorMessage, linkFLag);
        }



		private void SendContentToPrinter(string printContent, string printerName, string lang)
		{
			PrintHlper printHlper = new PrintHlper();
			printHlper.SendContentToPrinter(printContent, printerName, lang, typeof(LabelPrintNormal));
		}

		[SecuritySafeCritical]
		public bool CheckPrinter(string printerName)
		{
			bool result = false;
			foreach (string text in PrinterSettings.InstalledPrinters)
			{
				if (text.ToLower() == printerName.ToLower())
				{
					result = false;
					break;
				}
			}
			return result;
		}

		[SecuritySafeCritical]
		public void PrintLabelWithZpl(string printContent, string printerName, string lang)
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = "cn";
			}
			this.SendContentToPrinter(printContent, printerName, lang);
		}

		[SecuritySafeCritical]
		public string GetLocalPrintersList()
		{
			List<string> localPrinters = Printer.GetLocalPrinters();
			string text = "";
			for (int i = 0; i < localPrinters.Count; i++)
			{
				text = text + localPrinters[i] + ";";
			}
			return text;
		}

		[SecuritySafeCritical]
		public void KillProcess(string ProcessName)
		{
			try
			{
				Process[] processesByName = Process.GetProcessesByName(ProcessName);
				for (int i = 0; i < processesByName.Length; i++)
				{
					Process process = processesByName[i];
					if (!process.CloseMainWindow())
					{
						process.Kill();
					}
				}
			}
			catch
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 12f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = Color.Transparent;
			base.Name = "LabelPrint";
			base.Size = new Size(10, 10);
			base.ResumeLayout(false);
		}


	}
}
