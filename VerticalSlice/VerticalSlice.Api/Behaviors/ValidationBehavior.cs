using FluentValidation;
using MediatR;
using VerticalSlice.Api.Common;

namespace VerticalSlice.Api.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => !r.IsValid)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count != 0)
        {
            var errorMessage = string.Join("; ", failures.Select(f => f.ErrorMessage));
            var error = Error.Validation("Validation.Failed", errorMessage);

            // Use reflection to create Result<T>.Failure
            var resultType = typeof(TResponse);
            if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueType = resultType.GetGenericArguments()[0];
                var failureMethod = resultType.GetMethod("Failure");
                var result = failureMethod?.Invoke(null, new object[] { error });
                return result as TResponse ?? throw new InvalidOperationException("Failed to create validation result");
            }

            // For non-generic Result
            if (resultType == typeof(Result))
            {
                return (Result.Failure(error) as TResponse)!;
            }
        }

        return await next();
    }
}
