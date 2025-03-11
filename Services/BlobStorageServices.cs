using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;  // ✅ Import JSON library
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CLIMB_BE.Services
{
    public class BlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _problemsContainerName;
        private readonly string _resumesContainerName;

        public BlobStorageService(string connectionString, string problemsContainerName, string resumesContainerName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _problemsContainerName = problemsContainerName ?? throw new ArgumentNullException(nameof(problemsContainerName));
            _resumesContainerName = resumesContainerName ?? throw new ArgumentNullException(nameof(resumesContainerName));
        }

        // ✅ Retrieve company problems from Blob Storage (Fixes JSON Parsing)
        public async Task<List<ProblemDto>> GetCompanyProblemsAsync(string companyName)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_problemsContainerName);
            var blobClient = blobContainerClient.GetBlobClient($"{companyName}_problems.json");

            // ✅ Check if file exists before attempting to read
            if (!await blobClient.ExistsAsync())
            {
                return new List<ProblemDto>();
            }

            var response = await blobClient.DownloadAsync();
            using (var reader = new StreamReader(response.Value.Content))
            {
                var content = await reader.ReadToEndAsync();

                try
                {
                    // ✅ Fix: Ensure JSON deserialization correctly matches C# object structure
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true // ✅ Fixes case-sensitivity issues
                    };

                    return JsonSerializer.Deserialize<List<ProblemDto>>(content, options) ?? new List<ProblemDto>();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                    return new List<ProblemDto>(); // Return empty list if JSON is malformed
                }
            }
        }

        // ✅ Upload resume to Blob Storage
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

    // ✅ Fix: Ensure property names match JSON keys
    public class ProblemDto
    {
        public string Name { get; set; } = string.Empty;  // ✅ Default to empty string
        public string PracticeLink { get; set; } = string.Empty;  // ✅ Default to empty string
    }
}
