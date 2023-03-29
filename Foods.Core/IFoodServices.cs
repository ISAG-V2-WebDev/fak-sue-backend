namespace Foods.Core;

public interface IFoodServices
{
    List<Food> GetFoods();
    Food GetFood_id(string id);
    Food GetFood_restaurant(string restaurant);
    Food AddFood(Food food);
    void DeleteFood(string id);
    Food UpdateFood(Food food);
}