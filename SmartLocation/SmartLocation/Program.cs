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



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<Contexto>(opt =>
    opt.UseOracle(builder.Configuration.GetConnectionString("ConexaoOracle")));


builder.Services.AddControllers();


builder.Services.AddApiVersioning(opt =>
{
    opt.DefaultApiVersion = new ApiVersion(1, 0);
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.ReportApiVersions = true;
    opt.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});


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

// ===================== HEALTH CHECKS ===================
builder.Services
    .AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("OK"))
    .AddDbContextCheck<Contexto>("database");

// ===================== API KEY (Middleware) ============
builder.Services.Configure<ApiKeyOptions>(builder.Configuration.GetSection("ApiKey"));
builder.Services.AddSingleton<ApiKeyMiddleware>();

// ===================== ML.NET (serviço) ================
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

// API Key middleware (deixe Swagger e Health liberados)
app.UseWhen(ctx =>
    !ctx.Request.Path.StartsWithSegments("/swagger")
    && !ctx.Request.Path.StartsWithSegments("/health"),
    a => a.UseMiddleware<ApiKeyMiddleware>());

app.MapControllers();

// Health endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Name == "database"
});

app.Run();

// Necessário p/ WebApplicationFactory localizar o entrypoint
public partial class Program { }