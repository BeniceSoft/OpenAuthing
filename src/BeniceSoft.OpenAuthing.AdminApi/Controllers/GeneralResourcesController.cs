using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Models.GeneralResources;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 常规资源
/// </summary>
public class GeneralResourcesController : AuthingApiControllerBase
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseResult<Guid>>(StatusCodes.Status200OK)]
    public async Task<Guid> PostAsync(CreateGeneralResourceReq req)
    {
        return Guid.Empty;
    }
}