using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.OpenIddict.Responses;

public class QueryApplicationRes : EntityDto<string>
{
    public string ClientId { get; set; }

    public string DisplayName { get; set; }
}