using BeniceSoft.OpenAuthing.Commands.PermissionSpaces;
using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using BeniceSoft.OpenAuthing.Models.PermissionSpaces;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 权限空间
/// </summary>
public partial class PermissionSpacesController : AuthingApiControllerBase
{
    private IPermissionSpaceQueries PermissionSpaceQueries => LazyServiceProvider.LazyGetRequiredService<IPermissionSpaceQueries>();


    /// <summary>
    /// 列表查询
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ListPermissionSpaceRes>> GetAsync(string? searchKey = null)
    {
        var req = new ListPermissionSpaceReq
        {
            SearchKey = searchKey
        };
        return await PermissionSpaceQueries.ListAllAsync(req);
    }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GetPermissionSpaceRes> GetAsync(Guid id)
    {
        return await PermissionSpaceQueries.GetDetailAsync(id);
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