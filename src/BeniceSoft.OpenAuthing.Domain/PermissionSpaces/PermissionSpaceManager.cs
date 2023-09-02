using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace BeniceSoft.OpenAuthing.PermissionSpaces;

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

    public async Task<Guid> CreateAsync(string name, string displayName, string description, bool isSystemBuiltIn = false)
    {
        await ThrowIfDuplicateNameAsync(name);

        var space = new PermissionSpace(_guidGenerator.Create(), name, displayName, description, isSystemBuiltIn);
        space.SetNormalizedName(_normalizer.NormalizeName(name));
        await _repository.InsertAsync(space);

        return space.Id;
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