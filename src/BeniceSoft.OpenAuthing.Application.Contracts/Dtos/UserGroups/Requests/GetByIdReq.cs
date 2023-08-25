namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests
{
    /// <summary>
    /// 根据用户Id列表获取所有用户信息请求参数类
    /// </summary>
    public class GetByIdReq
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
    }
}
