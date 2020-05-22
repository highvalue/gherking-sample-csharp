using Gherkin.Core.CentralHub.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.CentralHub;

namespace Gherkin.Core.CentralHub.Gate
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryStatusController : ControllerBase
    {
        private readonly ILogger<InventoryStatusController> _logger;
        private readonly ILocalInventoryRepo _localInventoryRepo;
        private readonly ICentralInventoryRepo _centralInventoryRepo;
        private readonly ISupplierProvider _supplierProvider;

        public InventoryStatusController(
            ILogger<InventoryStatusController> logger, 
            ILocalInventoryRepo localInventoryRepo, 
            ICentralInventoryRepo centralInventoryRepo,
            ISupplierProvider supplierProvider
            )
        {
            this._logger = logger;
            this._localInventoryRepo = localInventoryRepo;
            this._centralInventoryRepo = centralInventoryRepo;
            this._supplierProvider = supplierProvider;
        }

        [HttpGet("{country}/{item}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Get([FromRoute] string country, string item)
        {
            var inventoryStatus = await GetInventoryStatusAsync(country, item);
            return Ok(inventoryStatus);
        }

        public async Task<InventoryStatusResult> GetInventoryStatusAsync(string country, string item)
        {
            var isInCountryStock = await _localInventoryRepo.IsInStockAsync(country, item);
            if (isInCountryStock)
            {
                return new InventoryStatusResult {InventoryStatus = "L-IS"};
            }

            var isInCentralStock = await _centralInventoryRepo.IsInStockAsync(item);
            if (isInCentralStock)
            {
                return new InventoryStatusResult { InventoryStatus = "C-IS" };
            }

            var supplierMode = await _supplierProvider.GetModeAsync(item);
            var inventoryStatus = InventoryStatusFromSupplierMode(supplierMode);

            return new InventoryStatusResult { InventoryStatus = inventoryStatus };
        }

        private string InventoryStatusFromSupplierMode(string supplierMode)
        {
            var lookup = new Dictionary<string, string>
            {
                { "just-in-time", "J-OOS"},
                { "late", "L-OOS"},
                { "refusal", "R-OOS"},
            };

            var found = lookup.ContainsKey(supplierMode);
            if (!found)
            {
                throw new Exception($"Supplier Mode not defined: {supplierMode}");
            }

            var inventoryStatus = lookup[supplierMode];
            return inventoryStatus;
        }
    }
}
