using Gherkin.Testing.Factory.Base;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Gherkin.Testing.OnlineStore
{
    public class DeliveryTimeTestContext
    {            
        public WebApplicationFactory<Gherkin.Provider.SupplierMock.Startup> SupplierServiceFactory;
        public WebApplicationFactory<Gherkin.Core.CentralHub.Startup> CentralHubServiceFactory;
        public WebApplicationFactory<Gherkin.Gate.OnlineStoreApi.Startup> OnlineStoreApiServiceFactory;
        public DbContextContainer LocalInventoryContextContainer { get; set; } = new DbContextContainer();
        public DbContextContainer CentralInventoryContextContainer { get; set; } = new DbContextContainer();
    }
}