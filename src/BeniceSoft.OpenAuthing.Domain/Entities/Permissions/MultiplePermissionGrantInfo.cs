using Volo.Abp;

namespace BeniceSoft.OpenAuthing.Entities.Permissions;

public class MultiplePermissionGrantInfo
{
    public Dictionary<string, PermissionGrantInfo> Result { get; }

    public MultiplePermissionGrantInfo()
    {
        Result = new Dictionary<string, PermissionGrantInfo>();
    }

    public MultiplePermissionGrantInfo(string[] names)
    {
        Check.NotNull(names, nameof(names));

        Result = new Dictionary<string, PermissionGrantInfo>();

        foreach (var name in names)
        {
            Result.Add(name, PermissionGrantInfo.NonGranted);
        }
    }
}