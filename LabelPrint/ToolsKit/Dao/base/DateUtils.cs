using System;
using System.Globalization;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class DateUtils
	{
		public static int DateYearMinValue = 1900;

		public static string DateToStr(object date)
		{
			return DateUtils.DateToStr(date, null);
		}

		public static string DateToStr(object date, string defaultValue)
		{
			string result;
			if (date == null)
			{
				result = defaultValue;
			}
			else if (date.GetType() == typeof(string))
			{
				result = (string)date;
			}
			else
			{
				if (!(date is System.DateTime))
				{
					throw new System.ArgumentOutOfRangeException("date");
				}
				result = DateUtils.DateToStr(new System.DateTime?((System.DateTime)date));
			}
			return result;
		}

		public static string DateToStr(System.DateTime? date)
		{
			return date.HasValue ? date.Value.ToString("yyyy-MM-dd") : null;
		}

		public static string DateTimeToStr(object date)
		{
			string result;
			if (date == null)
			{
				result = null;
			}
			else
			{
				if (!(date is System.DateTime))
				{
					throw new System.ArgumentOutOfRangeException("date");
				}
				result = DateUtils.DateTimeToStr(new System.DateTime?((System.DateTime)date));
			}
			return result;
		}

		public static string DateTimeToStr(System.DateTime? date)
		{
			return date.HasValue ? date.Value.ToString("yyyy-MM-dd HH:mm:ss") : null;
		}

		public static string TimeToStr(System.DateTime? date)
		{
			return date.HasValue ? date.Value.ToString("HH:mm:ss") : null;
		}

		public static System.DateTime? StrToDate(string value)
		{
			System.DateTime value2;
			System.DateTime? result;
			if (System.DateTime.TryParseExact(value, new string[]
			{
				"yyyy-MM-dd",
				"yyyyMMdd"
			}, null, System.Globalization.DateTimeStyles.None, out value2))
			{
				result = new System.DateTime?(value2);
			}
			else
			{
				if (!string.IsNullOrEmpty(value))
				{
					int num = value.IndexOf(' ');
					if (num > 0)
					{
						value = value.Substring(0, num);
					}
					System.DateTime today = System.DateTime.Today;
					string[] array = value.Split(new char[]
					{
						'-'
					});
					if (array.Length == 1)
					{
						int day;
						if (DateUtils.TryParseDay(value, out day))
						{
							result = DateUtils.NewDateTime(today.Year, today.Month, day);
							return result;
						}
					}
					else if (array.Length == 2)
					{
						int day;
						int month;
						if (DateUtils.TryParseMonth(array[0], out month) && DateUtils.TryParseDay(array[1], out day))
						{
							result = DateUtils.NewDateTime(today.Year, month, day);
							return result;
						}
					}
					else if (array.Length == 3)
					{
						int day;
						int month;
						int year;
						if (DateUtils.TryParseYear(array[0], out year) && DateUtils.TryParseMonth(array[1], out month) && DateUtils.TryParseDay(array[2], out day))
						{
							result = DateUtils.NewDateTime(year, month, day);
							return result;
						}
					}
				}
				result = null;
			}
			return result;
		}

		private static System.DateTime? NewDateTime(int year, int month, int day)
		{
			int num = System.DateTime.DaysInMonth(year, month);
			System.DateTime? result;
			if (day > num)
			{
				result = null;
			}
			else
			{
				result = new System.DateTime?(new System.DateTime(year, month, day));
			}
			return result;
		}

		private static bool TryParseYear(string str, out int year)
		{
			return int.TryParse(str, out year) && year >= DateUtils.DateYearMinValue && year <= 9999;
		}

		private static bool TryParseMonth(string str, out int month)
		{
			return int.TryParse(str, out month) && month >= 1 && month <= 12;
		}

		private static bool TryParseDay(string str, out int day)
		{
			return int.TryParse(str, out day) && day >= 1 && day <= 31;
		}

		public static int GetDaysBetween(System.DateTime date1, System.DateTime date2)
		{
			return (int)(date1 - date2).TotalDays;
		}
	}
}
