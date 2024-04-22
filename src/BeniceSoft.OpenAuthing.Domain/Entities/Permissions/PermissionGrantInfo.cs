namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class PermissionGrantInfo
{
    public static PermissionGrantInfo NonGranted { get; } = new(false);

    public virtual bool IsGranted { get; }

    public virtual string? ProviderKey { get; }

    public PermissionGrantInfo(bool isGranted, string? providerKey = null)
    {
        IsGranted = isGranted;
        ProviderKey = providerKey;
    }
}