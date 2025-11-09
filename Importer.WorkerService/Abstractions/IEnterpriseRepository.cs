using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.WorkerService.Abstractions
{
    public interface IEnterpriseRepository
    {
        Task<IEnumerable<MyEnterprise>> GetEnterprisesWithPendingFilesAsync(int MAX_PARALLEL_THREADS);
        Task MarkEnterpriseSyncErrorAsync(MyEnterprise id, Exception ex);
    }
}
