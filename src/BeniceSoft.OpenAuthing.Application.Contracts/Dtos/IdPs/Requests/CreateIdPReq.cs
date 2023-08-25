namespace BeniceSoft.OpenAuthing.Dtos.IdPs.Requests;

public class CreateIdPReq
{
    public string ProviderName { get; set; }
    
    public string Name { get; set; }
    
    public string DisplayName { get; set; }
    
    public Dictionary<string, string> Options { get; set; }
}