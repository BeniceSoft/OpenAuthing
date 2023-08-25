using BeniceSoft.Abp.Ddd.Application;
using BeniceSoft.OpenAuthing.Localization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace BeniceSoft.OpenAuthing.Services;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public abstract class AmAppServiceBase : BeniceSoftApplicationService
{
    protected AmAppServiceBase()
    {
        LocalizationResource = typeof(AMResource);
    }
}