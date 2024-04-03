namespace BeniceSoft.OpenAuthing.Entities.TreeServices;

public interface IParent<T>
{
    T ParentId { get; }

    T Id { get; }
}