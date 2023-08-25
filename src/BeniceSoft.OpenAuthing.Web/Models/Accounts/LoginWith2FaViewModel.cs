using System.ComponentModel.DataAnnotations;

namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class LoginWith2FaViewModel
{
    public string? ReturnUrl { get; set; }
    public bool RememberMe { get; set; }

    [Required]
    public string TwoFactorCode { get; set; } = default!;
    
    public bool RememberMachine { get; set; }
}