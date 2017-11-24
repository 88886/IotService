using System;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class NumberUtils
	{
		public static bool IsInfinityOrNaN(object valueObj)
		{
			bool result;
			if (valueObj.GetType() != typeof(double))
			{
				result = false;
			}
			else
			{
				double d = (double)valueObj;
				result = (double.IsInfinity(d) || double.IsNaN(d));
			}
			return result;
		}

		public static decimal Round(decimal number, int digit)
		{
			decimal result;
			if (digit >= 0)
			{
				result = System.Math.Round(number, digit, System.MidpointRounding.AwayFromZero);
			}
			else
			{
				decimal d = System.Math.Abs(number);
				decimal d2 = System.Convert.ToDecimal(System.Math.Pow(10.0, (double)digit));
				d *= d2;
				result = ((number < 0m) ? -1 : 1) * System.Math.Round(d + 0.00000001m) / d2;
			}
			return result;
		}

		internal static string Format(decimal value, string format, bool displayThousandSeperator)
		{
			string text = value.ToString(format);
			if (!displayThousandSeperator)
			{
				text = text.Replace(",", string.Empty);
			}
			return text;
		}

		public static string Format(object value)
		{
			return NumberUtils.Format(value, 0, 8);
		}

		public static string Format(object value, int decimalDigits, int decimalScale)
		{
			return NumberUtils.Format(value, decimalDigits, decimalScale, false, false);
		}

		public static string Format(object value, int decimalDigits, int decimalScale, bool displayThousandSeperator, bool displayEmptyForZero)
		{
			string result;
			if (value == null)
			{
				result = string.Empty;
			}
			else if (value.GetType() == typeof(string))
			{
				result = (string)value;
			}
			else
			{
				decimal num = System.Convert.ToDecimal(value);
				if (num != 0m)
				{
					if (decimalDigits > 0)
					{
						result = NumberUtils.Format(NumberUtils.Round(num, decimalDigits), "n" + decimalDigits, displayThousandSeperator);
					}
					else if (decimalScale > 0)
					{
						num = NumberUtils.Round(num, decimalScale);
						result = (displayThousandSeperator ? NumberUtils.FormatWithThousandSeperator(num, decimalScale) : NumberUtils.Format(num));
					}
					else if (displayThousandSeperator)
					{
						result = NumberUtils.FormatWithThousandSeperator(num, 8);
					}
					else
					{
						result = NumberUtils.Format(num);
					}
				}
				else
				{
					result = (displayEmptyForZero ? string.Empty : NumberUtils.Format(num));
				}
			}
			return result;
		}

		private static string FormatWithThousandSeperator(decimal value, int digits)
		{
			string str = value.ToString("n" + digits);
			return NumberUtils.FormatWithThousandSeperatorExtracted(str);
		}

		private static string FormatWithThousandSeperatorExtracted(string str)
		{
			int num = str.Length - 1;
			for (int i = num; i > 0; i--)
			{
				char c = str[i];
				if (c != '0')
				{
					if (i != num)
					{
						if (c != '.')
						{
							i++;
						}
						str = str.Substring(0, i + 1);
					}
					break;
				}
			}
			return str;
		}

		private static string Format(decimal value)
		{
			string format = "0." + new string('#', 8);
			return value.ToString(format);
		}

		internal static void Test()
		{
			decimal number = 0.245m;
			System.Console.WriteLine(NumberUtils.Round(number, 2));
		}

		public static int? ParseInt(string str)
		{
			int? result;
			if (str == null)
			{
				result = null;
			}
			else
			{
				str = str.Trim();
				if (string.IsNullOrEmpty(str))
				{
					result = null;
				}
				else
				{
					string text = string.Empty;
					string text2 = str;
					for (int i = 0; i < text2.Length; i++)
					{
						char c = text2[i];
						if (char.IsDigit(c))
						{
							text += c;
						}
					}
					int value;
					if (int.TryParse(text, out value))
					{
						result = new int?(value);
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		public static string GetUpperCase(object value, int decimalDigits, NumberUpperCaseKind upperCaseKind)
		{
			string result;
			decimal num;
			if (value == null || value.ToString().Trim() == "" || (value.GetType() == typeof(double) && (double.IsNaN((double)value) || double.IsInfinity((double)value))))
			{
				result = string.Empty;
			}
			else if (!decimal.TryParse(value.ToString(), out num))
			{
				result = value.ToString();
			}
			else
			{
				if (decimalDigits > 0)
				{
					num = NumberUtils.Round(num, decimalDigits);
				}
				string str = string.Empty;
				if (num < 0m)
				{
					str = "负";
				}
				string numberBits = NumberUtils.GetNumberBits(System.Math.Abs(num), 0);
				if (numberBits.Length > 12 || numberBits == "E")
				{
					result = "面值太大,不能转换";
				}
				else
				{
					string numberBits2 = NumberUtils.GetNumberBits(System.Math.Abs(num), 1);
					int length = numberBits.Length;
					string text = string.Empty;
					string[] array = new string[]
					{
						"零",
						"壹",
						"贰",
						"叁",
						"肆",
						"伍",
						"陆",
						"柒",
						"捌",
						"玖"
					};
					string[] array2 = new string[]
					{
						"",
						"",
						"拾",
						"佰",
						"仟",
						"万",
						"拾",
						"佰",
						"仟",
						"亿",
						"拾",
						"佰",
						"仟",
						"万"
					};
					string[] array3 = new string[]
					{
						"圆",
						"角",
						"分"
					};
					for (int i = 1; i <= length; i++)
					{
						string s = numberBits.Substring(length - i, 1);
						text = array[int.Parse(s)] + array2[i] + text;
					}
					text = text.Replace("拾零", "拾");
					text = text.Replace("零拾", "零");
					text = text.Replace("零佰", "零");
					text = text.Replace("零仟", "零");
					text = text.Replace("零万", "零");
					for (int i = 1; i <= 6; i++)
					{
						text = text.Replace("零零", "零");
					}
					text = text.Replace("零万", "零");
					text = text.Replace("零亿", "零");
					text = text.Replace("零零", "零");
					if (text.Length > 1)
					{
						if (text.Substring(text.Length - 1, 1) == "零")
						{
							text = text.Remove(text.Length - 1, 1);
						}
					}
					string str2 = "";
					if (numberBits2 != "0")
					{
						str2 = NumberUtils.GetDecimalUpperDisplay(numberBits2, array, upperCaseKind);
					}
					result = str + text + str2;
				}
			}
			return result;
		}

		private static string GetNumberBits(object value, int bit)
		{
			int num = value.ToString().IndexOf("E+");
			string result;
			if (num > 0)
			{
				result = "E";
			}
			else
			{
				char[] separator = new char[]
				{
					'.',
					'\t'
				};
				string[] array = value.ToString().Split(separator);
				if (array.Length - 1 < bit)
				{
					result = "0";
				}
				else
				{
					result = array[bit];
				}
			}
			return result;
		}

		private static string GetDecimalUpperDisplay(string value, string[] upperLow, NumberUpperCaseKind upperCaseKind)
		{
			string text = string.Empty;
			string result;
			if (upperCaseKind == NumberUpperCaseKind.Quantity)
			{
				if (long.Parse(value) == 0L)
				{
					result = "";
					return result;
				}
				text = "点";
				for (int i = 0; i < value.Length; i++)
				{
					text += upperLow[int.Parse(value[i].ToString())];
				}
			}
			else if (upperCaseKind == NumberUpperCaseKind.Money)
			{
				if (int.Parse(value) == 0)
				{
					result = "圆整";
					return result;
				}
				text = "圆";
				if (value.Length < 2)
				{
					text = text + upperLow[int.Parse(value[0].ToString())] + "角";
				}
				if (value.Length >= 2)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						upperLow[int.Parse(value[0].ToString())],
						"角",
						upperLow[int.Parse(value[1].ToString())],
						"分"
					});
				}
			}
			result = text;
			return result;
		}
	}
}
