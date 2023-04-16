using System.Security.Claims;
using Backend.Models;
using Backend.Models.Request;
using Backend.Models.Response;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
// [Authorize]
public class BlogController : ControllerBase
{
    private readonly IBlogServices _blogServices;
    private readonly ILogger<BlogController> _logger;

    public BlogController(IBlogServices blogService, ILogger<BlogController> logger)
    {
        _blogServices = blogService;
        _logger = logger;
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("list")]
    public async Task<IActionResult> GetBlogs()
    {
        return Ok(await _blogServices.GetBlogs());
    }
    
    [HttpGet]
    [AllowAnonymous]
    [Route("id={id:length(24)}")]
    public async Task<IActionResult> GetBlog(string id)
    {
        return Ok(await _blogServices.GetBlog(id));
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateBlog(CreateBlogRequest body)
    {
        string? username = Request.HttpContext.User.FindFirstValue("username");
        if (String.IsNullOrEmpty(username))
            return Unauthorized("You are not authorized user.");

        BlogResponse blogResponse = await _blogServices.CreateBlog(body, username);
        if (blogResponse.Author == null || blogResponse.BlogId == null)
            return NotFound("User is not found");

        return Ok(CreatedAtAction("CreateBlog", blogResponse));                             //Change this, maybe?
    }
    
    [HttpPatch]
    [Route("update/id={id:length(24)}")]
    public async Task<IActionResult> UpdateBlog(string id, EditContentRequest body)
    {
        Blog? blog = await _blogServices.UpdateBlog(id, body);
        if (blog == null)
            return NotFound("Blog is not found.");
        
        return Ok(blog);
    }
    
    // public Task<IActionResult> HideBlog(string id)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<IActionResult> DeleteBlog(string id)
    // {
    //     throw new NotImplementedException();
    // }
}