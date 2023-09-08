using BeniceSoft.OpenAuthing.Areas.Admin.Models.GeneralResources;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 常规资源
/// </summary>
public class GeneralResourcesController : AdminControllerBase
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync(CreateGeneralResourceReq req)
    {
        return Guid.Empty;
    }
}