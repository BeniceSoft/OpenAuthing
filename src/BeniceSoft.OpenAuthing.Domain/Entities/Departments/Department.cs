using BeniceSoft.OpenAuthing.Entities.TreeServices;
using Volo.Abp.Domain.Entities.Auditing;

namespace BeniceSoft.OpenAuthing.Entities.Departments;

/// <summary>
/// 部门
/// </summary>
public class Department : FullAuditedAggregateRoot<Guid>, ITreeWithCode<Guid>
{
    public Department(Guid id, string code, string name, Guid? parentId, int seq) : base(id)
    {
        Code = code;
        Name = name;
        ParentId = parentId;
        Seq = seq;
        _children = new List<Department>();
    }

    /// <summary>
    /// 部门编码
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 父级id
    /// </summary>
    public Guid? ParentId { get; private set; }
  
    /// <summary>
    /// 排序
    /// </summary>
    public int Seq { get; private set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; private set; } = string.Empty;

    public Department? Parent { get; set; }

    public IReadOnlyCollection<Department> Children => _children;

    private readonly List<Department> _children;

    public void SavePaths(string path)
    {
        Path = path;
    }

    public void Update(string code, string name, int seq)
    {
        Code = code;
        Name = name;
        Seq = seq;
    }
}
