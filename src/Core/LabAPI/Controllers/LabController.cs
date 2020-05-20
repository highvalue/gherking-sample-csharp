using System.Collections.Generic;
using System.Linq;
using Gherkin.Contract.LabAPI;
using Microsoft.AspNetCore.Mvc;

namespace Gherkin.Core.LabAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabController : ControllerBase
    {
        private static readonly List<string> Labs = new List<string>
        {
            "ID 1 G1 CNRx Guangzhou free trade zone",
            "ID 2 MXrX",
            "ID 3 DE",
            "ID 4 HU",
            "ID 5 PT",
            "ID 6 IN",
            "ID 7 G7 CN guangzhou bei CNMY"
        }; 

        [HttpGet]
        public IEnumerable<Lab> Get()
        {
            return Labs
                .Select(x => new Lab() { Name = x })
                .ToArray();
        }
    }
}
