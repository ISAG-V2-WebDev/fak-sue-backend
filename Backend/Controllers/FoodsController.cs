using Foods.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodsController : ControllerBase
{
    private readonly IFoodServices _foodServices;
    
    public FoodsController(IFoodServices foodServices)
    {
        _foodServices = foodServices;
    }

    [HttpGet]
    public IActionResult GetFoods()
    {
        return Ok(_foodServices.GetFoods());
    }

    [HttpGet("id={id}", Name = "GetFood_id")]
    public IActionResult GetFood_id(string id)
    {
        return Ok(_foodServices.GetFood_id(id));
    }
    
    [HttpGet("restaurant={restaurant}", Name = "GetFood_restaurant")]
    public IActionResult GetFood_restaurant(string restaurant)
    {
        return Ok(_foodServices.GetFood_restaurant(restaurant));
    }

    [HttpPost]
    public IActionResult AddFood(Food food)
    {
        _foodServices.AddFood(food);
        // return Ok(_foodServices.AddFood(food));
        return Ok(CreatedAtRoute("GetFood", new {id = food.Id}, food));
    }

    [HttpDelete("id={id}")]
    public IActionResult DeleteFood_id(string id)
    {
        _foodServices.DeleteFood_id(id);
        return NoContent();
    }

    [HttpPut]
    public IActionResult UpdateFood(Food food)
    {
        return Ok(_foodServices.UpdateFood(food));
    }
}