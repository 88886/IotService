using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_Value"), CompilerGenerated, Guid("3624B9E8-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
	public interface Free
	{
		string Value
		{
			[DispId(2)]
			get;
			[DispId(2)]
			set;
		}
	}
}
