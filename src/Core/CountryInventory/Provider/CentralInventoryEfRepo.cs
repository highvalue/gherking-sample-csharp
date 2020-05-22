using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gherkin.Core.CentralHub.Provider
{
    public class CentralInventoryEfRepo : ICentralInventoryRepo
    {
        private readonly CentralInvetoryContext _context;

        public CentralInventoryEfRepo(CentralInvetoryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> IsInStockAsync(string item)
        {
            return await _context.CentralStockItems
                .Where(st => st.ItemName == item && st.ItemAvailable == true)
                .AnyAsync();
        }

        public async Task<List<CentralStockItem>> GetAllAsync()
        {
            return await _context.CentralStockItems.ToListAsync();
        }
    }
}