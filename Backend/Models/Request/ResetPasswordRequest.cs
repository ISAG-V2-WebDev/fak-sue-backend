using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Backend.Models.Request;

public class ResetPasswordRequest
{
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = null!;

    [Required]
    [JsonPropertyName("confirm_password")]
    public string ConfirmPassword { get; set; } = null!;
}