using System;
using Azure;
using Azure.AI.DocumentIntelligence;

namespace CLIMB_BE.Services;

public class OCRServices : IOCRServices
{
    public async Task<string> ReadDocument(string blobUrl)
    {
        string content = "";

        string endpoint = Environment.GetEnvironmentVariable("AZURE_DOCUMENT_INTELLIGENCE_ENDPOINT")!;
        string key = Environment.GetEnvironmentVariable("AZURE_DOCUMENT_INTELLIGENCE_API_KEY")!;

        AzureKeyCredential credential = new AzureKeyCredential(key);
        DocumentIntelligenceClient client = new DocumentIntelligenceClient(new Uri(endpoint), credential);

        Uri fileUri = new Uri(blobUrl);

        Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-read", fileUri);
        AnalyzeResult result = operation.Value;

        foreach (DocumentPage page in result.Pages)
        {
            for (int i = 0; i < page.Lines.Count; i++)
            {
                DocumentLine line = page.Lines[i];
                content += line.Content + "\n";
            }
        }

        return content;
    }
}
