using System.Threading.Tasks;

namespace Gherkin.Gate.OnlineStoreApi.Provider
{
    public interface ICentralHubProvider
  {
      public Task<string> GetInventoryStatusAsync(string country, string code);
  }
}
