using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Gate.OnlineStoreApi.Provider;

namespace Gherkin.Gate.OnlineStoreApi.Core
{
    public class DeliveryTimeService : IDeliveryTimeService
    {
        private readonly ILogger<DeliveryTimeService> _logger;
        private readonly ICentralHubProvider _centralHubProvider;

        public DeliveryTimeService(
            ILogger<DeliveryTimeService> logger,
            ICentralHubProvider centralHubProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _centralHubProvider = centralHubProvider ?? throw new ArgumentNullException(nameof(centralHubProvider));
        }

        public async Task<string> GetDeliveryTimeAsync(string country, string item)
        {
            var isValid = IsValidItemName(country, item);
            if (!isValid)
            {
                return "canceled";
            }

            var status = await _centralHubProvider.GetInventoryStatusAsync(country, item);
            var deliveryTime = CalculateDeliveryTime(status);

            return deliveryTime;
        }

        private bool IsValidItemName(string country, string item)
        {
            return true; // TODO implement business validation
        }

        private string CalculateDeliveryTime(string status)
        {
            var lookup = new Dictionary<string, string>
            {
                { "L-IS", "premium"},   // Local Inventory is In-Stock
                { "C-IS", "standard"},  // Central Inventory is In-Stock,
                { "J-OOS", "delayed"},  // Central Inventory is Out-of-Stock, Supplier in Just-In-Time Mode
                { "L-OOS", "critical"}, // Central Inventory is Out-of-Stock, Supplier in Late Mode
                { "R-OOS", "canceled"}, // Central Inventory is Out-of-Stock, Supplier in Refusal Mode
            };

            var found = lookup.ContainsKey(status);
            if (!found)
            {
                return "canceled";
            }

            var deliveryTime = lookup[status];
            return deliveryTime;
        }
    }
}
