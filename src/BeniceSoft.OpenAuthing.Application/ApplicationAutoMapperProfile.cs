using AutoMapper;
using BeniceSoft.OpenAuthing.Dtos.Departments;
using BeniceSoft.OpenAuthing.Entities.Departments;

namespace BeniceSoft.OpenAuthing;

public class ApplicationAutoMapperProfile : Profile
{
    public ApplicationAutoMapperProfile()
    {
        CreateMap<Department, DepartmentDto>();
    }
}