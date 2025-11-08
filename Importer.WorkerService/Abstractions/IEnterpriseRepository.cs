using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.WorkerService.Abstractions
{
    public interface IEnterpriseRepository
    {
        Task<IEnumerable<object>> GetEnterprisesWithPendingFilesAsync(int mAX_PARALLEL_THREADS);
        Task MarkEnterpriseSyncErrorAsync(object id, Exception ex);
    }
}
