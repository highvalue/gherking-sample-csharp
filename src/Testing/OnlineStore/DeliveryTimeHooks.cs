using Gherkin.Core.CentralHub.Provider;
using Gherkin.Gate.OnlineStoreApi.Provider;
using Gherkin.Testing.Factory.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TechTalk.SpecFlow;

namespace Gherkin.Testing.OnlineStore
{
    [Binding]
    public sealed class DeliveryTimeHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private readonly DeliveryTimeTestContext _testContext;

        public DeliveryTimeHooks(DeliveryTimeTestContext testContext)
        {
           _testContext = testContext;
        }

        [BeforeScenario]
        [Scope(Tag = "DeliveryTime")]
        public void BeforeScenario()
        {
            var isolationId = Guid.NewGuid().ToString();

            _testContext.SupplierServiceFactory = TestServerBuilder<Gherkin.Provider.SupplierMock.Startup>
                .NewTestServer(new Uri("http://localhost:4601"))
                .WithDefaultHostBuilder()
                .Build();

            _testContext.CentralHubServiceFactory = TestServerBuilder<Gherkin.Core.CentralHub.Startup>
                .NewTestServer(new Uri("http://localhost:4602"))
                .AddInMemoryDB<LocalInvetoryContext>(isolationId)
                .LinkDbContext<LocalInvetoryContext>(_testContext.LocalInventoryContextContainer)
                .AddInMemoryDB<CentralInvetoryContext>(isolationId)
                .LinkDbContext<CentralInvetoryContext>(_testContext.CentralInventoryContextContainer)
                .AddTestService<ISupplierProvider>(serviceCollection =>
                {
                    serviceCollection.AddSingleton<ISupplierProvider>(serviceProvider => new SupplierProvider(
                        serviceProvider.GetRequiredService<ILogger<ISupplierProvider>>(),
                        _testContext.SupplierServiceFactory.CreateClient()));
                })
                .WithDefaultHostBuilder()
                .Build();

            _testContext.OnlineStoreApiServiceFactory = TestServerBuilder<Gherkin.Gate.OnlineStoreApi.Startup>
                .NewTestServer(new Uri("http://localhost:4603"))
                .AddTestService<ICentralHubProvider>(serviceCollection =>
                {
                    serviceCollection.AddSingleton<ICentralHubProvider>(serviceProvider => new CentralHubProvider(
                        serviceProvider.GetRequiredService<ILogger<ICentralHubProvider>>(),
                        _testContext.CentralHubServiceFactory.CreateClient()));
                })
                .WithDefaultHostBuilder()
                .Build();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }


}
