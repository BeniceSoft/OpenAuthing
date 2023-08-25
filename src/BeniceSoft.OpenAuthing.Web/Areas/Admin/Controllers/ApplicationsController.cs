using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Requests;
using BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;
using BeniceSoft.OpenAuthing.OpenIddict.Applications;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

public class ApplicationsController : AdminControllerBase
{
    private readonly IRepository<OpenIddictApplication, Guid> _applicationRepository;
    private readonly IAmApplicationManager _applicationManager;

    public ApplicationsController(IRepository<OpenIddictApplication, Guid> applicationRepository,
        IAmApplicationManager applicationManager)
    {
        _applicationRepository = applicationRepository;
        _applicationManager = applicationManager;
    }

    [HttpGet]
    public async Task<List<QueryApplicationRes>> GetAsync(string? searchKey = null)
    {
        var queryable = await _applicationRepository.GetQueryableAsync();
        var application = await AsyncExecuter.ToListAsync(queryable
            .WhereIf(!string.IsNullOrWhiteSpace(searchKey), x => x.DisplayName!.Contains(searchKey!))
            .OrderByDescending(x => x.CreationTime));

        return application.Adapt<List<QueryApplicationRes>>();
    }

    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateApplicationReq req)
    {
        var applicationDescriptor = new AmApplicationDescriptor
        {
            ClientId = req.ClientId,
            DisplayName = req.DisplayName,
            ClientType = req.ClientType
        };
        var application = (OpenIddictApplicationModel)await _applicationManager.CreateAsync(applicationDescriptor);

        return application.Id;
    }
}