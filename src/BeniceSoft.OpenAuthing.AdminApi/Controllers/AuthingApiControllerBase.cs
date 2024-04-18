using BeniceSoft.Abp.Ddd.Domain;
using BeniceSoft.OpenAuthing.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.BlobStoring;
using Volo.Abp.Linq;

namespace BeniceSoft.OpenAuthing.Controllers;

[ApiController]
[Route("api/admin/[controller]")]
[Authorize(Roles = AuthingConstants.AdminRoleName)]
public abstract class AuthingApiControllerBase : AbpController
{
    protected IAsyncQueryableExecuter AsyncExecuter => LazyServiceProvider.LazyGetRequiredService<IAsyncQueryableExecuter>();

    protected IQueryableWrapperFactory QueryableWrapperFactory => LazyServiceProvider.LazyGetRequiredService<IQueryableWrapperFactory>();

    protected UserManager UserManager => LazyServiceProvider.LazyGetRequiredService<UserManager>();

    protected IBlobContainer BlobContainer => LazyServiceProvider.LazyGetRequiredService<IBlobContainer>();

    protected IUserRepository UserRepository => LazyServiceProvider.LazyGetRequiredService<IUserRepository>();

    protected IMediator Mediator => LazyServiceProvider.LazyGetRequiredService<IMediator>();
}