namespace BeniceSoft.OpenAuthing.DynamicAuth;

public interface IDynamicAuthenticationManager
{
    void Add(string schemeName, string name, string displayName, IReadOnlyDictionary<string, string> optionsDictionary);
    
    void Remove(string schemeName, string name);
}