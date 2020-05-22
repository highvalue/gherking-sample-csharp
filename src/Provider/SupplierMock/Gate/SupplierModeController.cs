using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.Supplier;

namespace Gherkin.Provider.SupplierMock.Gate
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierModeController : ControllerBase
    {
        private readonly ILogger<SupplierModeController> _logger;
        private readonly Dictionary<string, object> _mockStore;

        public SupplierModeController(
            ILogger<SupplierModeController> logger,
            Dictionary<string, object> mockStore
            )
        {
            this._logger = logger;
            this._mockStore = mockStore;
        }

        //
        // Mock Interface
        //

        [HttpPost("ArrangeGetSupplierMode")]
        public async Task ArrangeComputerExists([FromBody] SupplierModeResult supplierModeResult)
        {
            this._mockStore.Add("supplierModeResult", supplierModeResult);
        }

        //
        // Regular Interface
        //

        [HttpGet("{item}")]
        public async Task<ActionResult<SupplierModeResult>> GetSupplierMode([FromRoute] string item)
        {
            var result = (SupplierModeResult)this._mockStore["supplierModeResult"];
            return Ok(result);
        }
    }
}
