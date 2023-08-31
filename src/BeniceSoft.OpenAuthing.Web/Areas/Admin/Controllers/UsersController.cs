using BeniceSoft.OpenAuthing.Areas.Admin.Models.Users;
using BeniceSoft.OpenAuthing.Dtos.DepartmentMembers;
using BeniceSoft.OpenAuthing.Extensions;
using BeniceSoft.OpenAuthing.Misc;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

/// <summary>
/// 用户
/// </summary>
public class UsersController : AdminControllerBase
{

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
        var departmentMembers = await DepartmentMemberRepository.GetQueryableAsync();
        var queryable = await UserRepository.GetQueryableAsync();
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(searchKey, x => x.UserName, x => x.PhoneNumber, x => x.Nickname)
            .WhereIf(excludeDepartmentId.HasValue, x => departmentMembers
                .Where(y => y.DepartmentId == excludeDepartmentId)
                .All(y => y.UserId != x.Id))
            .WhereIf(onlyEnabled, x => x.Enabled)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<UserPagedRes>();
        if (totalCount > 0)
        {
            var users = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PagedBy(pageIndex, pageSize)
                .ToListAsync();
            
            items = users.Select(x =>
            {
                var temp = x.Adapt<UserPagedRes>();
                temp.PhoneNumber = x.PhoneNumber?.Mask(3, 4);
                return temp;
            }).ToList();
        }

        return new(totalCount, items);
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<UserDetailRes> GetAsync(Guid id)
    {
        var user = await UserRepository.GetAsync(id);

        return user.Adapt<UserDetailRes>();
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateUserReq req)
    {
        var user = new User(GuidGenerator.Create(), req.UserName, req.PhoneNumber, req.PhoneNumberConfirmed);
        var result = await UserManager.CreateAsync(user, req.Password);
        ThrowIfIdentityFailedResult(result);

        return user.Id;
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    /// <param name="id"></param>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPut("{id}/avatar")]
    public async Task<bool> UploadUserAvatarAsync(Guid id, [FromForm] UploadUserAvatarReq req)
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
        var departments = await DepartmentRepository.GetQueryableAsync();
        var departmentMembers = await DepartmentMemberRepository.GetQueryableAsync();
        var queryable =
            from departmentMember in departmentMembers
            join department in departments on departmentMember.DepartmentId equals department.Id
            where departmentMember.UserId == id
            orderby departmentMember.IsMain descending, departmentMember.IsLeader descending 
            select new UserDepartmentDto
            {
                DepartmentId = department.Id,
                DepartmentName = department.Name,
                IsMain = departmentMember.IsMain,
                IsLeader = departmentMember.IsLeader
            };
        return await QueryableWrapperFactory.CreateWrapper(queryable)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 获取用户拥有的角色列表
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/roles")]
    public async Task<List<UserRoleRes>> GetUserRolesAsync(Guid id)
    {
        var roles = await RoleRepository.GetUserRolesAsync(id);
        throw new NotImplementedException();
    }
}