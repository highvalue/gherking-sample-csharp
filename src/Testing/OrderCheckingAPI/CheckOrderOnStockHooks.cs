using System;
using System.Collections.Generic;
using Gherkin.Contract.OrderCheckingAPI;
using Gherkin.Core.OrderCheckingAPI.Provider;
using Gherkin.Testing.Factory.Base;
using Gherkin.Testing.Factory.OrderCheckingAPI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TechTalk.SpecFlow;

namespace Gherkin.Testing.OrderCheckingAPI
{
    [Binding]
    public sealed class OrderCheckingAPIHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private readonly OrderCheckingAPITestContext _testContext;

        public OrderCheckingAPIHooks(OrderCheckingAPITestContext testContext)
        {
           _testContext = testContext;
        }

        [BeforeScenario]
        [Scope(Tag = "OrderCheckingAPI")]
        public void BeforeScenario()
        {
            var isolationId = Guid.NewGuid().ToString();

            var labTestService = TestServerBuilder<Gherkin.Core.LabAPI.Startup>
                                    .NewTestServer(new Uri("http://localhost:4601"))
                                    .WithDefaultHostBuilder()
                                    .Build();
          
            var orderCheckingTestService = TestServerBuilder<Gherkin.Core.OrderCheckingAPI.Startup>
                                    .NewTestServer(new Uri("http://localhost:4600"))
                                    .AddInMemoryDB<OrderCheckingContext>(isolationId)
                                    .ArrangeData<OrderCheckingContext>(context => context.AddRange(CreateTestData()))                                   
                                    .WithAppSettingsRelativeToProjectOf<OrderCheckingAPIHooks>("Factory\\OrderCheckingAPI\\appsettings.test.json")
                                    .AddTestService<ILabProvider>(serviceCollection =>
                                    {
                                        serviceCollection.AddSingleton<ILabProvider>(sp =>
                                        {
                                            return new LabProvider(
                                                sp.GetRequiredService<ILogger<LabProvider>>(),
                                                labTestService.CreateClient());
                                        });
                                    })
                                    .WithDefaultHostBuilder()
                                    .Build();

            //services.AddHttpClient<ILabProvider, LabProvider>(c => c.BaseAddress = new Uri(appSettings.SccmOrderSoftwareUri));


            _testContext.OrderCheckingAPIServerFactory = orderCheckingTestService;


            //// simplified builder that does the default boilerplate as seen above
            //var anotherMachineService = OrderCheckingAPITestServer
            //  .NewTestServer(new Uri("http://localhost:4600"), isolationId)
            //  .ArrangeData<OrderCheckingContext>(context => context.AddRange(CreateTestData()))
            //  .BindServices(labTestService)
            //  .Build();
        }

        private List<StockItem> CreateTestData()
        {
            return new List<StockItem>()
            { new StockItem()
            {
                Id = 1,
                OPC = "0005770284",
                Lab = "ID 3 DE",
            },
            new StockItem()
            {
                Id = 2,
                OPC = "0005770285",
                Lab = "ID 6 IN",
            }
            };
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
