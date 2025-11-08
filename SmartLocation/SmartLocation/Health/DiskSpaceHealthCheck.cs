using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.IO;

namespace SmartLocation.Health;

public class DiskSpaceHealthCheck : IHealthCheck
{
    private readonly string _driveRoot;
    private readonly long _minFreeBytes;

    /// <param name="driveRoot">Ex.: "C:\"</param>
    /// <param name="minFreeGB">Espaço mínimo livre em GB</param>
    public DiskSpaceHealthCheck(string driveRoot, int minFreeGB)
    {
        _driveRoot = driveRoot;
        _minFreeBytes = minFreeGB * 1024L * 1024L * 1024L;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var drive = new DriveInfo(_driveRoot);
            var free = drive.AvailableFreeSpace;

            return Task.FromResult(
                free >= _minFreeBytes
                    ? HealthCheckResult.Healthy($"Livre: {free / (1024L * 1024L * 1024L)} GB")
                    : HealthCheckResult.Degraded($"Pouco espaço livre no disco ({free} bytes)."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("Erro ao ler espaço em disco.", ex));
        }
    }
}
