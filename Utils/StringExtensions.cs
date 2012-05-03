using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UDIR.PAS.Core;

namespace Punch.Utils
{
    [DebuggerNonUserCode]
    public static class StringExtensions
    {
        public static bool SafeEquals(this string thisString, string value, bool nullAsEmpty = false)
        {
            if (!nullAsEmpty)
            {
                if (thisString == null)
                    return false;

                if (value == null)
                    return false;

                return (thisString.Trim()).Equals(value.Trim(), StringComparison.OrdinalIgnoreCase);
            }

            return (string.IsNullOrEmpty(thisString) ? string.Empty : thisString.Trim()).Equals(string.IsNullOrEmpty(value) ? string.Empty : value.Trim(), StringComparison.OrdinalIgnoreCase);
        }




        public static string Prepare(this string theString, params object[] args)
        {
            return string.Format(theString, args);
        }

        public static string Prepare(this string theString, object arg0)
        {
            return string.Format(theString, arg0);
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return value == null || value.Trim() == string.Empty;
        }

        public static string ToSeparatedList(this string[] stringArray, char delimitor)
        {
            var sb = new StringBuilder(string.Empty);
            char? delim = null;

            foreach (var s in stringArray.Where(s => !s.IsNullOrEmpty()))
            {
                if (delim != null)
                    sb.Append(delim);

                sb.Append(s.Trim());
                delim = delimitor;
            }

            return sb.ToString();
        }

        public static int[] ToIntArray(this string value)
        {
            if (!value.IsNumeric())
            {
                return new int[0];
            }

            return value.ToCharArray().Select(oneChar => int.Parse(oneChar.ToString())).ToArray();
        }

        public static byte[] StrToByteArray(this string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }


        public static T[] ToArray<T>(this string value, char delimitor)
        {
            return value.ToArray<T>(new[] { delimitor });
        }

        public static T[] ToArray<T>(this string value, char[] delimitors)
        {
            if (value.IsNullOrEmpty())
                return new T[0];

            var list = new List<T>();

            var valueArray = value.Split(delimitors, StringSplitOptions.RemoveEmptyEntries);

            foreach (var s in valueArray.Where(st => !st.IsNullOrEmpty()))
            {
                object objValue = s.Trim().Convert<T>();

                if (objValue != null)
                    list.Add((T)objValue);
            }

            return list.ToArray<T>();
        }

        public static TReturnType Convert<TReturnType>(this string value)
        {
            return value.Convert(default(TReturnType));
        }


        public static TReturnType Convert<TReturnType>(this string value, TReturnType defaultValue)
        {
            return TypeUtil.ChangeType(value, defaultValue);
        }

        public static bool IsNumeric(this string text)
        {
            int tryInt;

            return text.ToCharArray().All(oneChar => int.TryParse(oneChar.ToString(), out tryInt));
        }


        public static Guid ToGuid(this string guidText)
        {
            Guid guid = Guid.Empty;

            try
            {
                guid = new Guid(guidText);
            }
            catch
            {
            }

            return guid;
        }


        public static string Replace(this string stringValue, string[] oldValues, string newValue)
        {
            if (stringValue.IsNullOrEmpty())
                return stringValue;

            foreach (string oldValue in oldValues)
            {
                if (!oldValue.IsNullOrEmpty())
                {
                    stringValue = stringValue.Replace(oldValue, newValue);
                }
            }

            return stringValue;
        }
    }
}