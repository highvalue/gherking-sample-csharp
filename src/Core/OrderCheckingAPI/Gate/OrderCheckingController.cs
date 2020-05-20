using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.OrderCheckingAPI;
using Gherkin.Core.OrderCheckingAPI.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Gherkin.Core.OrderCheckingAPI.Gate
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCheckingController : ControllerBase
    {
        private readonly ILogger<OrderCheckingController> _logger;
        private readonly IOrderCheckingService _orderCheckingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderCheckingController"/> class.
        /// </summary>
        /// <param name="orderCheckingService">Instance of the OrderCheckingService</param>
        /// <param name="logger">The logger.</param>
        public OrderCheckingController(ILogger<OrderCheckingController> logger, IOrderCheckingService orderCheckingService)
        {
            this._logger = logger;
            this._orderCheckingService = orderCheckingService;
        }

        /// <summary>
        /// GET api returning all StockItems
        /// </summary>
        /// <param name="id">id of the item.</param>
        /// <returns>Not found status code or code 200 including json.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]      
        public async Task<ActionResult<List<StockItem>>> GetAll()
        {
            return await _orderCheckingService.GetAllAsync();
        }

        /// <summary>
        /// GET api returning the .
        /// </summary>
        /// <param name="id">id of the item.</param>
        /// <returns>Not found status code or code 200 including json.</returns>
        [HttpGet("{lab}/{opc}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CheckResult>> Get([FromRoute] CheckOrderOnStockCommand command)
        {

            var result = new CheckResult();

            // the lab business rule has exploded and we decide to return a bad request
            // in a real system a middleware/filter and a proper problem details implementation should
            // should be used for proper error and return code handling
            try
            {
                result.OnStock = await _orderCheckingService.IsOnStockAsync(command);

                if (result.OnStock)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound(result);
                }

            }
            catch (ArgumentOutOfRangeException e) 
            {
                return BadRequest(e.Message);
            }
        }
    }
}