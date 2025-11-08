using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Threading;
using System.Threading.Tasks;

namespace SmartLocation.Health;

public class OraclePingHealthCheck : IHealthCheck
{
    private readonly string? _connString;

    public OraclePingHealthCheck(IConfiguration config)
    {
        _connString = config.GetConnectionString("ConexaoOracle");
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_connString))
            return HealthCheckResult.Unhealthy("Connection string 'ConexaoOracle' ausente.");

        try
        {
            await using var conn = new OracleConnection(_connString);
            await conn.OpenAsync(cancellationToken);

            // SELECT 1 FROM DUAL para garantir round-trip
            await using var cmd = new OracleCommand("SELECT 1 FROM DUAL", conn);
            var result = await cmd.ExecuteScalarAsync(cancellationToken);

            return result is not null
                ? HealthCheckResult.Healthy("Oracle respondeu com sucesso.")
                : HealthCheckResult.Unhealthy("Oracle não retornou resultado.");
        }
        catch (System.Exception ex)
        {
            return HealthCheckResult.Unhealthy("Falha ao conectar no Oracle.", ex);
        }
    }
}
