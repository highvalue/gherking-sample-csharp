using System;
using Gherkin.Core.OrderCheckingAPI.Provider;
using Gherkin.Testing.Factory.Base;
using Gherkin.Testing.OrderCheckingAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gherkin.Testing.Factory.OrderCheckingAPI
{
   public class OrderCheckingAPITestServer 
    {
        private TestServerBuilder<Gherkin.Core.OrderCheckingAPI.Startup> _testServerBuilder;

        public  static OrderCheckingAPITestServer NewTestServer(Uri baseAddress, string isolationId)
        {
            var newInstance = new OrderCheckingAPITestServer();
            newInstance._testServerBuilder = TestServerBuilder<Gherkin.Core.OrderCheckingAPI.Startup>
                                    .NewTestServer(baseAddress)
                                    .AddInMemoryDB<OrderCheckingContext>(isolationId)
                                    .WithAppSettingsRelativeToProjectOf<OrderCheckingAPIHooks>("Factory\\OrderCheckingAPI\\appsettings.test.json")
                                    .WithDefaultHostBuilder();
            return newInstance;
        }

        public OrderCheckingAPITestServer BindServices(WebApplicationFactory<Gherkin.Core.LabAPI.Startup> labAPIServiceFactory)
        {
            _testServerBuilder.AddTestService<ILabProvider>(serviceCollection =>
              {
                  serviceCollection.AddSingleton<ILabProvider>(sp =>
                  {
                      return new LabProvider(
                          sp.GetRequiredService<ILogger<LabProvider>>(),
                          labAPIServiceFactory.CreateClient());
                  });
              });

              return this;
        }

        public  OrderCheckingAPITestServer ArrangeData<TDbContext>(Action<TDbContext> dataFactory)
          where TDbContext : DbContext
        {
            _testServerBuilder.ArrangeData<TDbContext>(dataFactory);
            return this;
        }

        public WebApplicationFactory<Gherkin.Core.OrderCheckingAPI.Startup> Build()
        {
            var result = _testServerBuilder.Build();
            _testServerBuilder = null;

            return result;          
        }
    }
}
