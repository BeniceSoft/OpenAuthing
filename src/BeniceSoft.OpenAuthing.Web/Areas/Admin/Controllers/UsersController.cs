using BeniceSoft.OpenAuthing.Areas.Admin.Models;
using BeniceSoft.OpenAuthing.Areas.Admin.Models.Users;
using BeniceSoft.OpenAuthing.Commands.Users;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 用户
/// </summary>
public class UsersController : AdminControllerBase
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
    public async Task<UserDetailRes> GetAsync(Guid id)
    {
        return await _userQueries.GetDetailAsync(id);
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateUserCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}/avatar")]
    public async Task<bool> UploadUserAvatarAsync(Guid id, [FromForm] UpdateUserAvatarReq req)
    {
        var user = await UserRepository.GetAsync(id);

        await using var stream = req.File.OpenReadStream();
        var fileName = Clock.Now.ToString("yyyyMMddHHmmss") + ".jpg";
        var fullFileName = $"avatars/{user.Id}/{fileName}";
        await BlobContainer.SaveAsync(fullFileName, stream);

        var avatar = $"/uploadFiles/host/default/{fullFileName}";
        user.UpdateAvatar(avatar);

        await UserManager.UpdateAsync(user);

        return true;
    }

    /// <summary>
    /// 获取用户所属部门列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/departments")]
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
    public async Task<List<UserRoleRes>> GetUserRolesAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}