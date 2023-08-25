using BeniceSoft.OpenAuthing.Enums;
using LinkMore.Abp.Core.Extensions;
using Volo.Abp.Application.Dtos;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;

public class RoleSubjectRes : EntityDto<Guid>
{
    public string? Avatar { get; set; }
    public string? Name { get; set; }

    public string? Description { get; set; }
    public RoleSubjectType SubjectType { get; set; }

    public string SubjectTypeDescription => SubjectType.GetDescription();

    public Guid SubjectId { get; set; }

    public DateTime CreationTime { get; set; }
}