using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.OrderCheckingAPI;
using Gherkin.Core.OrderCheckingAPI.Provider;
using Microsoft.Extensions.Logging;

namespace Gherkin.Core.OrderCheckingAPI.Core
{
    public class OrderCheckingService : IOrderCheckingService
    {
        private readonly IOrderCheckingRepo _repo;
        private readonly ILogger<OrderCheckingService> _logger;
        private readonly ILabProvider _labProvider;

        public OrderCheckingService(
            ILogger<OrderCheckingService> logger,
            IOrderCheckingRepo repo,
            ILabProvider labProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _labProvider = labProvider ?? throw new ArgumentNullException(nameof(labProvider));
        }

        public async Task<List<StockItem>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<bool> IsOnStockAsync(CheckOrderOnStockCommand command)
        {
            var labs = await _labProvider.GetExistingLabNamesAsync();

            // check business rules - in this case we check if we have a valid lab
           command.Validate(labs);

            return await _repo.IsOrderOnStockAsync(command.OPC, command.Lab);
        }     
    }
}
