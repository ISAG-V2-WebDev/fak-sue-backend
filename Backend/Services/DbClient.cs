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
        var admin = new MongoClient(foodsDbConfig.Value.Admin_Connection_String);
        var client = new MongoClient(foodsDbConfig.Value.Connection_String);
        
        var adminDatabase = admin.GetDatabase(foodsDbConfig.Value.Menu_Database_Name);
        var clientDatabase = client.GetDatabase(foodsDbConfig.Value.Menu_Database_Name);
        
        _menu = adminDatabase.GetCollection<Menu>(foodsDbConfig.Value.Menu_Collection_Name);
    }
    
    public IMongoCollection<Menu> MenuCollection() => _menu;
    IMongoCollection<Blog> IDbClient.BlogCollection()
    {
        throw new NotImplementedException();
    }

    IMongoCollection<Announcement> IDbClient.AnnouncementCollection()
    {
        throw new NotImplementedException();
    }
}