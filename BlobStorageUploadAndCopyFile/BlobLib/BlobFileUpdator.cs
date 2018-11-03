using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobLib
{
    public class BlobFileUpdator
    {
        private readonly CloudBlobContainer _cloudBlobContainer;

        public BlobFileUpdator(string connectionString, string containerName)
        {
            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                _cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            }
            else
            {
                throw new Exception("ConnectionString failed.");
            }
        }

        public async Task CreateAndCopyFileAsync(string fileName, string path, Stream file, int allFileCount)
        {
            var firstfileName = EditFileName(fileName, 1);
            var dir = _cloudBlobContainer.GetDirectoryReference(path);
            var cloudBlockBlob = dir.GetBlockBlobReference(firstfileName);


            // upload blob
            await cloudBlockBlob.UploadFromStreamAsync(file);


            // copy blob
            var numList = Enumerable.Range(2, allFileCount - 1);
            var tasks = numList.Select(i =>
            {
                var name = EditFileName(fileName, i);
                return CopyBlobAsync(dir, cloudBlockBlob, name);
            });

            await Task.WhenAll(tasks);


            Debug.WriteLine("ok!");
        }

        

        private async Task CopyBlobAsync(CloudBlobDirectory dir, CloudBlockBlob sourceBlob, string destFileName)
        {
            var destBlob = dir.GetBlockBlobReference(destFileName);
            await destBlob.StartCopyAsync(sourceBlob);
        }

        private string EditFileName(string fullPathFileName, int num)
        {
            var fileName = Path.GetFileName(fullPathFileName);
            var name = Path.GetFileNameWithoutExtension(fileName);
            var editName = $"{name}-{num}{Path.GetExtension(fileName)}";
            return editName;
        }
    }
}
