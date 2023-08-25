using BeniceSoft.OpenAuthing.Areas.Admin.Models.Roles;
using BeniceSoft.OpenAuthing.Enums;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace BeniceSoft.OpenAuthing.Areas.Admin.Controllers;

public partial class RolesController
{
    [HttpGet("{id}/subjects")]
    public async Task<List<RoleSubjectRes>> GetSubjectsAsync(Guid id)
    {
        var subjects = await _roleSubjectRepository.GetQueryableAsync();

        var roleSubjects = await AsyncExecuter.ToListAsync(subjects
            .Where(x => x.RoleId == id)
            .OrderByDescending(x => x.CreationTime));

        var result = new List<RoleSubjectRes>(roleSubjects.Count);

        var userIds = roleSubjects.Where(x => x.SubjectType == RoleSubjectType.User)
            .Select(x => x.SubjectId)
            .ToList();
        if (userIds.Any())
        {
            var userQueryable = await _userRepository.GetQueryableAsync();
            var users = await AsyncExecuter.ToListAsync(userQueryable
                .Where(x => userIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Nickname, x.UserName, x.Avatar }));
            foreach (var item in roleSubjects.Where(x => x.SubjectType == RoleSubjectType.User))
            {
                var subject = item.Adapt<RoleSubjectRes>();
                var user = users.FirstOrDefault(x => x.Id == item.SubjectId);
                subject.Name = user?.Nickname;
                subject.Description = user?.UserName;
                subject.Avatar = user?.Avatar;

                result.Add(subject);
            }
        }

        var userGroupIds = roleSubjects.Where(x => x.SubjectType == RoleSubjectType.UserGroup)
            .Select(x => x.SubjectId)
            .ToList();
        if (userGroupIds.Any())
        {
            var userGroupQueryable = await _userGroupRepository.GetQueryableAsync();
            var userGroups = await AsyncExecuter.ToListAsync(userGroupQueryable
                .Where(x => userGroupIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name }));
            foreach (var item in roleSubjects.Where(x => x.SubjectType == RoleSubjectType.UserGroup))
            {
                var subject = item.Adapt<RoleSubjectRes>();
                var userGroup = userGroups.FirstOrDefault(x => x.Id == item.SubjectId);
                subject.Name = userGroup?.Name;

                result.Add(subject);
            }
        }

        return result.OrderByDescending(x=>x.CreationTime).ToList();
    }

    [HttpPut("{id}/subjects")]
    public async Task<bool> SaveSubjectsAsync(Guid id, [FromBody] SaveRoleSubjectsReq req)
    {
        var role = await _roleRepository.GetAsync(id);
        foreach (var subject in req.Subjects)
        {
            role.AddSubject(GuidGenerator.Create(), subject.Type, subject.Id);
        }

        await _roleRepository.UpdateAsync(role);

        return true;
    }

    [HttpDelete("{id}/subjects/{roleSubjectId}")]
    public async Task<bool> RemoveSubjectAsync(Guid id, Guid roleSubjectId)
    {
        var role = await _roleRepository.GetAsync(id);
        role.RemoveSubject(roleSubjectId);

        await _roleRepository.UpdateAsync(role);

        return true;
    }
}