using BeniceSoft.OpenAuthing.Entities.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.Components.Pages.Account;

public partial class Login
{
    [Inject] public IClock Clock { get; set; } = null!;
    [Inject] public UserManager UserManager { get; set; } = null!;
    [Inject] public SignInManager<User> SignInManager { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IUnitOfWorkManager UnitOfWorkManager { get; set; } = null!;

    [SupplyParameterFromQuery] public string? ReturnUrl { get; set; }
    [SupplyParameterFromForm] public LoginModel? Model { get; set; }

    private EditContext? _editContext;
    private ValidationMessageStore? _messageStore;

    protected override void OnInitialized()
    {
        Model ??= new();
        _editContext = new(Model);
        _messageStore = new(_editContext);
    }

    private async Task HandleSubmit()
    {
        using var uow = UnitOfWorkManager.Begin();
        _messageStore?.Clear();

        if (_editContext?.Validate() != true)
        {
            return;
        }


        var result = await SignInManager.PasswordSignInAsync(Model!.Username, Model.Password, Model.RememberMe, true);
        if (result.Succeeded)
        {
            NavigationManager.NavigateTo(ReturnUrl ?? "/");

            return;
        }

        if (result.RequiresTwoFactor)
        {
            NavigationManager.NavigateTo($"/account/loginwith2fa?returnUrl={ReturnUrl}");
            
            return;
        }


        _messageStore?.Add(() => Model.Username, "Login failed. Please check the username and password.");
        _editContext.NotifyValidationStateChanged();
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}