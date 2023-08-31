using BeniceSoft.OpenAuthing.Commands.Departments;
using FluentValidation;

namespace BeniceSoft.OpenAuthing.Validators.Departments;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("名称不可为空");
    }
}