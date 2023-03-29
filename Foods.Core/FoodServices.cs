using MongoDB.Driver;

namespace Foods.Core;

public class FoodServices : IFoodServices
{
    private readonly IMongoCollection<Food> _foods;

    public FoodServices(IDbClient dbClient)
    {
        _foods = dbClient.GetFoodsCollection();
    }
    public List<Food> GetFoods() => _foods.Find(food => true).ToList();

    public Food GetFood_id(string id)
    {
        throw new NotImplementedException();
    }

    public Food GetFood_restaurant(string restaurant)
    {
        throw new NotImplementedException();
    }

    public Food AddFood(Food food)
    {
        _foods.InsertOne(food);
        return food;
    }

    public void DeleteFood(string id)
    {
        throw new NotImplementedException();
    }

    public Food UpdateFood(Food food)
    {
        throw new NotImplementedException();
    }
}