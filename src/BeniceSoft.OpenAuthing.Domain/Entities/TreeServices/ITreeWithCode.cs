namespace BeniceSoft.OpenAuthing.Entities.TreeServices;

public interface ITreeWithCode<T> : ITree<T>
    where T : struct
{
    public string Code { get; }
}