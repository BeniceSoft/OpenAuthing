﻿namespace BeniceSoft.OpenAuthing.Entities.TreeServices;

public interface ITree<T>
    where T : struct
{
    /// <summary>
    /// 父级Id
    /// </summary>
    public T? ParentId { get; }

    /// <summary>
    /// 层级
    /// </summary>
    public string Path { get; }
}