using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MSUi.Models
{
   
        public class Note
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string? Id { get; set; }
            [BsonElement("PatId")]
            public int? PatId { get; set; }
            public string Patient { get; set; }
            public string Notes { get; set; }
        }
    }

