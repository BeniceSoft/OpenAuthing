using BeniceSoft.OpenAuthing.Commands.Applications;
using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Requests;
using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 应用
/// </summary>
public class ApplicationsController : AdminControllerBase
{
    private readonly IApplicationQueries _applicationQueries;

    public ApplicationsController(IApplicationQueries applicationQueries)
    {
        _applicationQueries = applicationQueries;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <param name="searchKey"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<QueryApplicationRes>> GetAsync(string? searchKey = null)
    {
        return await _applicationQueries.ListQueryAsync(searchKey);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateApplicationReq req)
    {
        var command = new CreateApplicationCommand(req.ClientId, req.DisplayName, req.ClientType);
        return await Mediator.Send(command);
    }
}