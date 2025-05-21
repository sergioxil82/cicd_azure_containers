using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Auditory.API.Data
{
    public class UserRecordHistoryMongo
    {
        // Atributo Id para MongoDB
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        // Mapeador de nombre de atributo
        [BsonElement("userName")]
        public string UserName { get; set; }

        [BsonElement("firtsName")]
        public string FirstName { get; set; }
        
        [BsonElement("lastName")]
        public string LastName { get; set; }

        [BsonElement("lastRecord")]
        public DateTime LastRecord { get; set; }

        [BsonElement("mode")]
        public string Mode { get; set; }
    }
}
