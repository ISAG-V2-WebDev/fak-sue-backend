using Backend.Models;
using Backend.Models.Request;
using Backend.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Services;

public interface IBlogServices
{
    public Task<BlogListResponse> GetBlogs();
    public Task<BlogResponse> GetBlog(string id);
    public Task<BlogResponse> CreateBlog([FromBody] CreateBlogRequest body, string username);
    public Task<Blog?> UpdateBlog(string id, [FromBody] EditContentRequest body);
    public Task<BlogResponse> HideBlog(string id);
    public Task<BlogResponse> DeleteBlog(string id);
}