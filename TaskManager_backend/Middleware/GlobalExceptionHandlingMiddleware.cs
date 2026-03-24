using TaskManager_backend.DTOs;
using System.Net;

namespace TaskManager_backend.Middleware;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrió un error inesperado: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            ArgumentException => CreateErrorResponse(context, HttpStatusCode.BadRequest, "Solicitud inválida", exception.Message),
            InvalidOperationException => CreateErrorResponse(context, HttpStatusCode.BadRequest, "Operación inválida", exception.Message),
            UnauthorizedAccessException => CreateErrorResponse(context, HttpStatusCode.Unauthorized, "No autorizado", exception.Message),
            KeyNotFoundException => CreateErrorResponse(context, HttpStatusCode.NotFound, "Recurso no encontrado", exception.Message),
            _ => CreateErrorResponse(context, HttpStatusCode.InternalServerError, "Error interno del servidor", "Ocurrió un error inesperado")
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private static ApiResponse<object> CreateErrorResponse(HttpContext context, HttpStatusCode statusCode, string message, string detail)
    {
        context.Response.StatusCode = (int)statusCode;
        return ApiResponse<object>.ErrorResult(message, new List<string> { detail });
    }
}
