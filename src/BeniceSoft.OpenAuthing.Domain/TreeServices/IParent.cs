namespace BeniceSoft.OpenAuthing.TreeServices;

public interface IParent<T>
{
    T ParentId { get; }

    T Id { get; }
}