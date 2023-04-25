using System.Security.Claims;
using Backend.Models;
using Backend.Models.Request;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
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
    [Route("create/blogid={blogId:length(24)}")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest body, string blogId)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");

        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found");

        Blog? blog = await _blog.Find(x => x.Id == body.BlogId && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found");
        if (blog.Orders!.Count() == blog.MaxOrder)
            return BadRequest("This blog has reached its max order limited");

        Order newOrder = new Order { User = user, Detail = body.Detail, Menu = body.Menu, Quantity = body.Quantity, BlogId = body.BlogId};
        blog.Orders!.Add(newOrder);
        await _blog.ReplaceOneAsync(x => x.Id == newOrder.BlogId, blog);
        
        return Ok(newOrder);
    }

    [HttpGet]
    [Route("blogid={blogId:length(24)}/id={orderId:length(24)}")]
    public async Task<IActionResult> GetOrder(string blogId, string orderId)
    {
        Blog? blog = await _blog.Find(x => x.Id == blogId && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found");

        Order? order = blog.Orders!.Find(x => x.Id == orderId);
        if (order == null)
            return NotFound("Order is not found");

        return Ok(order);
    }

    [HttpPatch]
    [Route("update/blogid={blogId:length(24)}/id={orderId:length(24)}")]
    public async Task<IActionResult> UpdateOrder(string blogId, string orderId, EditOrderRequest body)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found");
        
        Blog? blog = await _blog.Find(x => x.Id == blogId && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found");

        Order? order = blog.Orders!.Find(x => x.Id == orderId);
        if (order == null)
            return NotFound("Order is not found");
        if (order.User.Username != username)
            return Unauthorized("You are not this author of this order");
        Order? newOrder = order;

        newOrder.Menu = body.Menu ?? order.Menu;
        newOrder.Detail = body.Detail ?? order.Detail;
        newOrder.Quantity = body.Quantity ?? order.Quantity;

        blog.Orders.Remove(order);
        blog.Orders.Add(newOrder);

        await _blog.ReplaceOneAsync(x => x.Id == blogId, blog);
        return Ok(newOrder);
    }

    [HttpPatch]
    [Route("delete/blogid={blogId:length(24)}/id={orderId:length(24)}")]
    public async Task<IActionResult> DeleteOrder(string blogId, string orderId)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (string.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");
        
        User? user = await _user.Find(x => x.Username == username && !x.Banned && !x.Deleted).FirstOrDefaultAsync();
        if (user == null)
            return NotFound("User is not found");
        
        Blog? blog = await _blog.Find(x => x.Id == blogId && !x.Hide && !x.Deleted).FirstOrDefaultAsync();
        if (blog == null)
            return NotFound("Blog is not found");

        Order? order = blog.Orders!.Find(x => x.Id == orderId && x.User.Username == username);
        if (order == null)
            return NotFound("Order is not found");

        blog.Orders.Remove(order);

        await _blog.ReplaceOneAsync(x => x.Id == blogId, blog);
        
        return NoContent();
    }
}