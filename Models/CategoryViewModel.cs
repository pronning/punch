using System.Collections.Generic;
using MongoDB.Bson;

namespace Punch.Models
{
    public class CategoryViewModel
    {
        public CategoryModel Category { get; set; }
        
        public bool HasDependencies { get; set; }
    }

    public class CategoryDropdownViewModel
    {
        public Dictionary<ObjectId, string> Categories { get; set; }
    }

}