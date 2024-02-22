using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Repositories;

namespace BeniceSoft.OpenAuthing.TreeServices;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TKey"></typeparam>
public class TreeService<T, TKey> : ITreeService<T, TKey>
    where TKey : struct
    where T : class,
    ITree<TKey>, IEntity<TKey>

{
    protected readonly IRepository<T, TKey> Repository;
    protected readonly TreeServiceOption TreeServiceOption;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="treeServiceOption"></param>
    public TreeService(IRepository<T, TKey> repository, TreeServiceOption treeServiceOption = null)
    {
        Repository = repository;
        TreeServiceOption = treeServiceOption ?? TreeServiceOption.Default();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<string> GetNextChildCodeAsync(TKey? parentId)
    {
        var lastChild = await GetLastChildOrNullAsync(parentId);
        if (lastChild != null)
        {
            return CalculateNextCode(lastChild.Path);
        }

        var parentCode = string.Empty;
        if (parentId != null)
        {
            parentCode = await GetCodeOrDefaultAsync(parentId.Value);
        }
        return AppendCode(
            parentCode,
            CreateCode(1)
        );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<T?> GetLastChildOrNullAsync(TKey? parentId)
    {
        var children = await GetChildrenAsync(parentId);
        return children.MaxBy(c => c.Path);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<string> GetCodeOrDefaultAsync(TKey id)
    {
        var ou = await Repository.GetAsync(id);
        return ou?.Path;
    }
 
    /// <summary>
    /// 找寻子集
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="recursive"></param>
    /// <returns></returns>
    public virtual async Task<List<T>> FindChildrenAsync(TKey? parentId, bool recursive = false)
    {
        if (parentId == null)
        {
            return await Repository.GetListAsync();
        }

        if (!recursive)
        {
            return await GetChildrenAsync(parentId);
        }

        var code = await GetCodeOrDefaultAsync(parentId.Value);

        return await GetAllChildrenWithParentCodeAsync(code, parentId.Value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<List<T>> GetChildrenAsync(TKey? parentId)
    {
        return await Repository.GetListAsync(s => s.ParentId.Equals(parentId));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="paths"></param>
    /// <param name="parentId"></param>
    /// <returns></returns>
    public virtual async Task<List<T>> GetAllChildrenWithParentCodeAsync(string paths, TKey parentId)
    {
        return await Repository.GetListAsync(s => s.Path.StartsWith(paths) && !s.Id.Equals(parentId));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="all"></param>
    /// <param name="current"></param>
    /// <param name="keySelector"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="TKey2"></typeparam>
    /// <typeparam name="TA"></typeparam>
    public void SetChildren<TKey2, TA>(List<TA> all, TA current, Func<TA, int> keySelector, Func<TA, bool>? predicate = null) where TA : class, IChildren<TA>, IParent<TKey2>
    {
        if (!(current.Children?.Any() ?? false))
        {
            current.Children = all
                .Where(s => s.ParentId?.Equals(current.Id) ?? false)
                .WhereIf(predicate != null, predicate)
                .OrderByDescending(keySelector).ToList();
            if (!current.Children.Any())
            {
                return;
            }
        }

        foreach (var child in current.Children)
        {
            SetChildren<TKey2, TA>(all, child, keySelector);
        }
    }

    //public virtual async Task<List<T>> GetAllChildrenWithParentCodeAsync(string paths, TKey parentId)
    //{
    //    return await _repository.GetListAsync(s => s.Paths.StartsWith(paths) && !s.Id.Equals(parentId) && s.TntId.Equals(_currentTenant.TryGetId()));
    //}

    /// <summary>
    /// Creates code for given numbers.
    /// Example: if numbers are 4,2 then returns "00004.00002";
    /// </summary>
    /// <param name="numbers">Numbers</param>
    private string CreateCode(params int[] numbers)
    {
        if (numbers.IsNullOrEmpty())
        {
            return null;
        }

        return numbers.Select(number => number.ToString(new string('0', TreeServiceOption.CodeUnitLength))).JoinAsString(".");
    }

    /// <summary>
    /// Appends a child code to a parent code.
    /// Example: if parentCode = "00001", childCode = "00042" then returns "00001.00042".
    /// </summary>
    /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
    /// <param name="childCode">Child code.</param>
    private string AppendCode(string parentCode, string childCode)
    {
        if (childCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return childCode;
        }

        return parentCode + "." + childCode;
    }

    /// <summary>
    /// Gets relative code to the parent.
    /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="parentCode">The parent code.</param>
    private string GetRelativeCode(string code, string parentCode)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return code;
        }

        if (code.Length == parentCode.Length)
        {
            return null;
        }

        return code[(parentCode.Length + 1)..];
    }

    /// <summary>
    /// Calculates next code for given code.
    /// Example: if code = "00019.00055.00001" returns "00019.00055.00002".
    /// </summary>
    /// <param name="code">The code.</param>
    private string CalculateNextCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var parentCode = GetParentCode(code);
        var lastUnitCode = GetLastUnitCode(code);

        return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
    }

    /// <summary>
    /// Gets the last unit code.
    /// Example: if code = "00019.00055.00001" returns "00001".
    /// </summary>
    /// <param name="code">The code.</param>
    private string GetLastUnitCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        return splittedCode[^1];
    }

    /// <summary>
    /// Gets parent code.
    /// Example: if code = "00019.00055.00001" returns "00019.00055".
    /// </summary>
    /// <param name="code">The code.</param>
    private string GetParentCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        if (splittedCode.Length == 1)
        {
            return null;
        }

        return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
    }
}