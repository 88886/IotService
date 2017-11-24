using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
	public static class StringUtils
	{
		private static readonly char[] cLowChineseChars = new char[]
		{
			'啊',
			'芭',
			'擦',
			'搭',
			'蛾',
			'发',
			'噶',
			'哈',
			'击',
			'喀',
			'垃',
			'妈',
			'拿',
			'欧',
			'啪',
			'期',
			'然',
			'撒',
			'塌',
			'挖',
			'昔',
			'压',
			'匝'
		};

		private static readonly char[] cLowPinyins = new char[]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'w',
			'x',
			'y',
			'z'
		};

		private static readonly string[] cHighPinyins = new string[]
		{
			"cjwgnspgcgnesypbtyyzdxykygtdjnmjqmbsgzscyjsyyzpgkbzgycywykgkljswkpjqhyzwddzlsgmrypywwcckznkydg",
			"ttnjjeykkzytcjnmcylqlypyqfqrpzslwbtgkjfyxjwzltbncxjjjjzxdttsqzycdxxhgckbphffssyybgmxlpbylllhlx",
			"spzmyjhsojnghdzqyklgjhxgqzhxqgkezzwyscscjxyeyxadzpmdssmzjzqjyzcdjewqjbdzbxgznzcpwhkxhqkmwfbpby",
			"dtjzzkqhylygxfptyjyyzpszlfchmqshgmxxsxjjsdcsbbqbefsjyhxwgzkpylqbgldlcctnmayddkssngycsgxlyzaybn",
			"ptsdkdylhgymylcxpycjndqjwxqxfyyfjlejbzrxccqwqqsbzkymgplbmjrqcflnymyqmsqyrbcjthztqfrxqhxmjjcjlx",
			"qgjmshzkbswyemyltxfsydsglycjqxsjnqbsctyhbftdcyzdjwyghqfrxwckqkxebptlpxjzsrmebwhjlbjslyysmdxlcl",
			"qkxlhxjrzjmfqhxhwywsbhtrxxglhqhfnmcykldyxzpwlggsmtcfpajjzyljtyanjgbjplqgdzyqyaxbkysecjsznslyzh",
			"zxlzcghpxzhznytdsbcjkdlzayfmydlebbgqyzkxgldndnyskjshdlyxbcghxypkdqmmzngmmclgwzszxzjfznmlzzthcs",
			"ydbdllscddnlkjykjsycjlkohqasdknhcsganhdaashtcplcpqybsdmpjlpcjoqlcdhjjysprchnknnlhlyyqyhwzptczg",
			"wwmzffjqqqqyxaclbhkdjxdgmmydjxzllsygxgkjrywzwyclzmssjzldbydcpcxyhlxchyzjqsqqagmnyxpfrkssbjlyxy",
			"syglnscmhcwwmnzjjlxxhchsyd ctxrycyxbyhcsmxjsznpwgpxxtaybgajcxlysdccwzocwkccsbnhcpdyznfcyytyckx",
			"kybsqkkytqqxfcwchcykelzqbsqyjqcclmthsywhmktlkjlycxwheqqhtqhzpqsqscfymmdmgbwhwlgsllysdlmlxpthmj",
			"hwljzyhzjxhtxjlhxrswlwzjcbxmhzqxsdzpmgfcsglsxymjshxpjxwmyqksmyplrthbxftpmhyxlchlhlzylxgsssstcl",
			"sldclrpbhzhxyyfhbbgdmycnqqwlqhjjzywjzyejjdhpblqxtqkwhlchqxagtlxljxmslxhtzkzjecxjcjnmfbycsfywyb",
			"jzgnysdzsqyrsljpclpwxsdwejbjcbcnaytwgmpabclyqpclzxsbnmsggfnzjjbzsfzyndxhplqkzczwalsbccjxjyzhwk",
			"ypsgxfzfcdkhjgxdlqfsgdslqwzkxtmhsbgzmjzrglyjbpmlmsxlzjqqhzsjczydjwbmjklddpmjegxyhylxhlqyqhkycw",
			"cjmyyxnatjhyccxzpcqlbzwwytwbqcmlpmyrjcccxfpznzzljplxxyztzlgdldcklyrlzgqtgjhhgjljaxfgfjzslcfdqz",
			"lclgjdjcsnclljpjqdcclcjxmyzftsxgcgsbrzxjqqctzhgyqtjqqlzxjylylbcyamcstylpdjbyregkjzyzhlyszqlznw",
			"czcllwjqjjjkdgjzolbbzppglghtgzxyghzmycnqsycyhbhgxkamtxyxnbskyzzgjzlqjdfcjxdygjqjjpmgwgjjjpkqsb",
			"gbmmcjssclpqpdxcdyykywcjddyygywrhjrtgznyqldkljszzgzqzjgdykshpzmtlcpwnjafyzdjcnmwescyglbtzcgmss",
			"llyxqsxsbsjsbbggghfjlypmzjnlyywdqshzxtyywhmcyhywdbxbtlmsyyyfsxjcsdxxlhjhf sxzqhfzmzcztqcxzxrtt",
			"djhnnyzqqmnqdmmglydxmjgdhcdyzbffallztdltfxmxqzdngwqdbdczjdxbzgsqqddjcmbkzffxmkdmdsyyszcmljdsyn",
			"sprskmkmpcklgdbqtfzswtfgglyplljzhgjjgypzltcsmcnbtjbqfkthbyzgkpbbymtdssxtbnpdkleycjnycdykzddhqh",
			"sdzsctarlltkzlgecllkjlqjaqnbdkkghpjtzqksecshalqfmmgjnlyjbbtmlyzxdcjpldlpcqdhzycbzsczbzmsljflkr",
			"zjsnfrgjhxpdhyjybzgdljcsezgxlblhyxtwmabchecmwyjyzlljjyhlgbdjlslygkdzpzxjyyzlwcxszfgwyydlyhcljs",
			"cmbjhblyzlycblydpdqysxqzbytdkyyjyycnrjmpdjgklcljbctbjddbblblczqrppxjcglzcshltoljnmdddlngkaqhqh",
			"jhykheznmshrp qqjchgmfprxhjgdychghlyrzqlcyqjnzsqtkqjymszswlcfqqqxyfggyptqwlmcrnfkkfsyylqbmqamm",
			"myxctpshcptxxzzsmphpshmclmldqfyqxszyjdjjzzhqpdszglstjbckbxyqzjsgpsxqzqzrqtbdkyxzkhhgflbcsmdldg",
			"dzdblzyycxnncsybzbfglzzxswmsccmqnjqsbdqsjtxxmbltxzclzshzcxrqjgjylxzfjphyxzqqydfqjjlzznzjcdgzyg",
			"ctxmzysctlkphtxhtlbjxjlxscdqxcbbtjfqzfsltjbtkqbxxjjljchczdbzjdczjdcprnpqcjpfczlclzxbdmxmphjsgz",
			"gszzqlylwtjpfsyasmcjbtzyycwmytcsjjlqcqlwzmalbxyfbpnlsfhtgjwejjxxglljstgshjqlzfkcgnndszfdeqfhbs",
			"aqtgylbxmmygszldydqmjjrgbjtkgdhgkblqkbdmbylxwcxyttybkmrtjzxqjbhlmhmjjzmqasldcyxyqdlqcafywyxqhz"
		};

		private static System.Text.Encoding gb2312 = System.Text.Encoding.GetEncoding("GB2312");

		private static readonly object[] cPinyinMap = new object[]
		{
			19969,
			"dz",
			19975,
			"wm",
			19988,
			"qj",
			20048,
			"ly",
			20056,
			"cs",
			20060,
			"mn",
			20094,
			"qg",
			20127,
			"qj",
			20167,
			"cq",
			20193,
			"yg",
			20250,
			"hk",
			20256,
			"cz",
			20282,
			"cs",
			20285,
			"jgq",
			20291,
			"dt",
			20314,
			"yd",
			20340,
			"ne",
			20375,
			"dt",
			20389,
			"jy",
			20391,
			"cz",
			20415,
			"bp",
			20446,
			"ys",
			20447,
			"sq",
			20504,
			"tc",
			20608,
			"kg",
			20854,
			"qj",
			20857,
			"zc",
			20911,
			"fp",
			20504,
			"tc",
			20608,
			"kg",
			20854,
			"qj",
			20857,
			"zc",
			20985,
			"aw",
			21032,
			"pb",
			21048,
			"qx",
			21049,
			"cs",
			21089,
			"ys",
			21119,
			"jc",
			21242,
			"sb",
			21273,
			"sc",
			21305,
			"py",
			21306,
			"qo",
			21330,
			"zc",
			21333,
			"dcs",
			21345,
			"kq",
			21378,
			"ca",
			21397,
			"cs",
			21414,
			"sx",
			21442,
			"cs",
			21477,
			"jg",
			21480,
			"dt",
			21484,
			"zs",
			21494,
			"yx",
			21505,
			"yx",
			21512,
			"hg",
			21523,
			"xh",
			21537,
			"pb",
			21542,
			"fp",
			21549,
			"kh",
			21574,
			"da",
			21588,
			"td",
			21618,
			"zc",
			21621,
			"kha",
			21632,
			"jz",
			21654,
			"gk",
			21679,
			"lkg",
			21683,
			"kh",
			21719,
			"hy",
			21734,
			"woe",
			21780,
			"wn",
			21804,
			"hx",
			21899,
			"dz",
			21903,
			"nr",
			21908,
			"wo",
			21939,
			"zc",
			21956,
			"sa",
			21964,
			"ya",
			21970,
			"td",
			22031,
			"jg",
			22040,
			"xs",
			22060,
			"zc",
			22066,
			"cz",
			22079,
			"hm",
			22129,
			"xj",
			22179,
			"xa",
			22237,
			"nj",
			22244,
			"td",
			22280,
			"qj",
			22300,
			"yh",
			22313,
			"xw",
			22331,
			"yq",
			22343,
			"jy",
			22351,
			"hp",
			22395,
			"dc",
			22412,
			"td",
			22484,
			"pb",
			22500,
			"pb",
			22534,
			"dz",
			22549,
			"dh",
			22561,
			"bp",
			22612,
			"td",
			22771,
			"kq",
			22831,
			"hb",
			22841,
			"jg",
			22855,
			"qj",
			22865,
			"qx",
			23013,
			"lm",
			23081,
			"mw",
			23487,
			"sx",
			23558,
			"jq",
			23561,
			"wy",
			23586,
			"yw",
			23614,
			"wy",
			23615,
			"ns",
			23631,
			"pb",
			23646,
			"sz",
			23663,
			"tz",
			23673,
			"yg",
			23762,
			"dt",
			23769,
			"zs",
			23780,
			"qj",
			23884,
			"qk",
			24055,
			"xh",
			24113,
			"dc",
			24162,
			"cz",
			24191,
			"ga",
			24273,
			"qj",
			24324,
			"nl",
			24377,
			"td",
			24378,
			"qj",
			24439,
			"pf",
			24554,
			"zs",
			24683,
			"td",
			24694,
			"ew",
			24733,
			"lk",
			24925,
			"tn",
			25094,
			"zg",
			25100,
			"xq",
			25103,
			"xh",
			25153,
			"bp",
			25170,
			"bp",
			25179,
			"kg",
			25203,
			"bp",
			25240,
			"zs",
			25282,
			"fb",
			25303,
			"na",
			25324,
			"kg",
			25341,
			"zy",
			25373,
			"wz",
			25375,
			"xj",
			25528,
			"sd",
			25530,
			"cs",
			25552,
			"td",
			25774,
			"cz",
			25874,
			"zc",
			26044,
			"yw",
			26080,
			"wm",
			26292,
			"bp",
			26333,
			"pb",
			26355,
			"zy",
			26366,
			"cz",
			26397,
			"cz",
			26399,
			"qj",
			26415,
			"sz",
			26451,
			"sb",
			26526,
			"cz",
			26552,
			"jg",
			26561,
			"td",
			26588,
			"gj",
			26597,
			"cz",
			26629,
			"zs",
			26638,
			"ly",
			26646,
			"qx",
			26653,
			"kg",
			26657,
			"xj",
			26727,
			"gh",
			26894,
			"zc",
			26937,
			"zs",
			26946,
			"zc",
			26999,
			"kj",
			27099,
			"kj",
			27449,
			"yq",
			27481,
			"xs",
			27542,
			"zs",
			27663,
			"zs",
			27748,
			"ts",
			27784,
			"sc",
			27788,
			"zd",
			27795,
			"td",
			27850,
			"bp",
			27852,
			"mb",
			27895,
			"ls",
			27898,
			"lp",
			27973,
			"qj",
			27981,
			"kh",
			27986,
			"hx",
			27994,
			"xj",
			28044,
			"yc",
			28065,
			"wg",
			28177,
			"sm",
			28267,
			"qj",
			28291,
			"kh",
			28337,
			"zq",
			28463,
			"tl",
			28548,
			"dc",
			28601,
			"td",
			28689,
			"pb",
			28805,
			"jg",
			28820,
			"qg",
			28846,
			"pb",
			28952,
			"td",
			28975,
			"zc",
			29325,
			"qj",
			29575,
			"sl",
			29602,
			"fb",
			30010,
			"td",
			30044,
			"cx",
			30058,
			"pf",
			30091,
			"ysp",
			30111,
			"yn",
			30229,
			"xj",
			30427,
			"sc",
			30465,
			"sx",
			30631,
			"qy",
			30655,
			"qj",
			30684,
			"qjg",
			30707,
			"sd",
			30729,
			"xh",
			30796,
			"lg",
			30917,
			"bp",
			31074,
			"mn",
			31085,
			"jz",
			31109,
			"cs",
			31181,
			"zc",
			31192,
			"mlb",
			31293,
			"jq",
			31400,
			"yx",
			31584,
			"jy",
			31896,
			"zn",
			31909,
			"zy",
			31995,
			"xj",
			32321,
			"fp",
			32327,
			"yz",
			32418,
			"hg",
			32420,
			"xq",
			32421,
			"hg",
			32438,
			"lg",
			32473,
			"gj",
			32488,
			"dt",
			32521,
			"jq",
			32527,
			"pb",
			32562,
			"zsq",
			32564,
			"jz",
			32735,
			"zd",
			32793,
			"pb",
			33071,
			"pf",
			33098,
			"lx",
			33100,
			"ya",
			33152,
			"bp",
			33261,
			"cx",
			33324,
			"bp",
			33333,
			"dt",
			33406,
			"ay",
			33426,
			"mw",
			33432,
			"pb",
			33445,
			"jg",
			33486,
			"zn",
			33493,
			"st",
			33507,
			"jq",
			33540,
			"qj",
			33544,
			"zc",
			33564,
			"qx",
			33617,
			"yt",
			33632,
			"jq",
			33636,
			"hx",
			33637,
			"yx",
			33694,
			"gw",
			33705,
			"fp",
			33728,
			"wy",
			33882,
			"sr",
			34067,
			"mw",
			34074,
			"wy",
			34121,
			"jq",
			34255,
			"cz",
			34259,
			"xl",
			34425,
			"hj",
			34430,
			"xh",
			34485,
			"kh",
			34503,
			"sy",
			34532,
			"gh",
			34552,
			"sx",
			34558,
			"ey",
			34593,
			"lz",
			34660,
			"qy",
			34892,
			"hx",
			34928,
			"sc",
			34999,
			"jq",
			35048,
			"bp",
			35059,
			"sc",
			35098,
			"cz",
			35203,
			"tq",
			35265,
			"jx",
			35299,
			"jx",
			35782,
			"sz",
			35828,
			"sy",
			35843,
			"td",
			35895,
			"gy",
			35977,
			"hm",
			36158,
			"jg",
			36228,
			"qj",
			36426,
			"qx",
			36466,
			"dc",
			36710,
			"cj",
			36711,
			"zyg",
			36767,
			"bp",
			36866,
			"sk",
			36951,
			"yw",
			37034,
			"xy",
			37063,
			"hx",
			37218,
			"cz",
			37325,
			"zc",
			38063,
			"bp",
			38079,
			"dt",
			38085,
			"qy",
			38107,
			"dc",
			38116,
			"td",
			38123,
			"yd",
			38224,
			"hg",
			38241,
			"xtc",
			38271,
			"cz",
			38415,
			"ye",
			38426,
			"kh",
			38461,
			"yd",
			38463,
			"ae",
			38466,
			"pb",
			38477,
			"jx",
			38518,
			"ty",
			38551,
			"wk",
			38585,
			"zc",
			38704,
			"xs",
			38739,
			"lj",
			38761,
			"gj",
			38808,
			"qs",
			39048,
			"jg",
			39049,
			"jx",
			39052,
			"hg",
			39076,
			"cz",
			39271,
			"xt",
			39534,
			"td",
			39552,
			"td",
			39584,
			"bp",
			39647,
			"sb",
			39730,
			"lg",
			39748,
			"pbt",
			40109,
			"zq",
			40479,
			"nd",
			40516,
			"hg",
			40536,
			"hg",
			40583,
			"qj",
			40765,
			"yq",
			40784,
			"qj",
			40840,
			"yk",
			40863,
			"gjq"
		};

		private static System.Collections.Hashtable pinyinMap;

		public static string GetHashCode(string str)
		{
			if (str == null)
			{
				throw new System.ArgumentNullException("str");
			}
			return str.GetHashCode().ToString("x", System.Globalization.CultureInfo.InvariantCulture);
		}

		public static bool EqualsIgnoreCase(string s1, string s2)
		{
			return string.Equals(s1, s2, System.StringComparison.OrdinalIgnoreCase);
		}

		public static bool EqualsIgnoreCase(string s1, int index1, string s2, int index2, int length)
		{
			return string.Compare(s1, index1, s2, index2, length, System.StringComparison.OrdinalIgnoreCase) == 0;
		}

		public static bool StartsWithIgnoreCase(string s1, string s2)
		{
			return s1.StartsWith(s2, System.StringComparison.OrdinalIgnoreCase);
		}

		public static bool EndsWithIgnoreCase(string s1, string s2)
		{
			return s1.EndsWith(s2, System.StringComparison.OrdinalIgnoreCase);
		}

		public static string Repeat(string str, int times)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(str.Length * times);
			for (int i = 0; i < times; i++)
			{
				stringBuilder.Append(str);
			}
			return stringBuilder.ToString();
		}

		public static string ReplaceFirst(string template, string placeholder, string replacement, System.StringComparison comparisonType)
		{
			string result;
			if (template == null)
			{
				result = null;
			}
			else
			{
				int num = template.IndexOf(placeholder, comparisonType);
				if (num < 0)
				{
					result = template;
				}
				else
				{
					result = new System.Text.StringBuilder(template.Substring(0, num)).Append(replacement).Append(template.Substring(num + placeholder.Length)).ToString();
				}
			}
			return result;
		}

		public static string Replace(string template, string placeholder, string replacement, System.StringComparison comparisonType)
		{
			string result;
			if (template == null)
			{
				result = null;
			}
			else
			{
				int num = template.IndexOf(placeholder, comparisonType);
				if (num < 0)
				{
					result = template;
				}
				else
				{
					result = new System.Text.StringBuilder(template.Substring(0, num)).Append(replacement).Append(StringUtils.Replace(template.Substring(num + placeholder.Length), placeholder, replacement, comparisonType)).ToString();
				}
			}
			return result;
		}

		public static string ByteArrayToHex(byte[] bytes)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < bytes.Length; i++)
			{
				stringBuilder.Append(bytes[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		public static byte[] HexToByteArray(string hex)
		{
			int num = hex.Length / 2;
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = byte.Parse(hex.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
			}
			return array;
		}

		public static int GetByteLength(string str)
		{
			int result;
			if (str == null)
			{
				result = 0;
			}
			else
			{
				int num = 0;
				for (int i = 0; i < str.Length; i++)
				{
					char c = str[i];
					num += ((c < '\u0080') ? 1 : 2);
				}
				result = num;
			}
			return result;
		}

		public static string SubstringByByte(string str, int len)
		{
			int num;
			return StringUtils.SubstringByByte(str, len, out num);
		}

		public static string SubstringByByte(string str, int len, out int newLen)
		{
			string text = string.Empty;
			newLen = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				int num = (c < '\u0080') ? 1 : 2;
				if (newLen + num > len)
				{
					break;
				}
				text += c;
				newLen += num;
			}
			return text;
		}

		public static string SubstringByByte(string str, int index, int length)
		{
			string text = string.Empty;
			int num = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				int num2 = (c < '\u0080') ? 1 : 2;
				if (num >= index && num < index + length)
				{
					text += c;
				}
				num += num2;
			}
			return text;
		}

		public static string MD5(string str)
		{
			string result;
			if (string.IsNullOrEmpty(str))
			{
				result = string.Empty;
			}
			else
			{
				System.Security.Cryptography.MD5 mD = System.Security.Cryptography.MD5.Create();
				byte[] bytes = mD.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
				result = StringUtils.ByteArrayToHex(bytes);
			}
			return result;
		}

		public static byte[] EncryptToByteArray(string str)
		{
			byte[] bytes = new System.Text.UTF8Encoding().GetBytes(str);
			System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
			byte[] result;
			try
			{
				System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, new System.Security.Cryptography.TripleDESCryptoServiceProvider().CreateEncryptor(StringUtils.Key(), StringUtils.Iv()), System.Security.Cryptography.CryptoStreamMode.Write);
				try
				{
					cryptoStream.Write(bytes, 0, bytes.Length);
					cryptoStream.FlushFinalBlock();
					result = memoryStream.ToArray();
				}
				finally
				{
					cryptoStream.Dispose();
				}
			}
			finally
			{
				memoryStream.Dispose();
			}
			return result;
		}

		public static string Encrypt(string str)
		{
			return string.IsNullOrEmpty(str) ? string.Empty : StringUtils.ByteArrayToHex(StringUtils.EncryptToByteArray(str));
		}

		public static string DecryptFromByteArray(byte[] data)
		{
			System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(data);
			string @string;
			try
			{
				System.Security.Cryptography.CryptoStream cryptoStream = new System.Security.Cryptography.CryptoStream(memoryStream, new System.Security.Cryptography.TripleDESCryptoServiceProvider().CreateDecryptor(StringUtils.Key(), StringUtils.Iv()), System.Security.Cryptography.CryptoStreamMode.Read);
				try
				{
					byte[] array = new byte[data.Length];
					int count = cryptoStream.Read(array, 0, array.Length);
					@string = new System.Text.UTF8Encoding().GetString(array, 0, count);
				}
				finally
				{
					cryptoStream.Dispose();
				}
			}
			finally
			{
				memoryStream.Dispose();
			}
			return @string;
		}

		public static string Decrypt(string hex)
		{
			return string.IsNullOrEmpty(hex) ? string.Empty : StringUtils.DecryptFromByteArray(StringUtils.HexToByteArray(hex));
		}

		private static byte[] Key()
		{
			return new byte[]
			{
				123,
				97,
				177,
				109,
				240,
				131,
				164,
				237,
				87,
				108,
				118,
				214,
				230,
				181,
				23,
				246,
				80,
				103,
				217,
				52,
				37,
				149,
				47,
				208
			};
		}

		private static byte[] Iv()
		{
			return new byte[]
			{
				96,
				55,
				83,
				220,
				97,
				81,
				172,
				170
			};
		}

		public static string[] TrimAll(string[] array)
		{
			string[] array2 = new string[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i].Trim();
			}
			return array2;
		}

		public static string GetPinyinCode(string str)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				char c2 = c;
				char? c3 = null;
				if (c >= '！' && c <= '～')
				{
					c2 = (char)(c - 'ﻠ');
				}
				else if (c > '\u007f')
				{
					c3 = StringUtils.GetCharPinyinCode(c);
				}
				char? c4 = c3;
				if ((c4.HasValue ? new int?((int)c4.GetValueOrDefault()) : null).HasValue)
				{
					stringBuilder.Append(c3);
				}
				else if ((c2 >= '0' && c2 <= '9') || (c2 >= 'A' && c2 <= 'Z') || (c2 >= 'a' && c2 <= 'z'))
				{
					stringBuilder.Append(char.ToLower(c2));
				}
			}
			return stringBuilder.ToString();
		}

		private static int CompareChineseChar(byte[] bytes1, char char2)
		{
			byte[] bytes2 = StringUtils.gb2312.GetBytes(new char[]
			{
				char2
			});
			int result;
			if (bytes1[0] > bytes2[0])
			{
				result = 1;
			}
			else if (bytes1[0] == bytes2[0])
			{
				result = (int)(bytes1[1] - bytes2[1]);
			}
			else
			{
				result = -1;
			}
			return result;
		}

		private static char GetHighPinyins(byte[] bytes)
		{
			return StringUtils.cHighPinyins[(int)(bytes[0] - 216)][(int)(bytes[1] - 160 - 1)];
		}

		private static void WriteToFile()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			stringBuilder2.Append('{');
			int num = 0;
			for (int i = 0; i < StringUtils.cPinyinMap.Length / 2; i++)
			{
				int num2 = (int)StringUtils.cPinyinMap[i * 2];
				char c = (char)num2;
				string text = (string)StringUtils.cPinyinMap[i * 2 + 1];
				if (i > 0)
				{
					stringBuilder2.Append(',');
				}
				stringBuilder2.Append('"').Append(num2).Append("\":\"").Append(text).Append('"');
				stringBuilder.Append(c).Append(' ').Append(num2).Append(' ').Append(text);
				char? singleCharPinyinCode = StringUtils.GetSingleCharPinyinCode(c);
				if (singleCharPinyinCode.HasValue && text[0] != singleCharPinyinCode.Value)
				{
					stringBuilder.Append(' ').Append(singleCharPinyinCode);
					num++;
				}
				if (text.Length == 1 && text[0] == singleCharPinyinCode.Value)
				{
					stringBuilder.Append(" <-");
				}
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine(num + " 个多音字第一音与字典中的不同");
			stringBuilder2.AppendLine("};");
			System.IO.File.WriteAllText("E:\\\\Carpa.NET\\docs\\ref\\多音字表.txt", stringBuilder.ToString() + stringBuilder2.ToString(), System.Text.Encoding.Default);
		}

		private static char? GetCharPinyinCode(char chineseChar)
		{
			if (StringUtils.pinyinMap == null)
			{
				StringUtils.pinyinMap = new System.Collections.Hashtable();
				for (int i = 0; i < StringUtils.cPinyinMap.Length / 2; i++)
				{
					StringUtils.pinyinMap[StringUtils.cPinyinMap[i * 2]] = StringUtils.cPinyinMap[i * 2 + 1];
				}
			}
			string text = (string)StringUtils.pinyinMap[(int)chineseChar];
			char? result;
			if (text != null)
			{
				result = new char?(text[0]);
			}
			else
			{
				result = StringUtils.GetSingleCharPinyinCode(chineseChar);
			}
			return result;
		}

		private static char? GetSingleCharPinyinCode(char chineseChar)
		{
			byte[] bytes = StringUtils.gb2312.GetBytes(new char[]
			{
				chineseChar
			});
			char? result;
			if (bytes[0] < 216)
			{
				for (int i = StringUtils.cLowChineseChars.Length - 1; i >= 0; i--)
				{
					char @char = StringUtils.cLowChineseChars[i];
					if (StringUtils.CompareChineseChar(bytes, @char) >= 0)
					{
						result = new char?(StringUtils.cLowPinyins[i]);
						return result;
					}
				}
				result = null;
			}
			else
			{
				result = new char?(StringUtils.GetHighPinyins(bytes));
			}
			return result;
		}

		public static string[] SplitByByte(string str, int len)
		{
			string[] result;
			if (string.IsNullOrEmpty(str))
			{
				result = new string[]
				{
					string.Empty
				};
			}
			else
			{
				System.Collections.Generic.IList<System.Text.StringBuilder> list = new System.Collections.Generic.List<System.Text.StringBuilder>();
				System.Text.StringBuilder stringBuilder = null;
				int num = 0;
				for (int i = 0; i < str.Length; i++)
				{
					char c = str[i];
					int num2 = (c < '\u0080') ? 1 : 2;
					if (num + num2 > len)
					{
						stringBuilder = null;
						num = 0;
					}
					if (stringBuilder == null)
					{
						stringBuilder = new System.Text.StringBuilder();
						list.Add(stringBuilder);
					}
					stringBuilder.Append(c);
					num += num2;
				}
				string[] array = new string[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					array[i] = list[i].ToString();
				}
				result = array;
			}
			return result;
		}

		public static string SimpleEncrypt(string str, string key)
		{
			string result;
			if (string.IsNullOrEmpty(str))
			{
				result = string.Empty;
			}
			else
			{
				int num;
				int num2;
				int num3;
				StringUtils.GetSimpleEncryptKey(key, out num, out num2, out num3);
				int num4 = new System.Random().Next(100);
				num += num4;
				num2 += num4;
				num3 += num4;
				string text = num4.ToString("x2");
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
				byte[] array = bytes;
				for (int i = 0; i < array.Length; i++)
				{
					byte b = array[i];
					byte b2 = (byte)((int)b ^ num >> 8);
					text += b2.ToString("x2");
					num = ((int)b2 + num) * num2 + num3;
				}
				result = text;
			}
			return result;
		}

		public static string SimpleDecrypt(string str, string key)
		{
			string result;
			if (string.IsNullOrEmpty(str))
			{
				result = string.Empty;
			}
			else
			{
				int num;
				int num2;
				int num3;
				StringUtils.GetSimpleEncryptKey(key, out num, out num2, out num3);
				int num4;
				if (!int.TryParse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out num4))
				{
					throw new System.InvalidOperationException("密文错误");
				}
				num += num4;
				num2 += num4;
				num3 += num4;
				int num5 = str.Length / 2 - 1;
				byte[] array = new byte[num5];
				for (int i = 0; i < num5; i++)
				{
					byte b = byte.Parse(str.Substring(i * 2 + 2, 2), System.Globalization.NumberStyles.HexNumber);
					byte b2 = (byte)((int)b ^ num >> 8);
					array[i] = b2;
					num = ((int)b + num) * num2 + num3;
				}
				result = System.Text.Encoding.UTF8.GetString(array, 0, array.Length);
			}
			return result;
		}

		private static void GetSimpleEncryptKey(string key, out int startKey, out int multKey, out int addKey)
		{
			if (string.IsNullOrEmpty(key) || key.Length != 6)
			{
				throw new System.InvalidOperationException("key 必须是6位数字");
			}
			startKey = int.Parse(key.Substring(0, 2));
			multKey = int.Parse(key.Substring(2, 2));
			addKey = int.Parse(key.Substring(4, 2));
		}

		public static bool IsEmpty(this string target)
		{
			return string.IsNullOrEmpty(target);
		}
	}
}
