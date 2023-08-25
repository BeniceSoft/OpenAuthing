using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.IdPs.Responses;

public class ExternalIdentityProviderSimpleRes : EntityDto<Guid>
{
    public string Name { get; set; }

    public string DisplayName { get; set; }
    public string ProviderName { get; set; }
    public bool Enabled { get; set; }
    public string Logo { get; set; }
    public string Title { get; set; }
    public DateTime CreationTime { get; set; }
}