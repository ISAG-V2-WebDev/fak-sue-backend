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
    
    // public Task<IActionResult> GetBlog(string id)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<IActionResult> CreateBlog(CreateBlogRequest body)
    // {
    //     throw new NotImplementedException();
    // }
    //
    // public Task<IActionResult> UpdateBlog(string id, EditContentRequest body)
    // {
    //     throw new NotImplementedException();
    // }
    //
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