using ECommer.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ECommer.Services
{
    public class AzureBlobHelper : IAzureBlobHelper
    {
        #region Constants
        private readonly CloudBlobClient _cloudBlobClient;
        #endregion

        #region Builder
        public AzureBlobHelper(IConfiguration configuration)
        {
            string keys = configuration["Blob:AzureStorage"];
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(keys); 
            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }
        #endregion

        #region Public methods
        public async Task<Guid> UploadAzureBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream();
            
            return await UploadAzureBlobAsync(stream, containerName);
        }

        public async Task<Guid> UploadAzureBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            
            return await UploadAzureBlobAsync(stream, containerName);
        }

        public async Task DeleteAzureBlobAsync(Guid id, string containerName)
        {
            CloudBlobContainer blobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob cloudBlock = blobContainer.GetBlockBlobReference($"{id}");
            
            await cloudBlock.DeleteAsync();
        }
        #endregion

        #region Private methods
        private async Task<Guid> UploadAzureBlobAsync(Stream stream, string containerName)
        {
            Guid nameGuid = Guid.NewGuid();
            
            CloudBlobContainer blobContainer = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob cloudBlock = blobContainer.GetBlockBlobReference($"{nameGuid}");
            
            await cloudBlock.UploadFromStreamAsync(stream);

            return nameGuid;
        }
        #endregion
    }
}
