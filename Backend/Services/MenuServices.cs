using MongoDB.Driver;
using Backend.Models;

namespace Backend.Services;

public class MenuServices : IMenuServices
{
    private readonly IMongoCollection<Menu> _menu;

    public MenuServices(IDbClient dbClient)
    {
        _menu = dbClient.GetMenuCollection();
    }
    public List<Menu> GetMenu() => _menu.Find(food => true).ToList();

    public Menu GetMenu_id(string id) => _menu.Find(food => food.Id == id).First();

    public List<Menu> GetMenu_restaurant(string restaurant) => _menu.Find(food => food.Restaurant == restaurant).ToList();

    public Menu AddMenu(Menu food)
    {
        _menu.InsertOne(food);
        return food;
    }

    public void DeleteMenu_id(string id) => _menu.DeleteOne(food => food.Id == id);

    public void DeleteMenu_foodname(string foodname) => _menu.DeleteOne(food => food.FoodName == foodname);

    public Menu UpdateMenu(Menu food)
    {
        GetMenu_id(food.Id);
        _menu.ReplaceOne(f => f.Id == food.Id, food);
        return food;
    }
}