using BeniceSoft.OpenAuthing.Commands.DepartmentMembers;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

public partial class DepartmentsController
{
    private readonly IDepartmentMemberQueries _departmentMemberQueries;

    /// <summary>
    /// 获取部门成员列表
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="onlyDirectUsers"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet("{departmentId}/members")]
    public async Task<PagedResultDto<QueryDepartmentMembersRes>> GetMemberAsync(Guid departmentId, bool onlyDirectUsers = false, int pageIndex = 1,
        int pageSize = 20)
    {
        return await _departmentMemberQueries.QueryDepartmentMembersAsync(departmentId, new()
        {
            OnlyDirectUsers = onlyDirectUsers,
            SkipCount = (pageIndex - 1) * pageSize,
            MaxResultCount = pageSize
        });
    }

    /// <summary>
    /// 添加部门成员
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost("{departmentId}/members")]
    public async Task<int> PostMemberAsync(Guid departmentId, [FromBody] AddDepartmentMembersReq req)
    {
        var command = new AddDepartmentMembersCommand
        {
            DepartmentId = departmentId,
            UserIds = req.UserIds
        };
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 设置/取消部门成员负责人
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="userId"></param>
    /// <param name="isLeader"></param>
    /// <returns></returns>
    [HttpPut("{departmentId}/members/{userId}/leader")]
    public async Task<bool> SetLeaderAsync(Guid departmentId, Guid userId, [FromQuery] bool isLeader)
    {
        var command = new SetLeaderCommand
        {
            DepartmentId = departmentId,
            UserId = userId,
            IsLeader = isLeader
        };
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 设置/取消部门成员主部门
    /// </summary>
    /// <param name="departmentId"></param>
    /// <param name="userId"></param>
    /// <param name="isMain"></param>
    /// <returns></returns>
    [HttpPut("{departmentId}/members/{userId}/main")]
    public async Task<bool> SetMainAsync(Guid departmentId, Guid userId, [FromQuery] bool isMain)
    {
        var command = new SetMainDepartmentCommand
        {
            DepartmentId = departmentId,
            UserId = userId,
            IsMain = isMain
        };

        return await Mediator.Send(command);
    }
}