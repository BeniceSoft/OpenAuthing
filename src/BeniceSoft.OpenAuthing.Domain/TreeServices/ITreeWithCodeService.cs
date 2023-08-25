using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.TreeServices;

public interface ITreeWithCodeService<T, TKey>
    : ITreeService<T, TKey>
    where T : class, ITreeWithCode<TKey>, IEntity<TKey>
    where TKey : struct
{

    /// <summary>
    /// 校验code
    /// </summary>
    /// <param name="entity"></param>
    Task ValidateCodeAsync(T entity);
}