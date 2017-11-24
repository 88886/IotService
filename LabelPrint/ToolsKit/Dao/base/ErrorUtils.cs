using System;

namespace PrintX.Dev.Utils.ToolsKit
{
	internal static class ErrorUtils
	{
		internal static string FormatString(string unformatted, params object[] args)
		{
			string result = unformatted;
			if (args != null && args.Length > 0)
			{
				result = string.Format(unformatted, args);
			}
			return result;
		}

		internal static void VerifyThrowArgumentLength(string parameter, string parameterName)
		{
			ErrorUtils.VerifyThrowArgumentNull(parameter, parameterName);
			if (parameter.Length == 0)
			{
				throw new System.ArgumentException(ErrorUtils.FormatString("参数“{0}”长度不能为0。", new object[]
				{
					parameterName
				}));
			}
		}

		internal static void VerifyThrowArgumentNull(object parameter, string parameterName)
		{
			if (parameter == null)
			{
				throw new System.ArgumentNullException(parameterName);
			}
		}
	}
}
