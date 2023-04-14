namespace Foods.Core;

public interface IFoodServices
{
    List<Food> GetFoods();
    Food GetFood_id(string id);
    List<Food> GetFood_restaurant(string restaurant);
    Food AddFood(Food food);
    void DeleteFood_id(string id);
    void DeleteFood_foodname(string foodname);
    Food UpdateFood(Food food);
}