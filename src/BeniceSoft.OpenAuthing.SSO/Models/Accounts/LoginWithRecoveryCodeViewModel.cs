namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class LoginWithRecoveryCodeViewModel
{
    public string? ReturnUrl { get; set; }
    public string RecoveryCode { get; set; }
}