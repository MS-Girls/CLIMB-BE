using CLIMB_BE.Models;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using Azure.Core;
using Azure.AI.Inference;

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
                var endpointUrl = Environment.GetEnvironmentVariable("ENDPOINTURL");
                if (string.IsNullOrEmpty(endpointUrl))
                {
                    throw new InvalidOperationException("The environment variable 'ENDPOINTURL' is not set.");
                }

                var endpoint = new Uri(endpointUrl);
                var key = Environment.GetEnvironmentVariable("KEY");
                if (string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("The environment variable 'KEY' is not set.");
                }

                var credential = new Azure.AzureKeyCredential(key);
                var model = "Phi-4-mini-instruct";

                var client = new ChatCompletionsClient(endpoint, credential, new AzureAIInferenceClientOptions());

                var role = request.role ?? "You are an AI assistant that helps people find information.";

                // Convert history to chat format
                var messages = new List<ChatRequestMessage>
        {
            new ChatRequestSystemMessage(role) // Set AI's role
        };

                // Add history messages if available
                if (request.history != null)
                {
                    foreach (var message in request.history)
                    {
                        if (message.sender == "user")
                        {
                            messages.Add(new ChatRequestUserMessage(message.message));
                        }
                        else if (message.sender == "assistant")
                        {
                            messages.Add(new ChatRequestAssistantMessage(message.message));
                        }
                    }
                }

                // Add the current user query
                messages.Add(new ChatRequestUserMessage(request.prompt));

                var requestOptions = new ChatCompletionsOptions()
                {
                    Messages = messages
                };

                Response<ChatCompletions> response = await client.CompleteAsync(requestOptions);

                return new ChatResponse
                {
                    Response = response.Value.Content
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
