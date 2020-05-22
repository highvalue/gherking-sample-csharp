using System.Threading.Tasks;

namespace Gherkin.Gate.OnlineStoreApi.Core
{
    public interface IDeliveryTimeService
    {
        Task<string> GetDeliveryTimeAsync(string country, string item);
    }
}
