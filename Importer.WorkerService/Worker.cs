using Importer.WorkerService.Abstractions;

namespace Importer.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IEnterpriseRepository _enterpriseRepository;
        private readonly IImportService _importService;
        private readonly int MAX_PARALLEL_THREADS;

        public Worker(
            ILogger<Worker> logger,
            IConfiguration configuration,
            IEnterpriseRepository enterpriseRepository,
            IImportService importService)
        {
            _logger = logger;
            _enterpriseRepository = enterpriseRepository;
            _importService = importService;

            MAX_PARALLEL_THREADS = configuration.GetValue<int>("MaxParallelThreads", 1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var enterprises = await _enterpriseRepository
                    .GetEnterprisesWithPendingFilesAsync(MAX_PARALLEL_THREADS);

                if (!enterprises.Any())
                {
                    await Task.Delay(2000, stoppingToken);
                    continue;
                }

                var tasks = enterprises.Select(async enterprise =>
                {
                    var enterpriseKey = "FAKE_ENTERPRISE_KEY";

                    try
                    {
                        _logger.LogInformation("Iniciando importação do tenant {enterpriseKey}", enterpriseKey);

                        await _importService.ProcessEnterpriseAsync(enterpriseKey, stoppingToken);

                        _logger.LogInformation("Empresa {enterpriseKey} importado com sucesso!", enterpriseKey);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex,
                            "Erro processando empresa {enterpriseKey}: {message}",
                            enterprise.ToString(), ex.Message);

                        await _enterpriseRepository.MarkEnterpriseSyncErrorAsync(enterpriseKey, ex);
                    }
                });

                await Task.WhenAll(tasks);

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
