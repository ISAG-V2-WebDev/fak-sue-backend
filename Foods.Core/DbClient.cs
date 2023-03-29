using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Foods.Core;

public class DbClient : IDbClient
{
    private readonly IMongoCollection<Food> _foods;

    public DbClient(IOptions<FoodsDB_config> foodsDbConfig)
    {
        var client = new MongoClient(foodsDbConfig.Value.Admin_Connection_String);
        var database = client.GetDatabase(foodsDbConfig.Value.Database_Name);
        _foods = database.GetCollection<Food>(foodsDbConfig.Value.Foods_Collection_Name);
    }
    
    public IMongoCollection<Food> GetFoodsCollection() => _foods;
}