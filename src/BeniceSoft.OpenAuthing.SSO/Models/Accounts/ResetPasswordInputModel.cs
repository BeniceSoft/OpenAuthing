using System.ComponentModel.DataAnnotations;

namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class ResetPasswordInputModel
{
    [Required]  public string Uid { get; set; }
    [Required] public string Password { get; set; }
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string? ConfirmPassword { get; set; }
    [Required] public string Code { get; set; }
}