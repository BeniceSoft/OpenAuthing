using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
    
    public string? ReturnUrl { get; set; }
    
    public List<AuthenticationScheme>? ExternalLogins { get; set; }
}