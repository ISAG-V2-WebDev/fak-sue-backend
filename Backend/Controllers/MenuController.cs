using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Backend.Models;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuServices _menuServices;
    private readonly ILogger<MenuController> _logger;
    public MenuController(IMenuServices menuServices, ILogger<MenuController> logger)
    {
        _menuServices = menuServices;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetMenu()
    {
        return Ok(_menuServices.GetMenu());
    }

    [HttpGet("id={id}", Name = "GetFood_id")]
    public IActionResult GetMenu_id(string id)
    {
        return Ok(_menuServices.GetMenu_id(id));
    }
    
    [HttpGet("restaurant={restaurant}", Name = "GetFood_restaurant")]
    public IActionResult GetMenu_restaurant(string restaurant)
    {
        return Ok(_menuServices.GetMenu_restaurant(restaurant));
    }

    [HttpPost]
    public IActionResult AddMenu(Menu food)
    {
        _menuServices.AddMenu(food);
        // return Ok(_foodServices.AddFood(food));
        return Ok(CreatedAtRoute("GetFood", new {id = food.Id}, food));
    }

    [HttpDelete("id={id}")]
    public IActionResult DeleteMenu_id(string id)
    {
        _menuServices.DeleteMenu_id(id);
        return NoContent();
    }

    [HttpPut]
    public IActionResult UpdateMenu(Menu food)
    {
        return Ok(_menuServices.UpdateMenu(food));
    }
}