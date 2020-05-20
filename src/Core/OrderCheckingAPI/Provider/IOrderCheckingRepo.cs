using System.Collections.Generic;
using System.Threading.Tasks;
using Gherkin.Contract.OrderCheckingAPI;

namespace Gherkin.Core.OrderCheckingAPI.Provider
{
    public interface IOrderCheckingRepo
    {
        /// <summary>
        /// Returns true, if an order represented by opc and lab is on stock
        /// </summary>       
        Task<bool> IsOrderOnStockAsync(string opc, string lab);
        Task<List<StockItem>> GetAllAsync();
    }
}
