using Importer.WorkerService.Abstractions;

namespace Importer.WorkerService.Infra.Data
{
    public class EnterpriseRepository : IEnterpriseRepository
    {
        private ILogger<EnterpriseRepository> _logger;
        private List<Enterprise> _enterprises = new()
        {
            new Enterprise { Key = "ENT001", Name = "ENT001" },
            new Enterprise { Key = "ENT002", Name = "ENT002" },
            new Enterprise { Key = "ENT003", Name = "ENT003" },
            new Enterprise { Key = "ENT004", Name = "ENT004" },
            new Enterprise { Key = "ENT005", Name = "ENT005" },
            new Enterprise { Key = "ENT006", Name = "ENT006" },
            new Enterprise { Key = "ENT007", Name = "ENT007" },
            new Enterprise { Key = "ENT008", Name = "ENT008" },
            new Enterprise { Key = "ENT009", Name = "ENT009" },
            new Enterprise { Key = "ENT0010", Name = "ENT0010" },
            new Enterprise { Key = "ENT0011", Name = "ENT0011" },
            new Enterprise { Key = "ENT0012", Name = "ENT0012" },
            new Enterprise { Key = "ENT0013", Name = "ENT0013" },
            new Enterprise { Key = "ENT0014", Name = "ENT0014" },
            new Enterprise { Key = "ENT0015", Name = "ENT0015" },
        };

        public EnterpriseRepository(ILogger<EnterpriseRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<Enterprise>> GetEnterprisesWithPendingFilesAsync(int MAX_PARALLEL_THREADS)
        {
            _logger.LogInformation("Fetching enterprises with pending files (max {MaxParallelThreads})", MAX_PARALLEL_THREADS);

            return await Task.FromResult(_enterprises
                .Take(MAX_PARALLEL_THREADS)
                .ToList());
        }

        public Task MarkEnterpriseSyncErrorAsync(Enterprise id, Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}
