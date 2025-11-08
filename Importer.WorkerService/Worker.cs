using Importer.WorkerService.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;

namespace Importer.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly int MAX_PARALLEL_THREADS;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration configuration,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            MAX_PARALLEL_THREADS = configuration.GetValue<int>("MaxParallelThreads", 1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Worker iniciado. Maximo de importacoes paralelas permitidas: {maxParallel}",
                MAX_PARALLEL_THREADS);

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var enterpriseRepository = scope.ServiceProvider.GetRequiredService<IEnterpriseRepository>();
                var importService = scope.ServiceProvider.GetRequiredService<IImportService>();

                var enterprises = await enterpriseRepository
                    .GetEnterprisesWithPendingFilesAsync(MAX_PARALLEL_THREADS);

                if (!enterprises.Any())
                {
                    _logger.LogDebug("Nenhuma empresa com arquivos pendentes encontrada. Aguardando...");
                    await Task.Delay(2000, stoppingToken);
                    continue;
                }

                _logger.LogInformation(
                    "Iniciando importacao para {enterpriseCount} empresa(s).",
                    enterprises.Count());

                var tasks = enterprises.Select(async enterprise =>
                {
                    using (NLog.ScopeContext.PushProperty("EnterpriseName", enterprise.Key))
                    {
                        try
                        {
                            _logger.LogInformation(
                                "Iniciando importacao para a empresa: {enterpriseKey} ({enterpriseName})",
                                enterprise.Key, enterprise.Name);

                            await importService.ProcessEnterpriseAsync(enterprise.Key, stoppingToken);

                            _logger.LogInformation(
                                "Importacao concluida com sucesso para a empresa: {enterpriseKey} ({enterpriseName})",
                                enterprise.Key, enterprise.Name);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Erro durante a importacao para a empresa: {enterpriseKey} ({enterpriseName}). Mensagem: {errorMessage}",
                                enterprise.Key, enterprise.Name, ex.Message);

                            await enterpriseRepository.MarkEnterpriseSyncErrorAsync(enterprise, ex);
                        }
                    }
                });

                await Task.WhenAll(tasks);

                _logger.LogInformation(
                    "Ciclo finalizado. Aguardando antes da próxima verificacao de pendências.");

                await Task.Delay(2000, stoppingToken);
            }

            _logger.LogWarning("Worker finalizado pois o cancellation token foi solicitado.");
        }
    }
}
