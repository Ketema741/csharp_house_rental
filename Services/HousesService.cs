using HouseStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HouseStoreApi.Services;

public class HousesService
{
    private readonly IMongoCollection<House> _housesCollection;
    

    public HousesService(
        IOptions<HouseStoreDatabaseSettings> houseStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            houseStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            houseStoreDatabaseSettings.Value.DatabaseName);

        _housesCollection = mongoDatabase.GetCollection<House>(
            houseStoreDatabaseSettings.Value.HouseCollectionName);
    }

    public async Task<List<House>> GetAsync() =>
        await _housesCollection.Find(_ => true).ToListAsync();

    public async Task<House?> GetAsync(string id) =>
        await _housesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(House newBook) =>
        await _housesCollection.InsertOneAsync(newBook);

    public async Task UpdateAsync(string id, House updatedBook) =>
        await _housesCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAsync(string id) =>
        await _housesCollection.DeleteOneAsync(x => x.Id == id);
}