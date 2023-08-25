using BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.OpenAuthing.Users;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

public partial class RolesController : AdminControllerBase
{
    private readonly IRepository<Role, Guid> _roleRepository;
    private readonly IRepository<RoleSubject, Guid> _roleSubjectRepository;
    private readonly IRepository<User, Guid> _userRepository;
    private readonly IRepository<UserGroup, Guid> _userGroupRepository;
    private readonly RoleManager _roleManager;

    public RolesController(IRepository<Role, Guid> roleRepository, RoleManager roleManager, IRepository<RoleSubject, Guid> roleSubjectRepository,
        IRepository<User, Guid> userRepository, IRepository<UserGroup, Guid> userGroupRepository)
    {
        _roleRepository = roleRepository;
        _roleManager = roleManager;
        _roleSubjectRepository = roleSubjectRepository;
        _userRepository = userRepository;
        _userGroupRepository = userGroupRepository;
    }

    [HttpGet]
    public async Task<PagedResultDto<RoleSimpleRes>> GetAsync(string? searchKey = null, int pageIndex = 1, int pageSize = 20)
    {
        var queryable = await _roleRepository.GetQueryableAsync();
        var queryableWrapper = QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(searchKey, x => x.Name, x => x.DisplayName)
            .AsNoTracking();

        var totalCount = await queryableWrapper.CountAsync();
        var items = new List<RoleSimpleRes>();
        if (totalCount > 0)
        {
            var roles = await queryableWrapper
                .OrderByDescending(x => x.CreationTime)
                .PageBy((pageIndex - 1) * pageSize, pageSize)
                .ToListAsync();

            items = roles.Adapt<List<RoleSimpleRes>>();
        }

        return new(totalCount, items);
    }

    [HttpGet("{id}")]
    public async Task<RoleDetailRes> GetAsync(Guid id)
    {
        var role = await _roleRepository.GetAsync(id, false);

        return role.Adapt<RoleDetailRes>();
    }

    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] InputRoleReq req)
    {
        var role = new Role(GuidGenerator.Create(), req.Name, req.DisplayName, req.Description);
        var result = await _roleManager.CreateAsync(role);
        ThrowIfIdentityFailedResult(result);
        return role.Id;
    }

    [HttpPut("{id}")]
    public async Task<bool> PusAsync(Guid id, [FromBody] InputRoleReq req)
    {
        var role = await _roleRepository.GetAsync(id);
        role.Update(req.Name, req.DisplayName, req.Description);

        var result = await _roleManager.UpdateAsync(role);

        ThrowIfIdentityFailedResult(result);

        return true;
    }

    [HttpDelete("{id}")]
    public async Task<bool> DeleteAsync(Guid id)
    {
        var role = await _roleRepository.GetAsync(id);
        if (role.IsSystemBuiltIn)
        {
            throw new UserFriendlyException("系统内置角色无法删除");
        }

        await _roleManager.DeleteAsync(role);
        return true;
    }

    [HttpPut("{id}/toggle-enabled")]
    public async Task<bool> ToggleEnabled(Guid id, bool enabled)
    {
        var role = await _roleRepository.GetAsync(id);
        role.ToggleEnabled(enabled);

        await _roleRepository.UpdateAsync(role);
        return true;
    }
}