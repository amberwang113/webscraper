using Azure.AI.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using WebsiteScraper.Constants;

namespace AIChatbotProject
{
    class Program
    {
        static async Task Main()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .Build();

            var credential = new DefaultAzureCredential();

            string databaseName = config[ConfigurationConstants.CosmosDbDatabaseName] ?? $"{config[ConfigurationConstants.WebsiteNameEnvironmentVariable]}-vectors";
            string containerName = "base";

            DBService dbService = new(config[ConfigurationConstants.CosmosDbEndpoint], credential, databaseName, containerName);
            EmbeddingService embeddingService = new(new AzureOpenAIClient(new Uri(config[ConfigurationConstants.AzureOpenAIEndpoint]), credential));
            Scraper scraper = new(dbService, embeddingService);

            await dbService.CreateDatabaseAndFreshContainerAsync();
            await scraper.KickOffScraping(config[ConfigurationConstants.WebsiteScrapingEndpoints] ?? config[ConfigurationConstants.WebsiteDefaultHostNameEnvironmentVariable], 4);
        }
    }
}