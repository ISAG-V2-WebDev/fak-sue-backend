using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Backend.Models;

namespace Backend.Services;

public class DbClient : IDbClient
{
    private readonly IMongoCollection<Menu> _menu;
    private IMongoCollection<User> _user;
    private IMongoCollection<Blog> _blog;
    private IMongoCollection<Announcement> _announcement;

    public DbClient(IOptions<FoodsDB_config> foodsDbConfig)
    {
        var admin = new MongoClient(foodsDbConfig.Value.Admin_Connection_String);
        var client = new MongoClient(foodsDbConfig.Value.Connection_String);
        
        var adminMenuDatabase = admin.GetDatabase(foodsDbConfig.Value.Menu_Database_Name);
        var clientMenuDatabase = client.GetDatabase(foodsDbConfig.Value.Menu_Database_Name);
        
        var adminUserDatabase = admin.GetDatabase(foodsDbConfig.Value.User_Database_Name);
        var clientUserDatabase = client.GetDatabase(foodsDbConfig.Value.User_Database_Name);
        
        _menu = adminMenuDatabase.GetCollection<Menu>(foodsDbConfig.Value.Menu_Collection_Name);
        _user = adminUserDatabase.GetCollection<User>(foodsDbConfig.Value.User_Collection_Name);
        _blog = adminUserDatabase.GetCollection<Blog>(foodsDbConfig.Value.Order_Collection_Name);
    }
    
    public IMongoCollection<Menu> MenuCollection() => _menu;
    public IMongoCollection<User> UserCollection() => _user;
    public IMongoCollection<Blog> BlogCollection() => _blog;
    public IMongoCollection<Announcement> AnnouncementCollection()
    {
        throw new NotImplementedException();
    }
}