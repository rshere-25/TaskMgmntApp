using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace TaskManagementApp.Services
{
    public class StorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public StorageService(IConfiguration configuration)
        {
            _blobServiceClient = new BlobServiceClient(
                new Uri(configuration["AzureStorage:BlobServiceEndpoint"]!),
                new Azure.Storage.StorageSharedKeyCredential(
                    configuration["AzureStorage:AccountName"],
                    configuration["AzureStorage:AccountKey"]
                )
            );

            _containerName = configuration["AzureStorage:ContainerName"]!;
        }

        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = "image/png" });

            return blobClient.Uri.ToString();
        }

        public async Task<Stream> DownloadImageFromUrlAsync(string imageUrl)
        {
            // Parse the image URL to extract the blob name.
            var uri = new Uri(imageUrl);
            var blobName = uri.Segments.Last();

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainer.GetBlobClient(blobName);

            MemoryStream ms = new MemoryStream();
            await blobClient.DownloadToAsync(ms);
            ms.Position = 0;
            return ms;
        }
    }

}
