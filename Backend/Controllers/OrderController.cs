using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMongoCollection<Blog> _blog;
    private readonly IMongoCollection<User> _user;
    private readonly ILogger<BlogController> _logger;

    public OrderController(ILogger<BlogController> logger, IDbClient dbClient)
    {
        _blog = dbClient.BlogCollection();
        _user = dbClient.UserCollection();
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
        
        return Ok();
    }
}