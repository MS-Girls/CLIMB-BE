using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CLIMB_BE.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _problemsContainerName;
        private readonly string _resumesContainerName;

        // Constructor that accepts the credentials
        public BlobStorageService(string connectionString, string problemsContainerName, string resumesContainerName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _problemsContainerName = problemsContainerName ?? throw new ArgumentNullException(nameof(problemsContainerName));
            _resumesContainerName = resumesContainerName ?? throw new ArgumentNullException(nameof(resumesContainerName));
        }

        // Retrieve company problems from Blob Storage
        public async Task<List<string>> GetCompanyProblemsAsync(string companyName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_problemsContainerName);
            var blobClient = blobContainerClient.GetBlobClient($"{companyName}_problems.txt");

            var response = await blobClient.DownloadAsync();
            using (var reader = new StreamReader(response.Value.Content))
            {
                var content = await reader.ReadToEndAsync();
                return content.Split('\n').ToList();
            }
        }

        // Upload resume to Blob Storage
        public async Task<string> UploadResumeAsync(IFormFile resumeFile)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_resumesContainerName);
            var blobClient = blobContainerClient.GetBlobClient(resumeFile.FileName);

            using (var stream = resumeFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
