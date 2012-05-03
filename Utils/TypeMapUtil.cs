using System;
using MongoDB.Bson;
using Punch.Models;

namespace Punch.Utils
{
    public static class TypeMapUtil
    {
        public static void DateTimeToUniversal(this object item)
        {
            var type = item.GetType();

            var propertyInfos = type.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if( propertyInfo.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)propertyInfo.GetValue(item, null);
                    var utc = value.ToUniversalTime();
                    propertyInfo.SetValue(item,utc, null);
                }
            }
        }

        public static void DateTimeToLocal(this object item)
        {
            var type = item.GetType();

            var propertyInfos = type.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)propertyInfo.GetValue(item, null);
                    var utc = value.ToLocalTime();
                    propertyInfo.SetValue(item, utc, null);
                }
            }
        }


        public static object MapObject(this BsonDocument document, Type type)
        {
            var target = Activator.CreateInstance(type);
            PopulateTarget(target, type, document);
            return target;
        }

        public static TC MapObject<TC>(this BsonDocument document) where TC : new()
        {
            var target = new TC();
            var type = target.GetType();
            PopulateTarget(target, type, document);
            return target;
        }

        private static void PopulateTarget(object target, Type type, BsonDocument document)
        {
            try
            {
            var propertyInfos = type.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                string propertyName = propertyInfo.Name;
                var customAttributes = propertyInfo.GetCustomAttributes(false);
                var fieldAttribute = customAttributes.GetInstanceOf<FieldAttribute>();

                // property name replacement
                if (fieldAttribute != null)
                {
                    if (fieldAttribute.FieldAction == FieldAction.Skip)
                        continue;

                    propertyName = fieldAttribute.DatabaseFieldName ?? propertyName;
                }

                // recursive
                var propertyType = propertyInfo.PropertyType;
                if (propertyType.BaseType == typeof(BaseModel))
                {
                    BsonElement element;
                    if (document.TryGetElement(propertyName, out element))
                    {
                        var innerDocument = element.ToBsonDocument();
                        object innerObject = innerDocument.MapObject(propertyType);
                        propertyInfo.SetValue(target, innerObject, null);
                    }
                    continue;
                }

                var propertyValue = document.GetBsonValue(propertyName);
                if( propertyValue is BsonDocument)
                {
                    var temp = document.GetBsonValue(propertyName);

                }

                if (propertyValue == null)
                    continue;

                var convertedValue = Convert(propertyValue);
                propertyInfo.SetValue(target, convertedValue, null);
            }

            }
            catch
            {
                return;
            }
        }


        public static BsonValue GetBsonValue(this BsonDocument document, string propertyName)
        {
            BsonElement element;
            if(document.TryGetElement(propertyName, out element))
                return element.Value;
            return null;
        }


        public static object GetProperty( this BsonDocument document, string propertyName)
        {
            BsonValue value;
            if (!document.TryGetValue(propertyName, out value))
                return null;
            return value;
        }

        public static T GetProperty<T>(this BsonDocument document, string propertyName)
        {
            BsonValue value;
            if (!document.TryGetValue(propertyName, out value))
                return default(T);

            return Convert<T>(value);
        }

        public static string ToString(BsonValue value)
        {
            return Convert<string>(value);
        }

        public static object Convert(BsonValue value)
        {
            object returnvalue = null;

            try
            {

                TypeSwitch.Do(value,
                              TypeSwitch.Case<BsonBoolean>(() => returnvalue = value.AsBoolean),
                              TypeSwitch.Case<BsonString>(() => returnvalue = value.AsString),
                              TypeSwitch.Case<BsonDateTime>(() => returnvalue = value.AsDateTime),
                              TypeSwitch.Case<BsonDouble>(() => returnvalue = value.AsDouble),
                              TypeSwitch.Case<BsonInt32>(() => returnvalue = value.AsInt32),
                              TypeSwitch.Case<BsonInt64>(() => returnvalue = value.AsInt64),
                              TypeSwitch.Case<BsonObjectId>(() => returnvalue = value.ToString()),
                              TypeSwitch.Case<BsonNull>(() => returnvalue = null),
                              TypeSwitch.Default(() => returnvalue = null)
                    );

                if (returnvalue == null)
                    throw new Exception("Uhåndtert");

                return returnvalue;
            }
            catch (Exception ex)
            {
                throw new Exception("Kunne ikke konvertere " + value.GetType(), ex);
            }
        }


        public static T Convert<T>(BsonValue value)
        {
            var returnvalue = (T) Convert(value);

            if (returnvalue == null)
                return default(T);
            return returnvalue;
        }


        public static BsonObjectId GetOid(this BsonDocument document, object o)
        {
            if ((o == null) || (o is DBNull))
                return default(BsonObjectId);
            return (BsonObjectId)o;
        }

        public static bool GetBool(this BsonDocument document, object o)
        {
            if ((o == null) || (o is DBNull))
                return false;
            return ((BsonBoolean)o).AsBoolean;
        }

        public static double GetDouble(this BsonDocument document, object o)
        {
            if ((o == null) || (o is DBNull))
                return 0;
            return ((BsonDouble)o).AsDouble;
        }

        public static string GetString(this BsonDocument document, object o)
        {
            if ((o == null) || (o is DBNull))
                return default(string);
            return ((BsonString)o).AsString;
        }

        public static DateTime GetDate(this BsonDocument document, object o)
        {
            if ((o == null) || (o is DBNull))
                return default(DateTime);
            return ((BsonDateTime)o).AsDateTime;
        }


        
    }
}