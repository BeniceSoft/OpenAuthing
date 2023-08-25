namespace BeniceSoft.OpenAuthing.TreeServices;

public interface IChildren<T>
    where T : class
{
    ICollection<T> Children { get; set; }
}