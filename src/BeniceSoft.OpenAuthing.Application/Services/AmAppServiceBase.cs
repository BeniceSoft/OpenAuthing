using BeniceSoft.OpenAuthing.Localization;
using LinkMore.Abp.Ddd.Application;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace BeniceSoft.OpenAuthing.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public abstract class AmAppServiceBase : LinkMoreApplicationService
{
    protected AmAppServiceBase()
    {
        LocalizationResource = typeof(AMResource);
    }
}