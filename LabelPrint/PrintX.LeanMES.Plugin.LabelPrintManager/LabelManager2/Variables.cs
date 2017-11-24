using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_Item"), CompilerGenerated, Guid("3624B9CF-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
    [ComVisible(true)]
	public interface Variables
	{
		FormVariables FormVariables
		{
			[DispId(2)]
			get;
			[DispId(2)]
			set;
		}
	}
}
