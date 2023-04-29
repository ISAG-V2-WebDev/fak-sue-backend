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
    
    // [Required]
    // [JsonPropertyName("email")]
    // public EmailAddressAttribute
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [Required]
    [JsonPropertyName("confirm_password")]
    public string ConfirmPassword { get; set; } = null!;
    
    [JsonPropertyName("student_id")] 
    public string StudentId { get; set; } = "";
}