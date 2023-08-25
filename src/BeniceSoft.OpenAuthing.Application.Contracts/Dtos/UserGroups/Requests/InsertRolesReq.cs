namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

/// <summary>
/// 
/// </summary>
public class InsertRolesReq
{
    /// <summary>
    /// 
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new List<Guid>();
}