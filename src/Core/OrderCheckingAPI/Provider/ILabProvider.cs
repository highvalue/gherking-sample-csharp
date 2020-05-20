using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gherkin.Core.OrderCheckingAPI.Provider
{
  public interface ILabProvider
    {
        public Task<List<string>> GetExistingLabNamesAsync();
    }
}
