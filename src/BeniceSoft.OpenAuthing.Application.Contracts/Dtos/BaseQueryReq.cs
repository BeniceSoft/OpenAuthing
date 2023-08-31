using BeniceSoft.Abp.Extensions.DynamicQuery.Abstractions.Model;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos;

public class BaseQueryReq : PagedResultRequestDto, IDynamicQueryRequest
{
    public List<DynamicQueryConditionGroup>? ConditionGroups { get; set; }

    public string? SearchKey { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}