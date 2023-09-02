using BeniceSoft.OpenAuthing.Areas.Admin.Models.PermissionSpaces;
using BeniceSoft.OpenAuthing.Commands.PermissionSpaces;
using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 权限空间
/// </summary>
public class PermissionSpacesController : AdminControllerBase
{
    private readonly IPermissionSpaceQueries _queries;

    public PermissionSpacesController(IPermissionSpaceQueries queries)
    {
        _queries = queries;
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedResultDto<PagedPermissionSpaceRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var req = new PagedPermissionSpaceReq
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchKey = searchKey
        };
        return await _queries.PagedListAsync(req);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync(CreatePermissionSpaceReq req)
    {
        var command = new CreatePermissionSpaceCommand(req.Name, req.DisplayName, req.Description);
        return await Mediator.Send(command);
    }
}