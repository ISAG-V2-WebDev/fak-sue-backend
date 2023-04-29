using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class RegisterRequest
{
    [Required]
    [JsonPropertyName("username")]
    public string Username { get; set; } = null!;

    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("student_id")] 
    public string StudentId { get; set; } = "";
    
    [Required]
    [JsonPropertyName("confirm_password")]
    public string ConfirmPassword { get; set; } = null!;
}