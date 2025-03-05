using System.Text.Json.Serialization;

namespace CLIMB_BE.Models
{
    public class ChatResponse
    {
        [JsonPropertyName("Response")] // Ensures correct serialization
    public string? Response { get; set; }
       
    }

}