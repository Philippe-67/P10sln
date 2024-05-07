using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MSNote.Models
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
