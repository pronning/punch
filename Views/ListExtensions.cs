using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Punch.Views
{
    public static class ListExtensions
    {
        public static SelectList ToSelectList<T>(T enumeration, string selected)
        {
            var source = Enum.GetValues(typeof(T));

            var items = new Dictionary<object, string>();

            var displayAttributeType = typeof(DisplayAttribute);

            foreach (var value in source)
            {
                FieldInfo field = value.GetType().GetField(value.ToString());

                DisplayAttribute attrs = (DisplayAttribute)field.
                              GetCustomAttributes(displayAttributeType, false).First();

                items.Add(value, attrs.GetName());
            }

            return new SelectList(items, "Key", "Value", selected);
        }

        public static IEnumerable<T> GetFirst<T>(this IEnumerable<T> enumerable, int number)
        {
            if (enumerable == null)
                enumerable = new T[0];

            int last = enumerable.Count();
            if( last > number )
                last = number;

            var array = enumerable.ToArray();
            for (int i = 0; i < last; i++)
            {
                yield return array[i];
            }

        }
    }
}