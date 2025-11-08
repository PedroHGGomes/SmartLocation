using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using SmartLocation.ML;
using SmartLocation.Models;
using SmartLocation.Security;
using Oracle.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Oracle.ManagedDataAccess.Client; 
using System.Text.Json;               
using System.IO;                      
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Db
builder.Services.AddDbContext<Contexto>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("ConexaoOracle")));

builder.Services.AddControllers();

// Api
builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SmartLocation API",
        Version = "v1",
        Description = "API RESTful para gestão de motos, sensores, usuários e endereços de pátio da Mottu."
    });

    c.EnableAnnotations();

    // Segurança por API Key no Swagger
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Chave de API via cabeçalho X-API-KEY",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Health checks
var oracleConn = builder.Configuration.GetConnectionString("ConexaoOracle");

// mínimo de espaço livre (em GB) no drive C:
const int minFreeGB = 1;

builder.Services
    .AddHealthChecks()
    // Liveness
    .AddCheck("self", () => HealthCheckResult.Healthy("OK"), tags: new[] { "live" })

    // Readiness: EF/DbContext
    .AddDbContextCheck<Contexto>("database", tags: new[] { "ready" })

    // Readiness: ping direto no Oracle (versão síncrona)
    .AddCheck("oracle-ping", () =>
    {
        if (string.IsNullOrWhiteSpace(oracleConn))
            return HealthCheckResult.Unhealthy("ConnectionString 'ConexaoOracle' ausente.");

        try
        {
            using var conn = new OracleConnection(oracleConn);
            conn.Open();
            using var cmd = new OracleCommand("SELECT 1 FROM DUAL", conn);
            var result = cmd.ExecuteScalar();

            return result is not null
                ? HealthCheckResult.Healthy("Oracle respondeu com sucesso.")
                : HealthCheckResult.Unhealthy("Oracle não retornou resultado.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha ao conectar no Oracle.", ex);
        }
    }, tags: new[] { "ready" })

    // Readiness: espaço em disco
    .AddCheck("disk-space", () =>
    {
        try
        {
            var drive = new DriveInfo("C");
            var free = drive.AvailableFreeSpace;
            var minBytes = minFreeGB * 1024L * 1024L * 1024L;

            return free >= minBytes
                ? HealthCheckResult.Healthy($"Livre: {free / (1024L * 1024L * 1024L)} GB")
                : HealthCheckResult.Degraded($"Pouco espaço livre no disco. Livre: {free} bytes");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Erro ao ler espaço em disco.", ex);
        }
    }, tags: new[] { "ready" });

// Api Key
builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("ApiKey"));
builder.Services.AddSingleton<ApiKeyMiddleware>();

// Ml.Net
builder.Services.AddSingleton<ManutencaoMlService>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartLocation API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// ApiKey middleware
// (libera swagger e health*)
app.UseWhen(ctx =>
    !ctx.Request.Path.StartsWithSegments("/swagger")
    && !ctx.Request.Path.StartsWithSegments("/health"),
    a => a.UseMiddleware<ApiKeyMiddleware>());

// Controllers
app.MapControllers();

// Endpoints - Health
static Task WriteHealthJson(HttpContext ctx, HealthReport report)
{
    ctx.Response.ContentType = "application/json; charset=utf-8";

    var payload = new
    {
        status = report.Status.ToString(),
        totalDuration = report.TotalDuration.TotalMilliseconds,
        entries = report.Entries.Select(e => new
        {
            name = e.Key,
            status = e.Value.Status.ToString(),
            duration = e.Value.Duration.TotalMilliseconds,
            description = e.Value.Description,
            error = e.Value.Exception?.Message,
            data = e.Value.Data
        })
    };

    var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
    return ctx.Response.WriteAsync(json);
}

// Liveness: só “self”
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live"),
    ResponseWriter = WriteHealthJson
});

// Readiness: dependências externas (EF + Oracle + disco)
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("ready"),
    ResponseWriter = WriteHealthJson
});

// Agregado: tudo
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = WriteHealthJson
});

app.Run();

// Necessário p/ WebApplicationFactory localizar o entrypoint
public partial class Program { }
