namespace TaskManagementApp.Services
{
    public interface IStorageService
    {
        Task<string> UploadImageAsync(Stream fileStream, string fileName);
        Task<Stream> DownloadImageFromUrlAsync(string imageUrl);

    }
}
