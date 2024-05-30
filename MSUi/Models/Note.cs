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
      //  public List<string> Notes { get; set; }  // Modifier la propriété Notes pour qu'elle soit une liste de chaînes de caractères
    }
    }

