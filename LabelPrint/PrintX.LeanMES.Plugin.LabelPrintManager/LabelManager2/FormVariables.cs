using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_Item"), CompilerGenerated, Guid("3624B9D1-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
    [ComVisible(true)]
	public interface FormVariables
	{
		[DispId(4)]
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Interface)]
		Free Item([MarshalAs(UnmanagedType.Struct)] [In] object Key);
	}
}
