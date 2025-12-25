using VerticalSlice.Api.Common;

namespace VerticalSlice.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(this Result<T> result)
    {
        return result.Match(
            onSuccess: value => Results.Ok(value),
            onFailure: error => error.Type switch
            {
                ErrorType.NotFound => Results.NotFound(new { error = error.Code, message = error.Message }),
                ErrorType.Validation => Results.BadRequest(new { error = error.Code, message = error.Message }),
                ErrorType.Conflict => Results.Conflict(new { error = error.Code, message = error.Message }),
                _ => Results.Problem(
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code)
            });
    }

    public static IResult ToHttpResult(this Result result)
    {
        return result.Match(
            onSuccess: () => Results.NoContent(),
            onFailure: error => error.Type switch
            {
                ErrorType.NotFound => Results.NotFound(new { error = error.Code, message = error.Message }),
                ErrorType.Validation => Results.BadRequest(new { error = error.Code, message = error.Message }),
                ErrorType.Conflict => Results.Conflict(new { error = error.Code, message = error.Message }),
                _ => Results.Problem(
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code)
            });
    }

    public static IResult ToCreatedResult<T>(this Result<T> result, string routeName, Func<T, object> routeValues)
    {
        return result.Match(
            onSuccess: value => Results.CreatedAtRoute(routeName, routeValues(value), value),
            onFailure: error => error.Type switch
            {
                ErrorType.NotFound => Results.NotFound(new { error = error.Code, message = error.Message }),
                ErrorType.Validation => Results.BadRequest(new { error = error.Code, message = error.Message }),
                ErrorType.Conflict => Results.Conflict(new { error = error.Code, message = error.Message }),
                _ => Results.Problem(
                    detail: error.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: error.Code)
            });
    }
}
