using Microsoft.Extensions.Options;

namespace SmartLocation.Security;

public class ApiKeyMiddleware : IMiddleware
{
    private readonly ApiKeyOptions _opts;

    public ApiKeyMiddleware(IOptions<ApiKeyOptions> opts)
    {
        _opts = opts.Value;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (string.IsNullOrWhiteSpace(_opts.Value))
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("API Key não configurada");
            return;
        }

        if (!context.Request.Headers.TryGetValue(_opts.HeaderName, out var provided) ||
            !string.Equals(provided, _opts.Value, StringComparison.Ordinal))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key inválida ou ausente");
            return;
        }

        await next(context);
    }
}