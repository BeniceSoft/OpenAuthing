using AutoMapper;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;

namespace BeniceSoft.OpenAuthing.Profiles;

public class DepartmentAutoMapperProfile : Profile
{
    public DepartmentAutoMapperProfile()
    {
        CreateMap<Department, DepartmentDto>();
    }
}