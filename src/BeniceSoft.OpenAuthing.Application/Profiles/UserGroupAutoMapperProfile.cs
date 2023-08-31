using AutoMapper;
using BeniceSoft.OpenAuthing.Dtos.UserGroups.Responses;
using BeniceSoft.OpenAuthing.UserGroups;

namespace BeniceSoft.OpenAuthing.Profiles;

public class UserGroupAutoMapperProfile : Profile
{
    public UserGroupAutoMapperProfile()
    {
        CreateMap<UserGroup, UserGroupDto>();
    }
}