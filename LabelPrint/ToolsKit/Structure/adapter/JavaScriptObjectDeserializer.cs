using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PrintX.Dev.Utils.ToolsKit
{
    internal class JavaScriptObjectDeserializer
    {
        private const string DateTimePrefix = "\"\\/Date(";

        private const int DateTimePrefixLength = 8;

        internal JavaScriptString _s;

        private JavaScriptSerializer _serializer;

        private int _depthLimit;

        internal static object BasicDeserialize(string input, int depthLimit, JavaScriptSerializer serializer)
        {
            object result;
            try
            {
                JavaScriptObjectDeserializer javaScriptObjectDeserializer = new JavaScriptObjectDeserializer(input, depthLimit, serializer);
                object obj = javaScriptObjectDeserializer.DeserializeInternal(0);
                char? nextNonEmptyChar = javaScriptObjectDeserializer._s.GetNextNonEmptyChar();
                if ((nextNonEmptyChar.HasValue ? new int?((int)nextNonEmptyChar.GetValueOrDefault()) : null).HasValue)
                {
                    throw new System.ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid JSON primitive: {0}.", new object[]
					{
						javaScriptObjectDeserializer._s.ToString()
					}));
                }
                result = obj;
            }
            catch (System.Exception ser)
            {
              
                    string text = string.Format("以下Json字符串解析出错:\n{0}\n\n", input);
                    throw;
            }
            return result;
        }

        private JavaScriptObjectDeserializer(string input, int depthLimit, JavaScriptSerializer serializer)
        {
            this._s = new JavaScriptString(input);
            this._depthLimit = depthLimit;
            this._serializer = serializer;
        }

        private object DeserializeInternal(int depth)
        {
            if (++depth > this._depthLimit)
            {
                throw new System.ArgumentException(this._s.GetDebugString("RecursionLimit exceeded."));
            }
            char? nextNonEmptyChar = this._s.GetNextNonEmptyChar();
            char? c = nextNonEmptyChar;
            object result;
            if (!(c.HasValue ? new int?((int)c.GetValueOrDefault()) : null).HasValue)
            {
                result = null;
            }
            else
            {
                this._s.MovePrev();
                if (this.IsNextElementDateTime())
                {
                    result = this.DeserializeStringIntoDateTime();
                }
                else if (JavaScriptObjectDeserializer.IsNextElementObject(nextNonEmptyChar))
                {
                    System.Collections.Generic.IDictionary<string, object> dictionary = this.DeserializeDictionary(depth);
                    if (dictionary.ContainsKey("__type"))
                    {
                        result = ObjectConverter.ConvertObjectToType(dictionary, null, this._serializer);
                    }
                    else
                    {
                        result = dictionary;
                    }
                }
                else if (JavaScriptObjectDeserializer.IsNextElementArray(nextNonEmptyChar))
                {
                    result = this.DeserializeList(depth);
                }
                else if (JavaScriptObjectDeserializer.IsNextElementString(nextNonEmptyChar))
                {
                    result = this.DeserializeString();
                }
                else
                {
                    result = this.DeserializePrimitiveObject();
                }
            }
            return result;
        }

        private System.Collections.IList DeserializeList(int depth)
        {
            System.Collections.IList list = new System.Collections.ArrayList();
            if (this._s.MoveNext() != '[')
            {
                throw new System.ArgumentException(this._s.GetDebugString("Invalid array passed in, '[' expected."));
            }
            bool flag = false;
            char? c;
            while (true)
            {
                char? nextNonEmptyChar;
                c = (nextNonEmptyChar = this._s.GetNextNonEmptyChar());
                if (!(nextNonEmptyChar.HasValue ? new int?((int)nextNonEmptyChar.GetValueOrDefault()) : null).HasValue || !(c != ']'))
                {
                    break;
                }
                this._s.MovePrev();
                object value = this.DeserializeInternal(depth);
                list.Add(value);
                flag = false;
                c = this._s.GetNextNonEmptyChar();
                if (c == ']')
                {
                    break;
                }
                flag = true;
                if (c != ',')
                {
                    goto Block_6;
                }
            }
            goto IL_147;
        Block_6:
            throw new System.ArgumentException(this._s.GetDebugString("Invalid array passed in, ',' expected."));
        IL_147:
            if (flag)
            {
                throw new System.ArgumentException(this._s.GetDebugString("Invalid array passed in, extra trailing ','."));
            }
            if (c != ']')
            {
                throw new System.ArgumentException(this._s.GetDebugString("Invalid array passed in, ']' expected."));
            }
            return list;
        }

        private System.Collections.Generic.IDictionary<string, object> DeserializeDictionary(int depth)
        {
            System.Collections.Generic.IDictionary<string, object> dictionary = null;
            if (this._s.MoveNext() != '{')
            {
                throw new System.ArgumentException(this._s.GetDebugString("Invalid object passed in, '{' expected."));
            }
            char? c;
            while (true)
            {
                char? nextNonEmptyChar;
                c = (nextNonEmptyChar = this._s.GetNextNonEmptyChar());
                if (!(nextNonEmptyChar.HasValue ? new int?((int)nextNonEmptyChar.GetValueOrDefault()) : null).HasValue)
                {
                    goto IL_250;
                }
                this._s.MovePrev();
                if (c == ':')
                {
                    break;
                }
                string text = null;
                if (c != '}')
                {
                    text = this.DeserializeMemberName();
                    if (string.IsNullOrEmpty(text))
                    {
                        goto Block_7;
                    }
                    if (this._s.GetNextNonEmptyChar() != ':')
                    {
                        goto Block_9;
                    }
                }
                if (dictionary == null)
                {
                    dictionary = new HashMap();
                    if (string.IsNullOrEmpty(text))
                    {
                        goto Block_11;
                    }
                }
                object value = this.DeserializeInternal(depth);
                dictionary[text] = value;
                c = this._s.GetNextNonEmptyChar();
                if (c == '}')
                {
                    goto Block_15;
                }
                if (c != ',')
                {
                    goto Block_17;
                }
            }
            throw new System.ArgumentException(this._s.GetDebugString("Invalid object passed in, member name expected."));
        Block_7:
            throw new System.ArgumentException(this._s.GetDebugString("Invalid object passed in, member name expected."));
        Block_9:
            throw new System.ArgumentException(this._s.GetDebugString("Invalid object passed in, ':' or '}' expected."));
        Block_11:
            c = this._s.GetNextNonEmptyChar();
            if (c != '}')
            {
                throw new System.InvalidOperationException();
            }
        Block_15:
            goto IL_250;
        Block_17:
            throw new System.ArgumentException(this._s.GetDebugString("Invalid object passed in, ':' or '}' expected."));
        IL_250:
            if (c != '}')
            {
                throw new System.ArgumentException(this._s.GetDebugString("Invalid object passed in, ':' or '}' expected."));
            }
            return dictionary;
        }

        private string DeserializeMemberName()
        {
            char? nextNonEmptyChar = this._s.GetNextNonEmptyChar();
            char? c = nextNonEmptyChar;
            string result;
            if (!(c.HasValue ? new int?((int)c.GetValueOrDefault()) : null).HasValue)
            {
                result = null;
            }
            else
            {
                this._s.MovePrev();
                if (JavaScriptObjectDeserializer.IsNextElementString(nextNonEmptyChar))
                {
                    result = this.DeserializeString();
                }
                else
                {
                    result = this.DeserializePrimitiveToken();
                }
            }
            return result;
        }

        private object DeserializePrimitiveObject()
        {
            string text = this.DeserializePrimitiveToken();
            object result;
            if (text.Equals("null"))
            {
                result = null;
            }
            else if (text.Equals("true"))
            {
                result = true;
            }
            else if (text.Equals("false"))
            {
                result = false;
            }
            else
            {
                if (text.IndexOf('.') < 0)
                {
                    int num;
                    if (int.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out num))
                    {
                        result = num;
                        return result;
                    }
                    long num2;
                    if (long.TryParse(text, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out num2))
                    {
                        result = num2;
                        return result;
                    }
                }
                decimal num3;
                if (decimal.TryParse(text, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out num3))
                {
                    result = num3;
                }
                else
                {
                    double num4;
                    if (!double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out num4))
                    {
                        throw new System.ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid JSON primitive: {0}.", new object[]
						{
							text
						}));
                    }
                    result = num4;
                }
            }
            return result;
        }

        private string DeserializePrimitiveToken()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            char? c = null;
            while (true)
            {
                char? c2;
                c = (c2 = this._s.MoveNext());
                if (!(c2.HasValue ? new int?((int)c2.GetValueOrDefault()) : null).HasValue)
                {
                    goto IL_B7;
                }
                if (!char.IsLetterOrDigit(c.Value) && c.Value != '.' && c.Value != '-' && c.Value != '_' && c.Value != '+')
                {
                    break;
                }
                stringBuilder.Append(c);
            }
            this._s.MovePrev();
        IL_B7:
            return stringBuilder.ToString();
        }

        private string DeserializeString()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            bool flag = false;
            char? c = this._s.MoveNext();
            char c2 = this.CheckQuoteChar(c);
            while (true)
            {
                char? c3;
                c = (c3 = this._s.MoveNext());
                if (!(c3.HasValue ? new int?((int)c3.GetValueOrDefault()) : null).HasValue)
                {
                    goto Block_8;
                }
                if (c == '\\')
                {
                    if (flag)
                    {
                        stringBuilder.Append('\\');
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                else if (flag)
                {
                    this.AppendCharToBuilder(c, stringBuilder);
                    flag = false;
                }
                else
                {
                    if (c == c2)
                    {
                        break;
                    }
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString();
        Block_8:
            throw new System.ArgumentException(this._s.GetDebugString("Unterminated string passed in."));
        }

        private void AppendCharToBuilder(char? c, System.Text.StringBuilder sb)
        {
            if (c == '"' || c == '\'' || c == '/')
            {
                sb.Append(c);
            }
            else if (c == 'b')
            {
                sb.Append('\b');
            }
            else if (c == 'f')
            {
                sb.Append('\f');
            }
            else if (c == 'n')
            {
                sb.Append('\n');
            }
            else if (c == 'r')
            {
                sb.Append('\r');
            }
            else if (c == 't')
            {
                sb.Append('\t');
            }
            else
            {
                if (!(c == 'u'))
                {
                    throw new System.ArgumentException(this._s.GetDebugString("Unrecognized escape sequence."));
                }
                sb.Append((char)int.Parse(this._s.MoveNext(4), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture));
            }
        }

        private char CheckQuoteChar(char? c)
        {
            char result = '"';
            if (c == '\'')
            {
                result = c.Value;
            }
            else if (c != '"')
            {
                throw new System.ArgumentException(this._s.GetDebugString("Invalid string passed in, '\"' expected."));
            }
            return result;
        }

        private object DeserializeStringIntoDateTime()
        {
            Match match = Regex.Match(this._s.ToString(), "^\"\\\\/Date\\((?<ticks>-?[0-9]+)(?:[a-zA-Z]|(?:\\+|-)[0-9]{4})?\\)\\\\/\"");
            string value = match.Groups["ticks"].Value;
            long num;
            object result;
            if (long.TryParse(value, out num))
            {
                this._s.MoveNext(match.Length);
                System.DateTime dateTime = new System.DateTime(num * 10000L + JavaScriptSerializer.DatetimeMinTimeTicks, System.DateTimeKind.Utc);
                dateTime = dateTime.ToLocalTime();
                result = dateTime;
            }
            else
            {
                result = this.DeserializeString();
            }
            return result;
        }

        private static bool IsNextElementArray(char? c)
        {
            return c == '[';
        }

        private bool IsNextElementDateTime()
        {
            string text = this._s.MoveNext(8);
            bool result;
            if (text != null)
            {
                this._s.MovePrev(8);
                result = string.Equals(text, "\"\\/Date(", System.StringComparison.Ordinal);
            }
            else
            {
                result = false;
            }
            return result;
        }

        private static bool IsNextElementObject(char? c)
        {
            return c == '{';
        }

        private static bool IsNextElementString(char? c)
        {
            return c == '"' || c == '\'';
        }
    }
}
