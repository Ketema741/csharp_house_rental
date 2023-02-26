using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseStoreApi.Models
{
    public class House
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;

        public string realtorId { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public string location { get; set; } 

        public int price { get; set; }

        public int area { get; set; }

        public string propertyType { get; set; } 

        public int bed { get; set; } 

        public string yearBuilt { get; set; }
    }
}
