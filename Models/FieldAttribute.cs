using System;

namespace Punch.Models
{
    public class FieldAttribute : Attribute
    {
        public string DatabaseFieldName { get; set; }
        public FieldAction FieldAction { get; set; }
        public string CollectionName { get; set; }
    }

    public enum FieldAction
    {
        Map,
        Skip
    }
}