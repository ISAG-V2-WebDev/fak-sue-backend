using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Backend.Models;

namespace Backend.Services;

public class DbClient : IDbClient
{
    private readonly IMongoCollection<Menu> _menu;
    
    public IMongoCollection<User> UserCollection { get; private set; }
    public IMongoCollection<Blog> BlogCollection { get; private set; }
    public IMongoCollection<Announcement> AnnouncementCollection { get; private set; }

    public DbClient(IOptions<FoodsDB_config> foodsDbConfig)
    {
        var client = new MongoClient(foodsDbConfig.Value.Admin_Connection_String);
        var database = client.GetDatabase(foodsDbConfig.Value.Database_Name);
        _menu = database.GetCollection<Menu>(foodsDbConfig.Value.Menu_Collection_Name);
    }
    
    public IMongoCollection<Menu> MenuCollection() => _menu;
}