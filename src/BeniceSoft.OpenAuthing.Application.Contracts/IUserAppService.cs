using Volo.Abp.Application.Services;

namespace BeniceSoft.OpenAuthing;

public interface IUserAppService : IApplicationService
{
    // /// <summary>
    // /// 获取用户拥有的角色集合
    // /// </summary>
    // /// <param name="id"></param>
    // /// <returns></returns>
    // Task<List<UserRoleRes>> GetUserRolesAsync(Guid id);
    //
    // /// <summary>
    // /// 获取拥有角色的用户列表
    // /// </summary>
    // /// <param name="req"></param>
    // /// <returns></returns>
    // Task<List<SimpleAmUserDto>> QueryInRoleUsersAsync(QueryByIdsReq req);
    //
    // /// <summary>
    // /// 获取在用户组内的用户列表
    // /// </summary>
    // /// <param name="req"></param>
    // /// <returns></returns>
    // Task<List<SimpleAmUserDto>> QueryInUserGroupUsersAsync(QueryByIdsReq req);
    //
    // /// <summary>
    // /// 查询在部门内的用户列表
    // /// </summary>
    // /// <param name="req"></param>
    // /// <returns></returns>
    // Task<List<SimpleAmUserDto>> QueryInDepartmentUsersAsync(QueryByIdsReq req);
}