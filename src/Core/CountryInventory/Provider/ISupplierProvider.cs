using Gherkin.BuildingBlocks.Tech.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using Gherkin.Contract.Supplier;

namespace Gherkin.Core.CentralHub.Provider
{
    public interface ISupplierProvider
    {
        public Task<string> GetModeAsync(string code);
    }

    public class SupplierProvider : HttpClientBase, ISupplierProvider
    {
        private readonly HttpClient _client;
        private readonly ILogger<ISupplierProvider> _logger;
        public SupplierProvider(ILogger<ISupplierProvider> logger, HttpClient client) : base(client, logger)
        {
            _client = client;
        }

        public async Task<string> GetModeAsync(string item)
        {
            var result = await GetAsync<SupplierModeResult>($"api/SupplierMode/{item}");
            return result.SupplierMode;
        }

    }
}
