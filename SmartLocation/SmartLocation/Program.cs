using Microsoft.EntityFrameworkCore;
using SmartLocation.Models;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext e configura a conex�o com Oracle
builder.Services.AddDbContext<Contexto>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("ConexaoOracle")));

// Servi�os da API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configura��es do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

