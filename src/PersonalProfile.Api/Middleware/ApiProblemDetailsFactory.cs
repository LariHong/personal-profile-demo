using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PersonalProfile.Api.Middleware;

public static class ApiProblemDetailsFactory
{
    public static ProblemDetails Create(
        HttpContext context,
        int statusCode,
        string title,
        string detail)
    {
        var problem = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = title,
            Status = statusCode,
            Detail = detail,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        return problem;
    }

    public static ValidationProblemDetails CreateValidationProblem(HttpContext context, ModelStateDictionary modelState)
    {
        var problem = new ValidationProblemDetails(modelState)
        {
            Type = "https://httpstatuses.com/400",
            Title = "資料驗證失敗",
            Status = StatusCodes.Status400BadRequest,
            Detail = "請依照 API contract 修正欄位內容後再送出。",
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;
        return problem;
    }
}
