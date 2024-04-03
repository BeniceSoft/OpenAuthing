namespace BeniceSoft.OpenAuthing.Entities.TreeServices;

public interface IChildren<T>
    where T : class
{
    ICollection<T> Children { get; set; }
}