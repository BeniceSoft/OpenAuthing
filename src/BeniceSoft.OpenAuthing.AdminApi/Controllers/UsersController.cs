using BeniceSoft.Abp.Core.Models;
using BeniceSoft.OpenAuthing.Commands.Users;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Dtos.Users;
using BeniceSoft.OpenAuthing.Models.Users;
using BeniceSoft.OpenAuthing.Queries;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 用户
/// </summary>
public class UsersController : AuthingApiControllerBase
{
    private readonly IUserQueries _userQueries;

    public UsersController(IUserQueries userQueries)
    {
        _userQueries = userQueries;
    }

    /// <summary>
    /// 获取用户列表
    /// </summary>
    /// <param name="searchKey"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <param name="excludeDepartmentId"></param>
    /// <param name="onlyEnabled"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType<ResponseResult<PagedResultDto<UserPagedRes>>>(StatusCodes.Status200OK)]
    public async Task<PagedResultDto<UserPagedRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20,
        Guid? excludeDepartmentId = null, bool onlyEnabled = false)
    {
        var req = new UserPagedReq
        {
            SearchKey = searchKey,
            PageIndex = pageIndex,
            PageSize = pageSize,
            ExcludeDepartmentId = excludeDepartmentId,
            OnlyEnabled = onlyEnabled
        };
        return await _userQueries.PagedQueryAsync(req);
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType<ResponseResult<UserDetailRes>>(StatusCodes.Status200OK)]
    public async Task<UserDetailRes> GetAsync(Guid id)
    {
        return await _userQueries.GetDetailAsync(id);
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType<ResponseResult<Guid>>(StatusCodes.Status200OK)]
    public async Task<Guid> PostAsync([FromBody] CreateUserReq req)
    {
        var command = new CreateUserCommand(req.UserName, req.PhoneNumber, req.Password, req.PhoneNumberConfirmed);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}/avatar")]
    [ProducesResponseType<ResponseResult<bool>>(StatusCodes.Status200OK)]
    public async Task<bool> UploadUserAvatarAsync(Guid id, [FromForm] UpdateUserAvatarReq req)
    {
        await using var stream = req.File.OpenReadStream();
        var fileName = GuidGenerator.Create().ToString("N") + ".jpg";
        var fullFileName = $"avatars/{fileName}";
        await BlobContainer.SaveAsync(fullFileName, stream);

        var avatarFileUrl = $"/uploadFiles/host/default/{fullFileName}";

        var command = new UpdateUserAvatarCommand(id, avatarFileUrl);
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 获取用户所属部门列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/departments")]
    [ProducesResponseType<ResponseResult<List<UserDepartmentDto>>>(StatusCodes.Status200OK)]
    public async Task<List<UserDepartmentDto>> GetUserDepartmentsAsync(Guid id)
    {
        return await _userQueries.ListUserDepartmentsAsync(id);
    }

    /// <summary>
    /// 获取用户拥有的角色列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/roles")]
    [ProducesResponseType<ResponseResult<List<UserRoleRes>>>(StatusCodes.Status200OK)]
    public async Task<List<UserRoleRes>> GetUserRolesAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}