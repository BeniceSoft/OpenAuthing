using BeniceSoft.OpenAuthing.Commands.PermissionSpaces;
using BeniceSoft.OpenAuthing.Dtos.PermissionSpaces;
using BeniceSoft.OpenAuthing.Models.PermissionSpaces;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

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
        return await _queries.ListAllAsync(req);
    }

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GetPermissionSpaceRes> GetAsync(Guid id)
    {
        return await _queries.GetDetailAsync(id);
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