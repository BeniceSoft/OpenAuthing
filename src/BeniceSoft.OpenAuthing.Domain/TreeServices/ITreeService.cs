using Volo.Abp.Domain.Entities;

namespace BeniceSoft.OpenAuthing.TreeServices;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface ITreeService<T, TKey>
    where T : class, ITree<TKey>, IEntity<TKey>
    where TKey : struct
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<string> GetNextChildCodeAsync(TKey? parentId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<T?> GetLastChildOrNullAsync(TKey? parentId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<string> GetCodeOrDefaultAsync(TKey id);
     
    /// <summary>
    /// 找寻子集
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    Task<List<T>> FindChildrenAsync(TKey? parentId, bool recursive = false);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<T>> GetChildrenAsync(TKey? parentId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    Task<List<T>> GetAllChildrenWithParentCodeAsync(string paths, TKey parentId);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TA"></typeparam>
    /// <param name="all"></param>
    /// <param name="current"></param>
    /// <param name="keySelector"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    void SetChildren<TKey2, TA>(List<TA> all, TA current, Func<TA, int> keySelector, Func<TA, bool> predicate = null) where TA : class, IChildren<TA>, IParent<TKey2>;
}