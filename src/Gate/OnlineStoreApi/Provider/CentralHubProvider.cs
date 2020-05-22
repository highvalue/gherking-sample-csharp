using Gherkin.BuildingBlocks.Tech.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Gherkin.Contract.CentralHub;

namespace Gherkin.Gate.OnlineStoreApi.Provider
{
    public class CentralHubProvider : HttpClientBase, ICentralHubProvider
    {
        private readonly HttpClient _client;
        private readonly ILogger<ICentralHubProvider> _logger;
        public CentralHubProvider(ILogger<ICentralHubProvider> logger, HttpClient client) : base(client, logger)
        {
            _client = client;
        }

        public async Task<string> GetInventoryStatusAsync(string country, string item)
        {
           var result = await GetAsync<InventoryStatusResult>($"api/InventoryStatus/{country}/{item}");
           return result.InventoryStatus;
        }

    }
}
