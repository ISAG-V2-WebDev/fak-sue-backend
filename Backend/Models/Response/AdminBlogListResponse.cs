using System.Text.Json.Serialization;

namespace Backend.Models.Response;

public class AdminBlogListResponse
{
    [JsonPropertyName("blogs")]
    public List<AdminBlogResponse> Blogs { get; set; } = new List<AdminBlogResponse>();
}