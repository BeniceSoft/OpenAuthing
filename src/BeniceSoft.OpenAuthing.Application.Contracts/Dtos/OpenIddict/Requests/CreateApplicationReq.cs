using System.ComponentModel.DataAnnotations;

namespace BeniceSoft.OpenAuthing.Dtos.OpenIddict.Requests;

public class CreateApplicationReq
{
    /// <summary>
    /// 应用标识
    /// </summary>
    [Required]
    public string ClientId { get; set; }

    /// <summary>
    /// 显示名称
    /// </summary>
    [Required]
    public string DisplayName { get; set; }

    /// <summary>
    /// 应用类型
    /// </summary>
    [Required]
    public string ClientType { get; set; }
}