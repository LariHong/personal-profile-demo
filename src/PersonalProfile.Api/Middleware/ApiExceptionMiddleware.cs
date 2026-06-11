using Microsoft.EntityFrameworkCore;
using PersonalProfile.Api.Services;

namespace PersonalProfile.Api.Middleware;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (DuplicateNationalIdException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status409Conflict, "資料重複", ex.Message);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Database update failed.");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "資料庫寫入失敗", "請確認 SQL Server 連線與資料表狀態。");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled API exception.");
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "伺服器錯誤", "請稍後再試或查看伺服器日誌。");
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        var problem = ApiProblemDetailsFactory.Create(context, statusCode, title, detail);

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problem);
    }
}
