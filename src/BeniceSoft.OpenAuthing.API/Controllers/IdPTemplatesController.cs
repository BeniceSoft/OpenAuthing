using BeniceSoft.OpenAuthing.Dtos.IdPTemplates.Responses;
using BeniceSoft.OpenAuthing.Entities.IdentityProviderTemplates;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Controllers;

/// <summary>
/// 身份提供程序模板
/// </summary>
public class IdPTemplatesController : AuthingApiControllerBase
{
    private readonly IRepository<ExternalIdentityProviderTemplate, Guid> _idpTemplateRepository;

    public IdPTemplatesController(IRepository<ExternalIdentityProviderTemplate, Guid> idpTemplateRepository)
    {
        _idpTemplateRepository = idpTemplateRepository;
    }

    [HttpGet]
    public async Task<List<IdPTemplateSimpleRes>> GetAsync()
    {
        var idPTemplates = await _idpTemplateRepository.GetQueryableAsync();
        return await idPTemplates
            .Select(x => new IdPTemplateSimpleRes
            {
                Name = x.Name,
                Title = x.Title,
                Description = x.Description,
                Logo = x.Logo
            })
            .ToListAsync();
    }

    [HttpGet("{name}")]
    public async Task<IdPTemplateRes> GetAsync(string name)
    {
        var idPTemplates = await _idpTemplateRepository.WithDetailsAsync(x => x.Fields.OrderBy(y => y.Order));
        var idPTemplate = await idPTemplates.Where(x => x.Name == name)
            .FirstOrDefaultAsync();

        if (idPTemplate is null)
        {
            throw new EntityNotFoundException();
        }

        return idPTemplate.Adapt<IdPTemplateRes>();
    }
}