using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_Name"), CompilerGenerated, Guid("3624B9C3-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
    [ComVisible(true)]
	public interface IApplication
	{
		Documents Documents
		{
			[DispId(3)]
			get;
			[DispId(3)]
			set;
		}

		Document ActiveDocument
		{
			[DispId(18)]
			get;
			[DispId(18)]
			set;
		}

		[DispId(23)]
		[PreserveSig]
		void Quit();
	}
}
