namespace BeniceSoft.OpenAuthing.Models.Accounts;

public class TowFactorAuthenticationViewModel
{
    public bool HasAuthenticator { get; set; }
    public bool Is2FaEnabled { get; set; }
    public bool IsMachineRemembered { get; set; }
    public int RecoveryCodesLeft { get; set; }
}