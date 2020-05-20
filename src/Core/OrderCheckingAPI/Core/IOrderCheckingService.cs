using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.OrderCheckingAPI;

namespace Gherkin.Core.OrderCheckingAPI.Core
{
    public interface IOrderCheckingService
    {
        /// <summary>
        /// Returns whether an order is on stock or not.
        /// </summary>        
        Task<bool> IsOnStockAsync(CheckOrderOnStockCommand command);
        Task<List<StockItem>> GetAllAsync();
    }
}
