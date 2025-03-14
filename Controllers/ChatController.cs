using CLIMB_BE.Models;
using CLIMB_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace CLIMB_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly IOCRServices _ocrServices;
        private readonly IChatServices _chatServices;
        public ResumeController(BlobStorageService blobStorageService, IOCRServices ocrServices, IChatServices chatServices)
        {
            _blobStorageService = blobStorageService;
            _ocrServices = ocrServices;
            _chatServices = chatServices;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ChatResponse>> Post([FromForm] UploadResumeDto request)
        {
            
            if (request.ResumeFile == null || request.ResumeFile.Length == 0)
            {
                return BadRequest(new { message = "Invalid file upload" });
            }

            try
            {
                // Step 1: Upload Resume to Blob Storage
                string resumeUrl = await _blobStorageService.UploadResumeAsync(request.ResumeFile);

                // Step 2: Extract Text Using OCR
                string resumeText = await _ocrServices.ReadDocument(resumeUrl);

                if (string.IsNullOrWhiteSpace(resumeText))
                {
                    return BadRequest(new { message = "Failed to extract text from resume." });
                }

                var chatRequest = new ChatRequest
                {
                    prompt = $"Analyze the following resume for ATS compatibility and provide feedback:\n\n{resumeText}\n\n" +
              "Return the response in the following format:\n" +
              "**ATS Score:** [Give a score out of 100 based on ATS compatibility]\n" +
              "**Resume Suggestions:** [List improvements to optimize the resume for ATS and recruiters]",
                    role = "You are an ATS resume evaluator. Assess the resume and provide structured feedback."
                };
                ChatResponse chatResponse = await _chatServices.GetResponse(chatRequest);

                return Ok(chatResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("chat")]
        public async Task<ActionResult<ChatResponse>> PostChat([FromBody] ChatRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.prompt))
            {
                return BadRequest(new { message = "Invalid chat request" });
            }

            try
            {
                Console.WriteLine("Request: " + request.prompt + " " + request.history);
                ChatResponse chatResponse = await _chatServices.GetResponse(request);
                return Ok(chatResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

        [HttpPost("resumeextraction")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> PostResumeExtraction([FromForm] UploadResumeDto request)
        {
            if (request.ResumeFile == null || request.ResumeFile.Length == 0)
            {
                return BadRequest(new { message = "Invalid file upload" });
            }

            try
            {
                
                string resumeUrl = await _blobStorageService.UploadResumeAsync(request.ResumeFile);

                // Step 2: Extract Text Using OCR
                string resumeText = await _ocrServices.ReadDocument(resumeUrl);

                if (string.IsNullOrWhiteSpace(resumeText))
                {
                    return BadRequest(new { message = "Failed to extract text from resume." });
                }

                return Ok(resumeText);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }

    }
}