using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Punch.Models;

namespace Punch.Data
{
    public static class Importer
    {
        public static List<string> Import()
        {
            var server = MongoServer.Create(ConnectionStrings.ConnectionString);
            var database = server.GetDatabase(ConnectionStrings.DataBaseName);
            var collection = database.GetCollection("expenses").FindAll();


            ModelCollection<ExpenseModel>.Destroy();
            var messages = new List<string>();
            int i = 0;
            foreach (var doc in collection)
            {
                messages.Add(i.ToString() + ": " + Ins(doc));
                i++;
            }

            messages.Add(InsertCategories());




            return messages;
        }

        private static string InsertCategories()
        {
           

            var cat = new CategoryModel
                          {
                              DefaultCommon = true,
                              Value = "Mat og husholdn.",
                              Filter = "Kiwi, Rimi, Ica, Rema, Meny"
                          };
            ModelCollection<CategoryModel>.InsertItem(cat);

            var cat2 = new CategoryModel
            {
                DefaultCommon = true,
                Value = "Bil",
                Filter = "Esso, Shell, Jet"
            };

            ModelCollection<CategoryModel>.InsertItem(cat2);

            return "kateg";
        }

        private static string Ins(BsonDocument doc)
        {
            try
            {
                BsonValue id;
            if (!doc.TryGetValue("_id", out id))
                throw new Exception("fant ikke _id");


                var exp = new ExpenseModel
                              {
                                  IsCommon = doc.GetValue("IsCommon", true).ToBoolean(),
                                  IsPossibleDuplicate = doc.GetValue("IsPossibleDuplicate", false).ToBoolean(),
                                  Means = doc.GetValue("Means", string.Empty).ToString(),
                                  Owner = doc.GetValue("Owner", string.Empty).ToString(),
                                  Amount = doc.GetValue("Amount", 0).ToDouble(),
                                  Description = doc.GetValue("Description", string.Empty).ToString(),
                                  Date = doc.GetValue("Date").AsDateTime
                              };
                var nu = ModelCollection<ExpenseModel>.InsertItem(exp);
                return "insert " + nu.Id;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}