using AutoMapper;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;
using BeniceSoft.OpenAuthing.Dtos.UserGroups.Responses;
using BeniceSoft.OpenAuthing.UserGroups;

namespace BeniceSoft.OpenAuthing;

public class ApplicationAutoMapperProfile : Profile
{
    public ApplicationAutoMapperProfile()
    {
        CreateMap<Department, DepartmentDto>();
        
        CreateMap<UserGroup, UserGroupDto>();
    }
}