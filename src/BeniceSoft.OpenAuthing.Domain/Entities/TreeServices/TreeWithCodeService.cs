using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.Entities.TreeServices;

public class TreeWithCodeService<T, TKey> : TreeService<T, TKey>
    , ITreeWithCodeService<T, TKey>
    where TKey : struct
    where T : class, ITreeWithCode<TKey>, IEntity<TKey>
{
    public TreeWithCodeService(IRepository<T, TKey> repository, TreeServiceOption treeServiceOption = null) : base(
        repository, treeServiceOption)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    public virtual async Task ValidateCodeAsync(T entity)
    {
        if (TreeServiceOption.IsIgnoreCodeExist)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(entity.Code))
        {
            return;
        }

        var siblings = (await FindChildrenAsync(entity.ParentId))
            .Where(ou => !ou.Id.Equals(entity.Id))
            .ToList();

        if (siblings.Any(ou => ou.Code == entity.Code))
        {
            throw new UserFriendlyException("已经存在相同的Code")
                .WithData("0", entity.Code);
        }
    }
}