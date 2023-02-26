namespace HouseStoreApi.Models;

public class HouseStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string HouseCollectionName { get; set; } = null!;
    public string RetailorCollectionName {get; set;} = null!;
    public string CustomerCollectionName {get; set;} = null!;
}