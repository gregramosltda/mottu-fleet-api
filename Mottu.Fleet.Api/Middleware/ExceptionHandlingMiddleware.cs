using System.Net;
using System.Text.Json;
using Mottu.Fleet.Api.Exceptions;

namespace Mottu.Fleet.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "Recurso não encontrado: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, "Erro de validação: {Message}", ex.Message);
            await WriteResponse(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno não tratado");
            await WriteResponse(context, HttpStatusCode.InternalServerError, "Erro interno do servidor");
        }
    }

    private static async Task WriteResponse(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)status;

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)status}",
            title = status.ToString(),
            status = (int)status,
            detail = message,
            instance = context.Request.Path.Value
        };

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}