using BeniceSoft.OpenAuthing.Dtos.DataResources;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 数据资源
/// </summary>
public class DataResourcesController : AdminControllerBase
{
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<PagedDataResourceResponseDto>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        return new();
    }
}