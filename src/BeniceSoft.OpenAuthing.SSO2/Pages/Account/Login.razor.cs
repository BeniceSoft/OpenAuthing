using BeniceSoft.OpenAuthing.Entities.Users;
using BeniceSoft.OpenAuthing.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;

namespace BeniceSoft.OpenAuthing.Pages.Account;

public partial class Login
{
    [Inject] private UserManager UserManager { get; set; } = null!;
    [Inject] private SignInManager<User> SignInManager { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILoggedInUserTemporaryStore LoggedInUserTemporaryStore { get; set; } = null!;

    private EditContext? editContext;
    private ValidationMessageStore? messageStore;
    private LoginModel loginModel = new();
    
    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }
    

    protected override void OnInitialized()
    {
        editContext = new(loginModel);
        messageStore = new(editContext);
    }

    private async Task HandleSubmit()
    {
        messageStore?.Clear();

        if (editContext?.Validate() != true)
        {
            return;
        }

        var user = await UserManager.FindByNameAsync(loginModel.Username);
        if (user is not null)
        {
            if (!await SignInManager.CanSignInAsync(user))
            {
                messageStore?.Add(() => loginModel, "Your account is blocked");
                editContext.NotifyValidationStateChanged();

                return;
            }

            var result = await SignInManager.CheckPasswordSignInAsync(user, loginModel.Password, true);
            if (result.Succeeded)
            {
                var token = await LoggedInUserTemporaryStore.AddAsync(new(user, loginModel.Password, loginModel.RememberMe, ReturnUrl));
                NavigationManager.NavigateTo($"/account/login?token={token}", true);

                return;
            }
        }

        messageStore?.Add(() => loginModel, "Login failed. Please check the username and password.");
        editContext.NotifyValidationStateChanged();
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}