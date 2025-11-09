using NDDigital.nddPrint.Common.LogsInputs;
using NDDigital.nddPrint.Common.LogsInputs.SyncFileBatches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Importer.WorkerService.Abstractions
{
    public interface IFilesInputFacade
    {
        IFilesInputManager FilesInputMgr { get; }
        ISyncFileBatchesManager SyncFileBatchesManager { get; }

        FileInput GetNextFile(long fileInputID = 0, string enterpriseName = "");
        FileInput GetNextFile(SyncFileBatch syncFileBatch);
        FileInput Retrieve(long fileInputID);
        SyncFileBatch GetNextSyncFileBatch(Guid batchID);
        void Complete(FileInput fileInput, bool allJobsSuccessful);
        void CompleteFile(FileInput fileInput);
        void CompleteFile(FileInput fileInput, bool allJobsSuccessful);
        void CreateSystemFile(FileInput fileInput, string messageError, string newContentFile = "");
        void SetFailed(FileInput fileInput, string messageError);
        void SetNoEnts(FileInput fileInput);
        void Remove(long fileInputID);
        void Reset(FileInput fileInput);
        bool TestConnection();
        void ResetAllFilesLocked();
        void ResetAllSyncFileBatchesLocked();
        Enterprise RetrieveEnterprise(int enterpriseID);
    }
}
