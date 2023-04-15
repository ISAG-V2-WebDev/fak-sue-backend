using System.Text.Json.Serialization;

namespace Backend.Models.Response;

public class BlogListResponse
{
    [JsonPropertyName("blogs")]
    public List<BlogResponse> Blogs { get; set; } = new List<BlogResponse>();
}