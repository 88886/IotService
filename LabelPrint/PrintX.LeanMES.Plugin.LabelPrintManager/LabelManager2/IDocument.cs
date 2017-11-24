using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LabelManager2
{
	[DefaultMember("_Name"), CompilerGenerated, Guid("3624B9C6-9E5D-11D3-A896-00C04F324E22"), InterfaceType(ComInterfaceType.InterfaceIsIDispatch), TypeIdentifier]
	[ComImport]
    [ComVisible(true)]
	public interface IDocument
	{
		Variables Variables
		{
			[DispId(1)]
			get;
			[DispId(1)]
			set;
		}

		Printer Printer
		{
			[DispId(4)]
			get;
			[DispId(4)]
			set;
		}

		[DispId(19)]
		[PreserveSig]
		short PrintDocument([In] int Quantity = 1);

		[DispId(25)]
		[PreserveSig]
		short FormFeed();

		[DispId(26)]
		[PreserveSig]
		void Close([In] bool Save = true);
	}
}
