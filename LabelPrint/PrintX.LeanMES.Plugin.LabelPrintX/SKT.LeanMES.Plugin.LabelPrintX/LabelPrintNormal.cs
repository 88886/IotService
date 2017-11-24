using PrintX.LeanMES.Plugin.LabelPrint;
using PrintX.LeanMES.Plugin.LabelPrintX;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Forms;

namespace PrintX.LeanMES.Plugin.LabelPrintX
{
	[Guid("F843DB2A-40AF-4D16-B695-F87A2DD566C2"), SecuritySafeCritical]
	public class LabelPrintNormal : UserControl, IObjectSafety
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

		private IContainer components = null;

		private bool _fSafeForScripting = true;

		private bool _fSafeForInitializing = true;

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
			base.Name = "LabelPrintNormal";
			base.Size = new Size(10, 10);
			base.ResumeLayout(false);
		}

		public LabelPrintNormal()
		{
			this.InitializeComponent();
		}

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

		private void SendContentToPrinter(string printContent, string printerName, string lang)
		{
			PrintHlper printHlper = new PrintHlper();
			printHlper.SendContentToPrinter(printContent, printerName, lang, typeof(LabelPrintNormal));
		}

		[SecuritySafeCritical]
		public bool CheckPrinter(string printerName)
		{
			return Printer.VerifyPrinter(printerName);
		}

		[SecuritySafeCritical]
		public void PrintLabel(string printContent, string printerName, string lang)
		{
			if (string.IsNullOrEmpty(lang))
			{
				lang = "cn";
			}
			this.SendContentToPrinter(printContent, printerName, lang);
		}
	}
}
