using CLIMB_BE.Models;

namespace CLIMB_BE.Services
{
    public class ChatServices : IChatServices
    {
        public ChatResponse GetResponse(ChatRequest request)
        {
            return new ChatResponse
            {
                response = request.prompt
            };
        }
    }
}
