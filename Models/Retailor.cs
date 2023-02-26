using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HouseStoreApi.Models;

public class Retailor
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string fullName { get; set; }=null!;
    public string password { get; set; }=null!;

    public string phone { get; set; } = null!;

    public string email { get; set; } = null!;
    
    public string specializations { get; set; } =null!;

    public string experienceYear { get; set; } =null!;

    public string description { get; set; } =null!;

    public string activityRange { get; set; } =null!;

    public int sold { get; set; }

    
}