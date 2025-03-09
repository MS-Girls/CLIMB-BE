using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 

public class UploadResumeDto
{
    [FromForm]  
    public IFormFile ResumeFile { get; set; }
}

