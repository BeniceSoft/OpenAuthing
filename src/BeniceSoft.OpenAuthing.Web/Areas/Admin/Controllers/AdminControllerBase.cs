using BeniceSoft.OpenAuthing.DepartmentMembers;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Roles;
using BeniceSoft.OpenAuthing.UserGroups;
using BeniceSoft.OpenAuthing.Users;
using BeniceSoft.Abp.Ddd.Domain;
using MediatR;
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
    
    protected IMediator Mediator => LazyServiceProvider.LazyGetRequiredService<IMediator>();
}