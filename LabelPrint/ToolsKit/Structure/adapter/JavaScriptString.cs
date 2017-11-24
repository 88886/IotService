using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrintX.Dev.Utils.ToolsKit
{
    internal class JavaScriptString
    {
        private string _s;

        private int _index;

        internal JavaScriptString(string s)
        {
            this._s = s;
        }

        internal char? GetNextNonEmptyChar()
        {
            char? result;
            while (this._s.Length > this._index)
            {
                char c = this._s[this._index++];
                if (!char.IsWhiteSpace(c))
                {
                    result = new char?(c);
                    return result;
                }
            }
            result = null;
            return result;
        }

        internal char? MoveNext()
        {
            char? result;
            if (this._s.Length > this._index)
            {
                result = new char?(this._s[this._index++]);
            }
            else
            {
                result = null;
            }
            return result;
        }

        internal string MoveNext(int count)
        {
            string result;
            if (this._s.Length >= this._index + count)
            {
                string text = this._s.Substring(this._index, count);
                this._index += count;
                result = text;
            }
            else
            {
                result = null;
            }
            return result;
        }

        internal void MovePrev()
        {
            if (this._index > 0)
            {
                this._index--;
            }
        }

        internal void MovePrev(int count)
        {
            while (this._index > 0 && count > 0)
            {
                this._index--;
                count--;
            }
        }

        private static void AppendCharAsUnicode(System.Text.StringBuilder builder, char c)
        {
            builder.Append("\\u");
            builder.AppendFormat(System.Globalization.CultureInfo.InvariantCulture, "{0:x4}", new object[]
			{
				(int)c
			});
        }

        internal static string QuoteString(string value)
        {
            System.Text.StringBuilder stringBuilder = null;
            string result;
            if (string.IsNullOrEmpty(value))
            {
                result = string.Empty;
            }
            else
            {
                int startIndex = 0;
                int num = 0;
                int i = 0;
                while (i < value.Length)
                {
                    char c = value[i];
                    if (c == '\r' || c == '\t' || c == '"' || c == '\'' || c == '\\' || c == '\n' || c == '\b' || c == '\f' || c < ' ')
                    {
                        if (stringBuilder == null)
                        {
                            stringBuilder = new System.Text.StringBuilder(value.Length + 5);
                        }
                        if (num > 0)
                        {
                            stringBuilder.Append(value, startIndex, num);
                        }
                        startIndex = i + 1;
                        num = 0;
                    }
                    char c2 = c;
                    if (c2 <= '"')
                    {
                        switch (c2)
                        {
                            case '\b':
                                stringBuilder.Append("\\b");
                                break;
                            case '\t':
                                stringBuilder.Append("\\t");
                                break;
                            case '\n':
                                stringBuilder.Append("\\n");
                                break;
                            case '\v':
                                goto IL_164;
                            case '\f':
                                stringBuilder.Append("\\f");
                                break;
                            case '\r':
                                stringBuilder.Append("\\r");
                                break;
                            default:
                                if (c2 != '"')
                                {
                                    goto IL_164;
                                }
                                stringBuilder.Append("\\\"");
                                break;
                        }
                    }
                    else if (c2 != '\'')
                    {
                        if (c2 != '\\')
                        {
                            goto IL_164;
                        }
                        stringBuilder.Append("\\\\");
                    }
                    else
                    {
                        JavaScriptString.AppendCharAsUnicode(stringBuilder, c);
                    }
                IL_188:
                    i++;
                    continue;
                IL_164:
                    if (c < ' ')
                    {
                        JavaScriptString.AppendCharAsUnicode(stringBuilder, c);
                    }
                    else
                    {
                        num++;
                    }
                    goto IL_188;
                }
                if (stringBuilder == null)
                {
                    result = value;
                }
                else
                {
                    if (num > 0)
                    {
                        stringBuilder.Append(value, startIndex, num);
                    }
                    result = stringBuilder.ToString();
                }
            }
            return result;
        }

        public override string ToString()
        {
            string result;
            if (this._s.Length > this._index)
            {
                result = this._s.Substring(this._index);
            }
            else
            {
                result = string.Empty;
            }
            return result;
        }

        internal string GetDebugString(string message)
        {
            bool @bool = true;
            return string.Format("{0} ({1}) {2}", message, this._index, @bool ? this._s : "需要显示详细信息，请设置@bool为true");
        }
    }
}
