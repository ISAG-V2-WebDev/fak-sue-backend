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

    [HttpPost]
    public IActionResult AddFood(Food food)
    {
        _foodServices.AddFood(food);
        return Ok(food);
    }
}