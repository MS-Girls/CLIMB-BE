using CLIMB_BE.Models;

namespace CLIMB_BE.Services
{
    public interface IChatServices
    {
        Task<ChatResponse> GetResponse(ChatRequest request);
    }
}