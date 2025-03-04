using CLIMB_BE.Models;

namespace CLIMB_BE.Services
{
    public interface IChatServices
    {
        ChatResponse GetResponse(ChatRequest request);
    }
}