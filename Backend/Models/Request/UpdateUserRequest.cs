using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class UpdateUserRequest
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("profile_image")]
    public string? ProfileImage { get; set; }
    
    [JsonPropertyName("student_id")]
    public string? StudentId { get; set; }
}