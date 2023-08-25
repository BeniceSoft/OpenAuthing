using BeniceSoft.OpenAuthing.DepartmentMembers;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.OpenAuthing.Users;
using LinkMore.Abp.Ddd.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Linq;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

[ApiController]
[Area("Admin")]
[Route("api/[area]/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = AmConstants.AdminRoleName)]
public abstract class AdminControllerBase : AbpController
{
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    protected IQueryableWrapperFactory QueryableWrapperFactory => LazyServiceProvider.LazyGetRequiredService<IQueryableWrapperFactory>();

    protected UserManager UserManager => LazyServiceProvider.LazyGetRequiredService<UserManager>();

    protected IBlobContainer BlobContainer => LazyServiceProvider.LazyGetRequiredService<IBlobContainer>();
    
    protected IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();
    
    protected IRoleRepository RoleRepository => LazyServiceProvider.LazyGetRequiredService<IRoleRepository>();
    
    protected IRepository<Department, Guid> DepartmentRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<Department, Guid>>();
    
    protected IRepository<DepartmentMember> DepartmentMemberRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<DepartmentMember>>();

    protected IRepository<UserGroup, Guid> UserGroupRepository => LazyServiceProvider.LazyGetRequiredService<IRepository<UserGroup, Guid>>();
    
    protected void ThrowIfIdentityFailedResult(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new UserFriendlyException(identityResult.Errors.Select(x => x.Description).JoinAsString(";"));
        }
    }
}