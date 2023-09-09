using BeniceSoft.OpenAuthing.Dtos.IdPs.Requests;
using BeniceSoft.OpenAuthing.Dtos.IdPs.Responses;
using BeniceSoft.OpenAuthing.DynamicAuth;
using BeniceSoft.OpenAuthing.IdentityProviders;
using BeniceSoft.OpenAuthing.IdentityProviderTemplates;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 身份提供程序
/// </summary>
public class IdPsController : AuthingApiControllerBase
{
    private readonly IDynamicAuthenticationManager _dynamicAuthenticationManager;
    private readonly IRepository<ExternalIdentityProvider, Guid> _idPRepository;
    private readonly IRepository<ExternalIdentityProviderTemplate, Guid> _idPTemplateRepository;

    public IdPsController(IDynamicAuthenticationManager dynamicAuthenticationManager, IRepository<ExternalIdentityProvider, Guid> idPRepository,
        IRepository<ExternalIdentityProviderTemplate, Guid> idPTemplateRepository)
    {
        _dynamicAuthenticationManager = dynamicAuthenticationManager;
        _idPRepository = idPRepository;
        _idPTemplateRepository = idPTemplateRepository;
    }

    /// <summary>
    /// 获取列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<List<ExternalIdentityProviderSimpleRes>> GetAsync(string? searchKey = null)
    {
        var idPs = await _idPRepository.GetQueryableAsync();
        var idPTemplates = await _idPTemplateRepository.GetQueryableAsync();

        var queryable =
            from idP in idPs
            join idPTemplate in idPTemplates on idP.ProviderName equals idPTemplate.Name
            select new ExternalIdentityProviderSimpleRes
            {
                Id = idP.Id, Name = idP.Name, DisplayName = idP.DisplayName, ProviderName = idP.ProviderName, Enabled = idP.Enabled,
                Logo = idPTemplate.Logo, Title = idPTemplate.Title, CreationTime = idP.CreationTime
            };

        return await QueryableWrapperFactory.CreateWrapper(queryable)
            .SearchByKey(searchKey, x => x.DisplayName, x => x.Name)
            .OrderByDescending(x => x.CreationTime)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<Guid> PostAsync([FromBody] CreateIdPReq req)
    {
        var options = req.Options
            .Select(x => new ExternalIdentityProviderOption(GuidGenerator.Create(), x.Key, x.Value))
            .ToList();
        var idp = new ExternalIdentityProvider(GuidGenerator.Create(), req.ProviderName, req.Name, req.DisplayName, options);
        await _idPRepository.InsertAsync(idp);

        _dynamicAuthenticationManager.Add(req.ProviderName, req.Name, req.DisplayName, req.Options);

        return idp.Id;
    }

    /// <summary>
    /// 切换启用状态
    /// </summary>
    /// <param name="id"></param>
    /// <param name="enabled"></param>
    /// <returns></returns>
    [HttpPut("{id}/toggle-enabled")]
    public async Task<bool> ToggleEnabledAsync(Guid id, bool enabled)
    {
        throw new NotImplementedException();
    }
}