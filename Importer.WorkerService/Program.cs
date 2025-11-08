using Importer.WorkerService;
using Importer.WorkerService.Abstractions;
using Importer.WorkerService.Application.Services;
using Importer.WorkerService.Infra.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

var logger = LogManager.Setup()
                       .LoadConfigurationFromAppSettings()
                       .GetCurrentClassLogger();

try
{
    logger.Info("Inicializando Importer.WorkerService...");

    var builder = Host.CreateApplicationBuilder(args);

    // Remove os providers padrao
    builder.Logging.ClearProviders();

    // Habilita o NLog como provider de log
    builder.Logging.AddNLog();

    // Servicos da aplicacao
    builder.Services.AddHostedService<Worker>();
    builder.Services.AddScoped<IImportService, ImportService>();
    builder.Services.AddSingleton<IEnterpriseRepository, EnterpriseRepository>();

    var host = builder.Build();
    await host.RunAsync();
}
catch (Exception ex)
{
    logger.Error(ex, "O Worker falhou ao iniciar");
    throw;
}
finally
{
    LogManager.Shutdown();
}
