using CLIMB_BE.Models;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using OpenAI.Chat;

namespace CLIMB_BE.Services
{
    public class ChatServices : IChatServices
    {
        public async Task<ChatResponse> GetResponse(ChatRequest request)
        {
            DotNetEnv.Env.Load();
            DotNetEnv.Env.TraversePath().Load();
          try 
            {
                var deploymentName = System.Environment.GetEnvironmentVariable("DEPLOYMENT_NAME");
        var endpointUrl = System.Environment.GetEnvironmentVariable("ENDPOINTURL");
        var key = System.Environment.GetEnvironmentVariable("KEY");
                
                var client = new AzureOpenAIClient(new Uri(endpointUrl), new ApiKeyCredential(key));
                var chatClient = client.GetChatClient(deploymentName);
                var role = request.role ?? "You are an AI assistant that helps people find information.";

               var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(role)
                };

                // Add chat history if available
                if (request.history != null && request.history.Any())
                {
                    foreach (var chat in request.history)
                    {
                        messages.Add(new UserChatMessage(chat));
                        messages.Add(new AssistantChatMessage(chat));
                    }
                }

      
                messages.Add(new UserChatMessage(request.prompt));
                
                var response = await chatClient.CompleteChatAsync(messages, new ChatCompletionOptions()
                {
                    Temperature = (float)0.7,
                    FrequencyPenalty = (float)0,
                    PresencePenalty = (float)0,
                });
               
                string chatResponseText = response.Value.Content.Last().Text;
                
                return new ChatResponse
                {
                    Response = chatResponseText 
                };
            }
            catch (Exception ex)
            {
                return new ChatResponse
                {
                    Response = $"An error occurred: {ex.Message}"
                };
            }
        }
    }
}
