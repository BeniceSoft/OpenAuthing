using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Dtos.DepartmentMembers.Requests;

public class QueryDepartmentMembersReq : PagedResultRequestDto
{
    public bool OnlyDirectUsers { get; set; }
}