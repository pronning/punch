using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Punch.Models
{
    public abstract class BaseModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}