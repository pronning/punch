using System;
using System.Collections.Generic;

namespace Punch.Models
{
    public class EventModel
    {
        public bool Success { get; set; }
        public DateTime Time { get; set; }
        public string Description { get; set; }
        public List<MessageModel> Messages { get; set; }
    }
}