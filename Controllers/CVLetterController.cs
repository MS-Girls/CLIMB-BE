using CLIMB_BE.Models;
using CLIMB_BE.Services;
using Microsoft.AspNetCore.Mvc;
namespace CLIMB_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CVLetterController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly IOCRServices _ocrServices;
        private readonly IChatServices _chatServices;
        public CVLetterController(BlobStorageService blobStorageService, IOCRServices ocrServices, IChatServices chatServices)
        {
            _blobStorageService = blobStorageService;
            _ocrServices = ocrServices;
            _chatServices = chatServices;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ChatResponse>> Post([FromForm] CVRequestDto request)
        {
            string jobTitle = request.JobTitle;
            string jobDesc = request.JobDesc;
            IFormFile resumeFile = request.ResumeFile;

            if (resumeFile == null || jobTitle == null || jobTitle == "" || jobDesc == null || jobDesc == "")
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            try
            {
                // Step 1: Upload Resume to Blob Storage
                string resumeUrl = await _blobStorageService.UploadResumeAsync(resumeFile);

                // Step 2: Extract Text Using OCR
                string resumeText = await _ocrServices.ReadDocument(resumeUrl);

                if (string.IsNullOrWhiteSpace(resumeText))
                {
                    return BadRequest(new { message = "Failed to extract text from resume." });
                }

                var chatRequest = new ChatRequest
                {
                    prompt = $"Given the Job Title, Job Description and the Resume.\nGenerate appropriate Cover Letter.Give only the content. It is not required to Specify from and to address. Write to the Hiring Manager and sign the cover letter by mentioning the applicant's name.\n\nJob Title: {jobTitle}\n\nJob Description: {jobDesc}\n\nResume: {resumeText}",
                    role = "You are an helpful assistant that efficiently generates cover letter for job interviews"
                };
                ChatResponse chatResponse = await _chatServices.GetResponse(chatRequest);

                return Ok(chatResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}