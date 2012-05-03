using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Punch.Utils;

namespace Punch.Data
{
    public static class ModelCollection<T> where T : new()
    {
        #region collections

        private static Type ContentType { get { return typeof (T); } }

        private static MongoCollection<T> DataCollection { get { return ConnectionManager<T>.GetCollection(ContentType.Name); } }
        
        #endregion

        public static T GetItem( string id )
        {
            try
            {
                var item = DataCollection.FindOneById(new BsonObjectId(id));
                item.DateTimeToLocal();
                return item;
            }
            catch
            {
                return default(T);
            }
        }

        public static T GetOne(QueryComplete query)
        {
            var items = GetCollection(query);
            return items.FirstOrDefault();
        }


        public static List<T> GetCollection()
        {
            return GetCollection(null);
        }


        public static List<T> GetCollection(QueryComplete query)
        {
            var resultCollection = (query == null)
                                       ? DataCollection.FindAll()
                                       : DataCollection.Find(query);
            var list = resultCollection.ToList();
            foreach (T item in list)
            {
                item.DateTimeToLocal();
            }

            return list;
        }

        //public static IEnumerable<T> Insert( IEnumerable<T> items)
        //{
        //    return items.Select(InsertItem);
        //}

        public static T InsertItem(T item)
        {
            item.DateTimeToUniversal();
            try
            {
                var res = DataCollection.Insert(item, SafeMode.True);

                if (!res.Ok)
                {
                    throw new Exception("Klarte ikke å lagre: " + res.LastErrorMessage);
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"c:/temp/debug.txt", ex.Message);
            }
            
            return item;
        }

        public static void UpdateItem(T item)
        {
            item.DateTimeToUniversal();
            var res = DataCollection.Save(item, SafeMode.True);
            if (!res.Ok || res.DocumentsAffected == 0)
            {
                throw new Exception("Klarte ikke å oppdatere");
            }
        }

        public static void DeleteItem(string id)
        {
            var query = Query.EQ("_id", new BsonObjectId(id));
            var res = DataCollection.Remove(query, SafeMode.True);
            if (!res.Ok)
            {
                throw new Exception("Klarte ikke å slette");
            }
        }

        public static void Destroy()
        {
            if (DataCollection.Exists())
                DataCollection.Drop();
        }
    }
}