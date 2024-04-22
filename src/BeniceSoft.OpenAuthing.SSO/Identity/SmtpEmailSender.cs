using BeniceSoft.OpenAuthing.Entities.Users;
using Microsoft.AspNetCore.Identity;
using IEmailSender = Volo.Abp.Emailing.IEmailSender;

namespace BeniceSoft.OpenAuthing.Identity;

public class SmtpEmailSender(IEmailSender emailSender, ILogger<SmtpEmailSender> logger) : IEmailSender<User>
{
    public Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        logger.LogDebug("Sending confirmation link to user: {0}, confirmationLink: {1}", user.UserName, confirmationLink);
        // ISSUE: reference to a compiler-generated field
        return emailSender.QueueAsync(email, "Confirm your email", "Please confirm your account by <a href='" + confirmationLink + "'>clicking here</a>.");
    }

    public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        logger.LogDebug("Sending reset password link to user: {0}, resetLink: {1}", user.UserName, resetLink);
        // ISSUE: reference to a compiler-generated field
        return emailSender.QueueAsync(email, "Reset your password", "Please reset your password by <a href='" + resetLink + "'>clicking here</a>.");
    }

    public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        logger.LogDebug("Sending reset password code to user: {0}, resetCode: {1}", user.UserName, resetCode);
        // ISSUE: reference to a compiler-generated field
        return emailSender.QueueAsync(email, "Reset your password", "Please reset your password using the following code: " + resetCode);
    }
}