namespace Importer.WorkerService.Abstractions
{
    public interface IImportService
    {
        Task ProcessEnterpriseAsync(string enterpriseKey, CancellationToken cancellationToken);
    }
}
