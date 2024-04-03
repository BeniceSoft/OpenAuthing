using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.Entities.PermissionSpaces;

public class PermissionSpaceManager : IScopedDependency
{
    private readonly ILookupNormalizer _normalizer;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IRepository<PermissionSpace, Guid> _repository;

    public PermissionSpaceManager(ILookupNormalizer normalizer, IGuidGenerator guidGenerator, IRepository<PermissionSpace, Guid> repository)
    {
        _normalizer = normalizer;
        _guidGenerator = guidGenerator;
        _repository = repository;
    }

    public async Task<PermissionSpace?> FindByNameAsync(string name)
    {
        return await _repository.FirstOrDefaultAsync(x => x.NormalizedName == _normalizer.NormalizeName(name));
    }

    public async Task<Guid> CreateAsync(PermissionSpace permissionSpace)
    {
        await ThrowIfDuplicateNameAsync(permissionSpace.Name);

        permissionSpace.SetNormalizedName(_normalizer.NormalizeName(permissionSpace.Name));
        await _repository.InsertAsync(permissionSpace);

        return permissionSpace.Id;
    }

    public async Task<Guid> CreateAsync(string name, string displayName, string description, bool isSystemBuiltIn = false)
    {
        var space = new PermissionSpace(_guidGenerator.Create(), name, displayName, description, isSystemBuiltIn);
        return await CreateAsync(space);
    }

    private async Task ThrowIfDuplicateNameAsync(string name, Guid? excludeId = null)
    {
        var normalizedName = _normalizer.NormalizeName(name);
        Expression<Func<PermissionSpace, bool>> predicate = x => x.NormalizedName == normalizedName;
        if (excludeId.HasValue)
        {
            predicate = predicate.And(x => x.Id != excludeId);
        }

        if (await _repository.AnyAsync(predicate))
        {
            throw new UserFriendlyException($"已存在名为「{name}」的权限空间");
        }
    }
}