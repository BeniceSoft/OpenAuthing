using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Applications;
using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Requests;
using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 应用
/// </summary>
public class ApplicationsController : AuthingApiControllerBase
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
    [ProducesResponseType<ResponseResult<List<QueryApplicationRes>>>(StatusCodes.Status200OK)]
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
    [ProducesResponseType<ResponseResult<Guid>>(StatusCodes.Status200OK)]
    public async Task<Guid> PostAsync([FromBody] CreateApplicationReq req)
    {
        var command = new CreateApplicationCommand(req.ClientId, req.DisplayName, req.ClientType);
        return await Mediator.Send(command);
    }
}