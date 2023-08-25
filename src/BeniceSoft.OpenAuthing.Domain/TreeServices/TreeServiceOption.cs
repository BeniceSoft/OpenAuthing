namespace BeniceSoft.OpenAuthing.TreeServices;

/// <summary>
/// 
/// </summary>
public class TreeServiceOption
{
    /// <summary>
    /// Maximum length of the DisplayName property.
    /// </summary>
    public int MaxDisplayNameLength { get; set; } = 128;

    /// <summary>
    /// Maximum depth of an OU hierarchy.
    /// </summary>
    public int MaxDepth { get; set; } = 16;

    /// <summary>
    /// Length of a code unit between dots.
    /// </summary>
    public int CodeUnitLength { get; set; } = 6;

    /// <summary>
    /// Maximum length of the Code property.
    /// </summary>
    public int MaxCodeLength => MaxDepth * (CodeUnitLength + 1) - 1;

    /// <summary>
    /// 是否忽略code已经存在
    /// </summary>
    public bool IsIgnoreCodeExist { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static TreeServiceOption Default()
    {
        return new TreeServiceOption()
        {
            MaxDisplayNameLength = 128,
            CodeUnitLength = 5,
            MaxDepth = 16
        };
    }
}