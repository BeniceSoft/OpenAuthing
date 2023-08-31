using System.ComponentModel.DataAnnotations;
using BeniceSoft.OpenAuthing.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Volo.Abp;
using Volo.Abp.Validation;

namespace BeniceSoft.OpenAuthing.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation("Validating command {CommandType}", typeName);

        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            var validationErrors = failures
                .Select(x => new ValidationResult(x.ErrorMessage, new[] { x.PropertyName }))
                .ToArray();
            throw new AbpValidationException($"Command Validation Errors for type {typeof(TRequest).Name}", validationErrors);
        }

        return await next();
    }
}