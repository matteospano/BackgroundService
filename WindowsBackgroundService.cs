using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.WindowsService { 

public sealed class WindowsBackgroundService : BackgroundService
{
    private readonly provaServizio _provaServizio;
    private readonly ILogger<WindowsBackgroundService> _logger;

    public WindowsBackgroundService(
        provaServizio provaServizio,
        ILogger<WindowsBackgroundService> logger) =>
        (_provaServizio, _logger) = (provaServizio, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string prova = await _provaServizio.GetProvaAsync();
            _logger.LogWarning(prova);

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
}