using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class CVRequestDto
{
    [FromForm] public IFormFile ResumeFile { get; set; }
    [FromForm] public string JobTitle { get; set; }
    [FromForm] public string JobDesc { get; set; }
}

