using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_FullName"), CompilerGenerated, Guid("3624B9CC-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
    [ComVisible(true)]
	public interface Printer
	{
		[DispId(13)]
		[PreserveSig]
		bool SwitchTo([MarshalAs(UnmanagedType.BStr)] [In] string strPrinterName, [MarshalAs(UnmanagedType.BStr)] [In] string strPortName = "", [In] bool DirectAccess = false);
	}
}
