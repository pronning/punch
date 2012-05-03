using System.Collections.Generic;
using System.Linq;
using Punch.Models;

namespace Punch.Data
{
    
    public class CategoryDataManager
    {
        public static IEnumerable<CategoryModel> GetCategories()
        {
            var list = ModelCollection<CategoryModel>.GetCollection();
            return list;
        }

        public static CategoryModel GetCategory(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var category = ModelCollection<CategoryModel>.GetItem(id);
            return category;
        }

        public static CategoryModel InsertItem(CategoryModel category)
        {
            category = ModelCollection<CategoryModel>.InsertItem(category);
            return category;
        }
        
        public static bool UpdateItem(CategoryModel category)
        {
            try
            {
                ModelCollection<CategoryModel>.UpdateItem(category);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static void DeleteItem(string id)
        {
            if (string.IsNullOrEmpty(id))
                return;

            ModelCollection<CategoryModel>.DeleteItem(id);
        }

        internal static Dictionary<string,CategoryModel> GetFilters()
        {
            var categories = GetCategories().Where( item => !string.IsNullOrEmpty(item.Filter));
            var filterMap = new Dictionary<string, CategoryModel>();

            foreach( var category in categories)
            {
                string[] filters = category.Filter.Split(',');
                foreach (string t in filters)
                {
                    var filter = t.Trim();
                    if (string.IsNullOrEmpty(filter) || filterMap.ContainsKey(filter))
                        continue;

                    filterMap.Add(filter, category);
                }
            }

            return filterMap;
        }
    }
}