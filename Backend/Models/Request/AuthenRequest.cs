using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class AuthenRequest
{
    [Required]
    [JsonPropertyName("username")]
    public string username { get; set; }
    
    [Required]
    [JsonPropertyName("password")]
    public string password { get; set; }
}