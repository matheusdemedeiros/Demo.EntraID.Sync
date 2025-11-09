using Importer.WorkerService.Abstractions;

namespace Importer.WorkerService.Infra.Data
{
    public class EnterpriseRepository : IEnterpriseRepository
    {
        private ILogger<EnterpriseRepository> _logger;
        private List<MyEnterprise> _enterprises = new()
        {
            new MyEnterprise { Key = "ENT001", Name = "ENT001" },
            new MyEnterprise { Key = "ENT002", Name = "ENT002" },
            new MyEnterprise { Key = "ENT003", Name = "ENT003" },
            new MyEnterprise { Key = "ENT004", Name = "ENT004" },
            new MyEnterprise { Key = "ENT005", Name = "ENT005" },
            new MyEnterprise { Key = "ENT006", Name = "ENT006" },
            new MyEnterprise { Key = "ENT007", Name = "ENT007" },
            new MyEnterprise { Key = "ENT008", Name = "ENT008" },
            new MyEnterprise { Key = "ENT009", Name = "ENT009" },
            new MyEnterprise { Key = "ENT0010", Name = "ENT0010" },
            new MyEnterprise { Key = "ENT0011", Name = "ENT0011" },
            new MyEnterprise { Key = "ENT0012", Name = "ENT0012" },
            new MyEnterprise { Key = "ENT0013", Name = "ENT0013" },
            new MyEnterprise { Key = "ENT0014", Name = "ENT0014" },
            new MyEnterprise { Key = "ENT0015", Name = "ENT0015" },
        };

        public EnterpriseRepository(ILogger<EnterpriseRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<MyEnterprise>> GetEnterprisesWithPendingFilesAsync(int MAX_PARALLEL_THREADS)
        {
            _logger.LogInformation("Fetching enterprises with pending files (max {MaxParallelThreads})", MAX_PARALLEL_THREADS);

            return await Task.FromResult(_enterprises
                .Take(MAX_PARALLEL_THREADS)
                .ToList());
        }

        public Task MarkEnterpriseSyncErrorAsync(MyEnterprise id, Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}
