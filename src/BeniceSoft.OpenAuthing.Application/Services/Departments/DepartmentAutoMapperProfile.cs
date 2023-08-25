using AutoMapper;
using BeniceSoft.OpenAuthing.Departments;
using BeniceSoft.OpenAuthing.Dtos.Departments.Requests;

namespace BeniceSoft.OpenAuthing.Services.Departments;

public class DepartmentAutoMapperProfile : Profile
{
    public DepartmentAutoMapperProfile()
    {
        CreateMap<Department, DepartmentDto>();
    }
}