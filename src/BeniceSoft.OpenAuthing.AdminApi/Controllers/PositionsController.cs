using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Positions;
using BeniceSoft.OpenAuthing.Dtos.Positions;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// Positions
/// </summary>
public class PositionsController : AuthingApiControllerBase
{
    private IPositionQueries PositionQueries => LazyServiceProvider.LazyGetRequiredService<IPositionQueries>();

    /// <summary>
    /// Get positions (Pagination)
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseResult<PagedResultDto<PositionRes>>>(StatusCodes.Status200OK)]
    public async Task<PagedResultDto<PositionRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var req = new PositionPagedReq
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            SearchKey = searchKey
        };
        return await PositionQueries.PagedQueryAsync(req);
    }

    /// <summary>
    /// Create a new position
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseResult<Guid>>(StatusCodes.Status200OK)]
    public async Task<Guid> PostAsync([FromBody] InputPositionReq req)
    {
        var command = new CreatePositionCommand(req.Name, req.Description);
        return await Mediator.Send(command);
    }
    
    /// <summary>
    /// Delete a position
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public async Task<bool> DeleteAsync(Guid id)
    {
        var command = new DeletePositionCommand(id);
        return await Mediator.Send(command);
    }
}