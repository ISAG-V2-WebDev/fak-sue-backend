using MongoDB.Driver;

namespace Foods.Core;

public interface IDbClient
{
    IMongoCollection<Food> GetFoodsCollection();
}