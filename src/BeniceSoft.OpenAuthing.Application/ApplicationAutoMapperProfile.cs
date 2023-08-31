using AutoMapper;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments;

namespace BeniceSoft.OpenAuthing;

public class ApplicationAutoMapperProfile : Profile
{
    public ApplicationAutoMapperProfile()
    {
        CreateMap<Department, DepartmentDto>();
    }
}