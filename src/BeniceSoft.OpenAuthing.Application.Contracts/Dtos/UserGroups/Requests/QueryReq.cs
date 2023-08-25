namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests
{
    /// <summary>
    /// 分页获取
    /// </summary>
    public class QueryReq : BaseQueryReq
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid? RoleId { get; set; }
 
        /// <summary>
        /// 禁用 启用	
        /// </summary>
        public bool? IsEnabled { get; set; }
    }
}
