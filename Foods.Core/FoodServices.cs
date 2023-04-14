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

    public Food GetFood_id(string id) => _foods.Find(food => food.Id == id).First();

    public List<Food> GetFood_restaurant(string restaurant) => _foods.Find(food => food.Restaurant == restaurant).ToList();

    public Food AddFood(Food food)
    {
        _foods.InsertOne(food);
        return food;
    }

    public void DeleteFood_id(string id) => _foods.DeleteOne(food => food.Id == id);

    public void DeleteFood_foodname(string foodname) => _foods.DeleteOne(food => food.FoodName == foodname);

    public Food UpdateFood(Food food)
    {
        GetFood_id(food.Id);
        _foods.ReplaceOne(f => f.Id == food.Id, food);
        return food;
    }
}