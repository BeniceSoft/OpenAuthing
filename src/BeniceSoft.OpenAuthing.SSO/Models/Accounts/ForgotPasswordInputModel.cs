using System.ComponentModel.DataAnnotations;

namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class ForgotPasswordInputModel
{
    [Required] [EmailAddress] public string Email { get; set; }
}