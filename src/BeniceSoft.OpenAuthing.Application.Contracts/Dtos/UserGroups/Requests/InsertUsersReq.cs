namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

/// <summary>
/// 
/// </summary>
public class InsertUsersReq
{
    ///// <summary>
    ///// 
    ///// </summary>
    //public Guid Id { get; set; }

    /// <summary>
    /// 用户ID合集
    /// </summary>
    public List<Guid> UserIds { get; set; } = new List<Guid>();
}