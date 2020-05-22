using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gherkin.Core.CentralHub.Provider
{
    public interface ICentralInventoryRepo
    {
        public Task<bool> IsInStockAsync(string item);
        public Task<List<CentralStockItem>> GetAllAsync();
    }
}
