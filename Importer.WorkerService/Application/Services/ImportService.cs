using Importer.WorkerService.Abstractions;

namespace Importer.WorkerService.Application.Services
{
    public class ImportService : IImportService
    {
        private readonly ILogger<ImportService> _logger;
        public ImportService(ILogger<ImportService> logger)
        {
            _logger = logger;
        }

        public Task ProcessEnterpriseAsync(string enterpriseKey, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing import for enterprise: {enterpriseKey}", enterpriseKey);

            return Task.Delay(5000);
        }
    }
}
