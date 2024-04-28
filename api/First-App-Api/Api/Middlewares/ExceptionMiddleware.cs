using System.Net;
using System.Text.Json;
using Api.Errors;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;

    public ExceptionMiddleware(RequestDelegate requestDelegate,
        ILogger<ExceptionMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception exception)
        { 
            await HandleExceptionAsync(context, exception);
            if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                throw;
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new ApiResponse((int)HttpStatusCode.InternalServerError);

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}