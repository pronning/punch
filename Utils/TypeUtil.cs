using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Punch.Utils;


namespace UDIR.PAS.Core
{
    //[DebuggerNonUserCode]
	public class TypeUtil
	{
        public static T IsNull<T>(object value)
		{
			return IsNull(value, default(T));
		}

		public static T IsNull<T>(object value, T defaultValue)
		{
			if (value == null || value == DBNull.Value)				
				return defaultValue;

			var retValue        = default(T);
			var retValueType    = typeof(T);

			try
			{
				var valueType = value.GetType();

				if (valueType == retValueType || valueType.IsSubclassOf(retValueType))
					retValue = (T) value;
				else if (retValueType.IsEnum)
					retValue = (T) Enum.Parse(retValueType, value.ToString(), true);
				else
					retValue = ChangeType<T>(value);
			}
			catch { }

			return retValue;
		}


		public static T ChangeType<T>(object value)
		{
			return ChangeType(value, default(T));
		}

		public static T ChangeType<T>(object value, T defaultValue)
		{
			var conversionType = typeof(T);
			var valueType = value == null ? null : value.GetType();
			var retValue = defaultValue;


			if (value == null || value == DBNull.Value || (valueType == typeof(string) && ((string)value).Trim() == string.Empty))
				return defaultValue;

			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				var nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			}

			try { retValue = (T)Convert.ChangeType(value, conversionType, System.Threading.Thread.CurrentThread.CurrentCulture); }
			catch { }

			return retValue;
		}


		public static object ChangeType(object value, Type type, bool handleExceptions = false)
		{
			var conversionType = type;
			var valueType = value == null ? null : value.GetType();
			
			if (value == null || value == DBNull.Value || (valueType == typeof (string) && ((string)value).Trim() == string.Empty))
				return DefaultForType(type);

			if (value is int && type.IsEnum)
				return value;

			if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof (Nullable<>)))
			{
				var nullableConverter = new NullableConverter(conversionType);
				conversionType = nullableConverter.UnderlyingType;
			}

			try
			{
				return Convert.ChangeType(value, conversionType, System.Globalization.CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				if (!handleExceptions)
					throw;
			}

			return DefaultForType(type);
		}

		private static object DefaultForType(Type targetType)
		{
			return targetType.IsValueType ? System.Activator.CreateInstance(targetType) : null;
		}

		public static void SetObjectsProperty<T>(string propertyName, object value, params T[] objects)
		{
			PropertyInfo pi = null;
			Type type = null;

			foreach (var obj in objects)
			{
				if (pi == null)
				{
					type = obj.GetType();
					pi = type.GetProperty(propertyName);
				}

				if (pi == null)
					throw new ArgumentException(string.Format("Object of type \"{0}\" does not have property \"{1}\"", type.Name, propertyName));

				pi.SetValue(obj, value, null);
			}
		}

		public static object Coalesce(params object[] values)
		{
			object retObject = null;

			for (var i = 0; i <= values.Length - 1; i++)
			{
				if (values[i] == null)
					continue;

				retObject = values[i];
				break;
			}

			return retObject;
		}


		public static bool IsNullableType(Type theType)
		{
			if (theType == null)
				return false;

			return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
		}

		[DebuggerNonUserCode]
		public static string ToString(object objectValue)
		{
			return ToString(objectValue, false);
		}


		[DebuggerNonUserCode]
		public static string ToString(object objectValue, bool includeCollections) 
		{

			if (objectValue == null)
				return string.Empty;

			var objectString    = string.Empty;
			var delim           = string.Empty;

			try
			{
				var properties = objectValue.GetType().GetProperties().ToList().OrderBy(p => p.Name);

				foreach (var pi in properties)
				{
					if (pi.Name.Equals("ExtensionData"))
						continue;

					var value = pi.GetValue(objectValue, null);

					if (value is ICollection && includeCollections)
						objectString += "{0}{1}: {2}".Prepare(delim, pi.Name, CollectionToString((ICollection)value));
					else
						objectString += "{0}{1}: {2}".Prepare(delim, pi.Name, value == null ? "Null" : value.ToString());
					
					delim = ", ";
				}
			}
			catch
			{
				objectString = objectValue.ToString();
			}

			return objectString;
		}

		[DebuggerNonUserCode]
		public static string CollectionToString(ICollection collection)
		{
			var collectionString = string.Empty;
			
			if (collection == null)
				return collectionString;

			var delim = string.Empty;

			foreach (var item in collection)
			{
				collectionString += delim + ((item == null) ? "Null" : item.ToString());
				delim = ", ";
			}

			return "{{{0}}}\n".Prepare(collectionString);
		}

		public static IEnumerable<Type> FindImplementingTypes<T>()
		{
			var targetType = typeof(T);
			return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes().Where(t => t.GetInterfaces().Contains(targetType)));
		}

		public static TAttributeType GetAttribute<TAttributeType>(MethodInfo methodInfo) where TAttributeType: Attribute
		{
			if (methodInfo == null)
				return null;

			return methodInfo.GetCustomAttributes(true).FirstOrDefault(a => a is TAttributeType) as TAttributeType;
		}


		public static bool HasAttribute<TAttributeType>(Type componentType)
		{
			return componentType.GetCustomAttributes(true).OfType<TAttributeType>().Any();
		}


	    public static void DisposeIf(object obj)
	    {
	        if (obj == null)
	            return;

	        var disposable = obj as IDisposable;

            if (disposable != null)
                disposable.Dispose();
	    }

        public static DateTime TestUtc()
        {
            DateTime dateTime = DateTime.Now;

            return dateTime;
        }
	}
}
