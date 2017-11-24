using System;
using System.Collections.Generic;

namespace PrintX.LeanMES.Plugin.LabelPrint
{
	public class LabelData
	{
		private List<LabelInfo> labelContent;

		public List<LabelInfo> LabelContent
		{
			get
			{
				return this.labelContent;
			}
			set
			{
				this.labelContent = value;
			}
		}
	}
}
