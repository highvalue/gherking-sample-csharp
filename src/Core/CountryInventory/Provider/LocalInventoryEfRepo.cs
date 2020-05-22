using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Gherkin.Core.CentralHub.Provider
{
    public class LocalInventoryEfRepo : ILocalInventoryRepo
    {
        private readonly LocalInvetoryContext _context;

        public LocalInventoryEfRepo(LocalInvetoryContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> IsInStockAsync(string country, string item)
        {
            return await _context.LocalStockItems
                .Where(st => st.CountryCode == country && st.ItemName == item && st.ItemAvailable == true)
                .AnyAsync();
        }

        public async Task<List<LocalStockItem>> GetAllAsync()
        {
            return await _context.LocalStockItems.ToListAsync();
        }
    }
}