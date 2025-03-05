namespace CLIMB_BE.Models
{
    public class ChatRequest
    {
        public required string prompt { get; set; }
        public  string? role { get; set; }
        public List<string>? history { get; set; } 

    }
}