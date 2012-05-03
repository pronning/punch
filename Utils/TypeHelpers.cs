using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Punch.Utils
{
    public static class TypeHelpers
    {
        public static bool IsDate( this string dateString )
        {
            if( string.IsNullOrEmpty(dateString))
                return false;

            DateTime d;
            return DateTime.TryParse(dateString, out d);
        }

        public static IEnumerable<string> Clean(this IEnumerable<string> enumerable)
        {
            return enumerable.Select(str => str.TrimStart('"').TrimEnd('"'));
        }

        public static bool ContainsCaseInsensitive(this string source, string value)
        {
            int results = source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
            return results != -1;
        }

        public static T GetInstanceOf<T>(this object[] attributes) where T : class
        {
            if( attributes.Length == 0)
                return null;

            return (from attribute in attributes where attribute.GetType() == typeof (T) select attribute as T).FirstOrDefault();
        }

        public static string ToSeparatedArray<T>(this IEnumerable<T> array, char separator)
        {
            string ret = array.Cast<object>().Aggregate(string.Empty, (current, item) => current + (item.ToString() + separator));
            return ret.TrimEnd(separator);
        }

       
        public static byte[] ToByteArray(this Stream stream)
        {
            if (stream == null)
                return new byte[0];

            var packageFileContent = new byte[stream.Length];
            using (var br = new BinaryReader(stream))
            {
                stream.Position = 0;
                for (long i = 0; i < stream.Length; i++)
                {
                    packageFileContent[i] = br.ReadByte();
                }
            }
            return packageFileContent;
        }
    }
}