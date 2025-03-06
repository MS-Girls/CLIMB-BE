using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using CLIMB_BE.Services;

namespace CLIMB_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlobStorageController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;

        public BlobStorageController(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        // GET: api/BlobStorage/get-problems
        [HttpGet("get-problems")]
        public async Task<IActionResult> GetCompanyProblems([FromQuery] string companyName)
        {
            var problems = await _blobStorageService.GetCompanyProblemsAsync(companyName);
            return Ok(problems);
        }

        // POST: api/BlobStorage/upload-resume
        [HttpPost("upload-resume")]
        public async Task<IActionResult> UploadResume([FromForm] IFormFile resumeFile)
        {
            var resumeUrl = await _blobStorageService.UploadResumeAsync(resumeFile);
            return Ok(new { ResumeUrl = resumeUrl });
        }
    }
}
