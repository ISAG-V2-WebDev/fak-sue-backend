using Backend.Models;

namespace Backend.Services;

public interface IMenuServices
{
    List<Menu> GetMenu();
    Menu GetMenu_id(string id);
    List<Menu> GetMenu_restaurant(string restaurant);
    Menu AddMenu(Menu food);
    void DeleteMenu_id(string id);
    void DeleteMenu_foodname(string foodname);
    Menu UpdateMenu(Menu food);
}