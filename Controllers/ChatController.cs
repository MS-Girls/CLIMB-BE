using CLIMB_BE.Models;
using CLIMB_BE.Services;
using Microsoft.AspNetCore.Mvc;

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

        public ActionResult<ChatResponse> Post([FromBody] ChatRequest request)
        {
            var res = _chatServices.GetResponse(request);
            return Ok(res);
        }

    }
}