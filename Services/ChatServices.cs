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
                var endpointUrl = System.Environment.GetEnvironmentVariable("ENDPOINTURL");
                if (string.IsNullOrEmpty(endpointUrl))
                {
                    throw new InvalidOperationException("The environment variable 'ENDPOINTURL' is not set.");
                }
                var endpoint = new Uri(endpointUrl);
                var key = System.Environment.GetEnvironmentVariable("KEY");
                if (string.IsNullOrEmpty(key))
                {
                    throw new InvalidOperationException("The environment variable 'KEY' is not set.");
                }
                var credential = new Azure.AzureKeyCredential(key);
                var model = "Phi-4-mini-instruct";

                var client = new ChatCompletionsClient(
                    endpoint,
                    credential,
                   new AzureAIInferenceClientOptions());
                   var role = request.role ?? "You are an AI assistant that helps people find information.";
                var requestOptions = new ChatCompletionsOptions()
                {
                    Messages =
                            {
                                new ChatRequestSystemMessage(role),
                                new ChatRequestUserMessage(request.prompt),
                            },
                            Model=model
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
