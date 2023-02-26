using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseStoreApi.Models;

public class Customer
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }=null!;
    public string fullName { get; set; }=null!;

    public string phone { get; set; } = null!;

    public string email { get; set; } =null!;

    public string password { get; set; } = null!;
    
}