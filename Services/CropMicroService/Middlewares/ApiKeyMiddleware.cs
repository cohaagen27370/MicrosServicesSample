namespace CropMicroService.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private readonly string _apiKeyHeaderName;
    private readonly string _expectedApiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;

        _apiKeyHeaderName = _configuration.GetValue<string>("ApiKeySettings:HeaderName") ?? "X-Api-Key";
        _expectedApiKey = _configuration.GetValue<string>("ApiKeySettings:ApiKey") ?? string.Empty;

        if (string.IsNullOrEmpty(_expectedApiKey))
        {
            _logger.LogWarning("La clé API attendue est vide. Le middleware API Key pourrait ne pas fonctionner correctement.");
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/swagger") || 
            context.Request.Path.StartsWithSegments("/api/public")) 
        {
            await _next(context); // Passe la main au prochain middleware sans vérification
            return;
        }

        if (!context.Request.Headers.TryGetValue(_apiKeyHeaderName, out var receivedApiKey))
        {
            _logger.LogWarning($"Requête bloquée par middleware: en-tête '{_apiKeyHeaderName}' manquant pour {context.Request.Path}.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key manquant.");
            return;
        }

        if (!receivedApiKey.ToString().Equals(_expectedApiKey, StringComparison.Ordinal))
        {
            _logger.LogWarning($"Requête bloquée par middleware: API Key invalide pour {context.Request.Path}.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key invalide.");
            return;
        }

        await _next(context); // Continue le pipeline
    }
}