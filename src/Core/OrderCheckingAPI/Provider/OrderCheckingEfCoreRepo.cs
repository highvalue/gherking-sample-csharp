using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gherkin.Contract.OrderCheckingAPI;
using Microsoft.EntityFrameworkCore;

namespace Gherkin.Core.OrderCheckingAPI.Provider
{
    public class OrderCheckingEfCoreRepo : IOrderCheckingRepo
        {
            private readonly OrderCheckingContext _context;

            /// <summary>
            /// Initializes a new instance of the <see cref="OrderCheckingEfCoreRepo"/> class.
            /// </summary>
            /// <param name="dbClient">Cosmos DB client.</param>
            /// <param name="databaseName">Name of the database.</param>
            /// <param name="containerName">Name of the container.</param>
            public OrderCheckingEfCoreRepo( OrderCheckingContext context)
            {
                _context = context ?? throw new ArgumentNullException(nameof(context)); 
            }

        public  async Task<List<StockItem>> GetAllAsync()
        {
            return await _context.StockItems.ToListAsync();
        }

        /// <summary>
        /// Returns true, if an order represented by opc and lab is on stock
        /// </summary>   
        public async Task<bool> IsOrderOnStockAsync(string opc, string lab) 
        {
            return await _context.StockItems
                .Where(x => x.Lab == lab && x.OPC == opc)
                .AnyAsync();        
        }      
    }
}
