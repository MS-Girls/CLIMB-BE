namespace CLIMB_BE.Models
{
    public class ChatRequest
    {
        public required string prompt { get; set; }
        public  string? role { get; set; }
        public List<ChatMessage>? history { get; set; }

    }
}

public class ChatMessage
{
    public string? sender { get; set; } 
    public string? message { get; set; }
}