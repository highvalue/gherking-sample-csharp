using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Gherkin.Contract.Supplier;

namespace Gherkin.Provider.Supplier.Gate
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierModeController : ControllerBase
    {
        private readonly ILogger<SupplierModeController> _logger;

        public SupplierModeController(
            ILogger<SupplierModeController> logger
            )
        {
            this._logger = logger;
        }

        [HttpGet("{item}")]
        public async Task<ActionResult<SupplierModeResult>> Get([FromRoute] string item)
        {
            return Ok(new SupplierModeResult{SupplierMode = "just-in-time"});
            //return Ok(new SupplierModeResult{SupplierMode = "late" });
            //return Ok(new SupplierModeResult{SupplierMode = "refusal" });
        }
    }
}
