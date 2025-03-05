using CLIMB_BE.Models;
using CLIMB_BE.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CLIMB_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatServices _chatServices;

        public ChatController(IChatServices chatServices)
        {
            _chatServices = chatServices;
        }

        [HttpPost]
        public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest request)
        {
            // Validate input
            if (request == null || string.IsNullOrWhiteSpace(request.prompt))
            {
                return BadRequest(new { message = "Invalid request" });
            }

            // Await the async method
            var response = await _chatServices.GetResponse(request);
            return Ok(response);
        }
    }
}