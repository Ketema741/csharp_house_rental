using HouseStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace HouseStoreApi.Services;

public class RetailorsService
{
    private readonly IMongoCollection<Retailor> _retailorsCollection;

    public RetailorsService(
        IOptions<HouseStoreDatabaseSettings> houseStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            houseStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            houseStoreDatabaseSettings.Value.DatabaseName);

        _retailorsCollection = mongoDatabase.GetCollection<Retailor>(
            houseStoreDatabaseSettings.Value.RetailorCollectionName);
    }

    public async Task<List<Retailor>> GetAsync() =>
        await _retailorsCollection.Find(_ => true).ToListAsync();

    public async Task<Retailor?> GetAsync(string id) =>
        await _retailorsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    
    public async Task<Retailor?> GetAsyncEmail(string email) =>
        await _retailorsCollection.Find(x => x.email == email).FirstOrDefaultAsync();

    public async Task CreateAsync(Retailor newRetailor) =>
        await _retailorsCollection.InsertOneAsync(newRetailor);

    public async Task UpdateAsync(string id, Retailor updatedRetailor) =>
        await _retailorsCollection.ReplaceOneAsync(x => x.Id == id, updatedRetailor);

    public async Task RemoveAsync(string id) =>
        await _retailorsCollection.DeleteOneAsync(x => x.Id == id);
}