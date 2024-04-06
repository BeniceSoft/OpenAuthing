namespace BeniceSoft.OpenAuthing.OpenIddictExtensions;

public class AmOpenIddictExtensionGrantsOptions
{
    public Dictionary<string, IExtensionGrantHandler> GrantHandlers { get; }

    public AmOpenIddictExtensionGrantsOptions()
    {
        GrantHandlers = new();
    }

    public THandler? Find<THandler>(string name) where THandler : IExtensionGrantHandler
    {
        if (!GrantHandlers.ContainsKey(name))
        {
            return default;
        }

        return (THandler)GrantHandlers[name];
    }
}