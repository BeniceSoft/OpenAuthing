namespace BeniceSoft.OpenAuthing.Dtos.UserGroups.Requests;

public class GetListReq
{
    /// <summary>
    /// 启用/禁用
    /// </summary>
    public bool? IsEnabled { get; set; }

    public int? MaxResultCount { get; set; } = 100;

    public string? SearchKey { get; set; }
}