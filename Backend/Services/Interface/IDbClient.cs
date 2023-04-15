using MongoDB.Driver;
using Backend.Models;

namespace Backend.Services;

public interface IDbClient
{
    IMongoCollection<Menu> GetMenuCollection();
}