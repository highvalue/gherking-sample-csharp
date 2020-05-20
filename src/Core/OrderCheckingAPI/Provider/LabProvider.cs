using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gherkin.BuildingBlocks.Technical.Http;
using Gherkin.Contract.LabAPI;
using Microsoft.Extensions.Logging;

namespace Gherkin.Core.OrderCheckingAPI.Provider
{
    public class LabProvider : HttpClientBase, ILabProvider
    {
        private readonly HttpClient _client;
        private readonly ILogger<LabProvider> _logger;
        public LabProvider(ILogger<LabProvider> logger, HttpClient client) : base(client, logger)
        {
            _client = client;
        }

        public async Task<List<string>> GetExistingLabNamesAsync()
        {
           var labs = await GetAsync<List<Lab>>("api/lab");

            return labs
                .Select(x => x.Name)
                .ToList();
        }

    }
}
