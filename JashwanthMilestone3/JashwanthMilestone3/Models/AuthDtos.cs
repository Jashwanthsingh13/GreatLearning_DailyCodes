using System.ComponentModel.DataAnnotations;

namespace JashwanthMilestone3.Models;

public class RegisterRequest
{
    [Required]
    [MinLength(4)]
    [MaxLength(30)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [MaxLength(100)]
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
