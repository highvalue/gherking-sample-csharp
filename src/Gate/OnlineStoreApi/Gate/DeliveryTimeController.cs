using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.OnlineStoreApi;
using Gherkin.Gate.OnlineStoreApi.Core;

namespace Gherkin.Gate.OnlineStoreApi.Gate
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class DeliveryTimeController : ControllerBase
    {
        private readonly ILogger<DeliveryTimeController> _logger;
        private readonly IDeliveryTimeService _deliveryTimeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryTimeController"/> class.
        /// </summary>
        /// <param name="orderCheckingService">Instance of the OrderCheckingService</param>
        /// <param name="logger">The logger.</param>
        public DeliveryTimeController(ILogger<DeliveryTimeController> logger, IDeliveryTimeService deliveryTimeService)
        {
            this._logger = logger;
            this._deliveryTimeService = deliveryTimeService;
        }

        /// <summary>
        /// GET api returning DeliveryTime for Item based on CountryCode .
        /// </summary>
        /// <param name="id">id of the item.</param>
        /// <returns>OK status with DeliveryTime or BadRequest for invalid schema (e.g. invalid country)</returns>
        [HttpGet("{country}/{item}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<DeliveryTimeResult>> Get([FromRoute] string country, string item)
        {
            var isValid = IsValidCountryCode(country);
            if (!isValid)
            {
                var errorMessage = $"Invalid Country Code: {country}";
                return BadRequest(errorMessage);
            }

            var deliveryTime = await _deliveryTimeService.GetDeliveryTimeAsync(country, item);
            var result = new DeliveryTimeResult {DeliveryTime = deliveryTime};

            return Ok(result);
        }

        private bool IsValidCountryCode(string country)
        {
            var validCountryCodes = new List<string>{"DE", "UK", "USA", "IN", "CN"};
            var isValid = validCountryCodes.Contains(country);
            return isValid;
        }
    }
}