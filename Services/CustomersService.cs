using HouseStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HouseStoreApi.Services;

public class CustomersService
{
    private readonly IMongoCollection<Customer> _customersCollection;

    public CustomersService(
        IOptions<HouseStoreDatabaseSettings> houseStoreDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            houseStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            houseStoreDatabaseSettings.Value.DatabaseName);

        
        _customersCollection = mongoDatabase.GetCollection<Customer>(
            houseStoreDatabaseSettings.Value.CustomerCollectionName);
    }

    public async Task<List<Customer>> GetAsync() =>
        await _customersCollection.Find(_ => true).ToListAsync();

    public async Task<Customer?> GetAsync(string id) =>
        await _customersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Customer newRetailor) =>
        await _customersCollection.InsertOneAsync(newRetailor);

    public async Task UpdateAsync(string id, Customer updatedRetailor) =>
        await _customersCollection.ReplaceOneAsync(x => x.Id == id, updatedRetailor);

    public async Task RemoveAsync(string id) =>
        await _customersCollection.DeleteOneAsync(x => x.Id == id);
}