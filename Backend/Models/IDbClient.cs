using MongoDB.Driver;

namespace Backend.Models;

public interface IDbClient
{
    IMongoCollection<Menu> GetMenuCollection();
}