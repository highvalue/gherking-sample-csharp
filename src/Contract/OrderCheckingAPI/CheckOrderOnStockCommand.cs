using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Gherkin.Contract.OrderCheckingAPI
{
    public class CheckOrderOnStockCommand
    {
        /// <summary>
        /// The OPC value.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "opc")]
        public string OPC { get; set; }

        /// <summary>
        /// The Lab value.
        /// </summary>
        [Required]
        [JsonProperty(PropertyName = "lab")]
        public string Lab { get; set; }
       
    }
}
