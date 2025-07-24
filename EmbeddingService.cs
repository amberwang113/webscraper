using OpenAI.Embeddings;
using Azure.AI.OpenAI;

namespace AIChatbotProject
{
    public class EmbeddingService
    {
        private EmbeddingClient embeddingClient;

        public EmbeddingService(AzureOpenAIClient azureClient)
        {
            embeddingClient = azureClient.GetEmbeddingClient("text-embedding-3-small");
        }

        public async Task<float[]> GetEmbeddingAsync(string sentence)
        {
            return (await embeddingClient.GenerateEmbeddingAsync(sentence)).Value.ToFloats().ToArray();
        }
    }
}
