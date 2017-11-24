using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_Item"), CompilerGenerated, Guid("3624B9CB-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
	public interface Documents
	{
		[DispId(7)]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Interface)]
		Document Open([MarshalAs(UnmanagedType.BStr)] [In] string strDocName, [In] bool ReadOnly = false);

		[DispId(9)]
		[PreserveSig]
		void CloseAll([In] bool Save = true);
	}
}
