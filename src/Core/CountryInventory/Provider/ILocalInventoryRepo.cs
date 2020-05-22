using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gherkin.Core.CentralHub.Provider
{
    public interface ILocalInventoryRepo
    {
        public Task<bool> IsInStockAsync(string country, string item);
        public Task<List<LocalStockItem>> GetAllAsync();
    }
}
