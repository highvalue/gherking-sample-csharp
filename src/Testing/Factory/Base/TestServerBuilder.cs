using Gherkin.Testing.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Gherkin.BuildingBlocks.Utils.Time;

namespace Gherkin.Testing.Factory.Base
{
    public class TestServerBuilder<T> where T : class
    {

        protected List<Action<IServiceCollection>> TestServices { get; set; }
        protected List<Action<IServiceCollection>> MockDbContexts { get; set; }
        protected List<Action<IServiceCollection>> ArrangedData { get; set; }
        protected Action<IWebHostBuilder> WebHostBuilder { get; set; }
        protected string AppSettingsEntryPoint { get; set; }
        protected string AppSettingsTestProject { get; set; }
        protected Uri BaseAddress { get; set; }

        private TestServerBuilder()
        {
            TestServices = new List<Action<IServiceCollection>>();
            MockDbContexts = new List<Action<IServiceCollection>>();
            ArrangedData = new List<Action<IServiceCollection>>();
        }

        public static TestServerBuilder<T> NewTestServer()
        {
            return new TestServerBuilder<T>();
        }

        public static TestServerBuilder<T> NewTestServer(Uri baseAddress)
        {
            return new TestServerBuilder<T>() { BaseAddress = baseAddress };
        }

        public virtual TestServerBuilder<T> AddDefaultTestServices()
        {
            return this;
        }

        ///// <summary>
        ///// For RabbitMQ: Creates a suffix for each exchange so that tests are isolated in the broker
        ///// Marks all exchanges as non-durable and autodelete
        ///// </summary>
        ///// <param name="isolationId"></param>
        ///// <returns></returns>
        //public virtual TestServerBuilder<T> AddIsolationId(string isolationId)
        //{
        //    AddTestService<IOptions<RabbitMQIsolationOptions>>(services =>
        //     services
        //    .AddOptions<RabbitMQIsolationOptions>()
        //    .Configure(x => {
        //        x.IsolationId = isolationId;
        //        x.EnforceTransient = true;
        //    })
        //    );

        //    return this;
        //}

        public virtual TestServerBuilder<T> AddInMemoryDB<TDbContext>(string isolationId)
            where TDbContext : DbContext
        {
            Action<IServiceCollection> action = serviceCollecton =>
            {
                // Remove the app's ApplicationDbContext registration.
                var descriptor = serviceCollecton.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<TDbContext>));

                if (descriptor != null)
                {
                    serviceCollecton.Remove(descriptor);
                }

                serviceCollecton.AddDbContext<TDbContext>(options =>
                  // add inmemory datacontext for persistence
                  // inmemory has no transactions, so we suppress the warning/error, otherwise we would need check in repo
                  options.UseInMemoryDatabase(databaseName: typeof(TDbContext).Name + isolationId.ToString())
                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                   );

            };
            MockDbContexts.Add(action);
            return this;

        }
        public virtual TestServerBuilder<T> PinTimeServer(TimeData timeData)
        {
            AddTestService<ITimeServer>(services =>
             services.AddTransient<ITimeServer>(factory => new MockTimeServer(timeData))
            );

            return this;
        }

        public TestServerBuilder<T> AddTestService<TService>(Action<IServiceCollection> action)
        {
            // automatically remove existing services that are not automatically removed like typed HttpClients
            Action<IServiceCollection> wrapper = serviceCollecton =>
            {
                //serviceCollecton.Remove<TService>();                
                action.Invoke(serviceCollecton);
            };
            TestServices.Add(action);
            return this;
        }

        //public TestServerBuilder<T> AddHttpClient<TService>(Func<TService> service, Func<HttpMessageHandler> messageHandler, Func<IHttpClientBuilder>)
        //{
        //    // automatically remove existing services that are not automatically removed like typed HttpClients
        //    Action<IServiceCollection> wrapper = serviceCollecton =>
        //    {
        //        serviceCollecton.Remove<TService>();
        //        action.Invoke(serviceCollecton);
        //    };
        //    TestServices.Add(action);
        //    return this;
        //}

        public virtual TestServerBuilder<T> WithDefaultHostBuilder()
        {
            WebHostBuilder = builder =>
            {
                // add jsonfile
                if (!string.IsNullOrWhiteSpace(AppSettingsEntryPoint))
                {
                    builder.ConfigureAppConfiguration((context, conf) =>
                    {
                        conf.AddJsonFile(AppSettingsEntryPoint);
                    });
                }

                // adx mock dbcontexts
                if (MockDbContexts.Count > 0)
                {
                    builder.ConfigureServices(serviceCollection =>
                    {
                        foreach (var mockDbContext in MockDbContexts)
                        {
                            mockDbContext.Invoke(serviceCollection);
                        }

                        foreach (var arrangedData in ArrangedData)
                        {
                            arrangedData.Invoke(serviceCollection);
                        }
                    }
                  );
                }

                if (!string.IsNullOrWhiteSpace(AppSettingsTestProject))
                {
                    builder.ConfigureAppConfiguration((context, conf) =>
                    {
                        conf.AddJsonFile(AppSettingsTestProject);
                    });
                }

                //add testservices
                builder.ConfigureTestServices(collection =>
                {
                    foreach (var testService in TestServices)
                    {
                        testService.Invoke(collection);
                    }
                });
            };
            return this;
        }

        public virtual TestServerBuilder<T> WithAppSettingsFromEntryPoint(string appSettingsPath)
        {
            if (!string.IsNullOrWhiteSpace(AppSettingsTestProject)) throw new InvalidOperationException($"'{nameof(AppSettingsTestProject)}' is already configured");
            AppSettingsEntryPoint = appSettingsPath;
            return this;
        }

        public virtual TestServerBuilder<T> WithAppSettingsFromAbsolutePath(string appSettingsPath)
        {
            // this works the same
            if (!string.IsNullOrWhiteSpace(AppSettingsTestProject)) throw new InvalidOperationException($"'{nameof(AppSettingsTestProject)}' is already configured");
            AppSettingsEntryPoint = appSettingsPath;
            return this;
        }

        public virtual TestServerBuilder<T> WithAppSettingsRelativeToProjectOf<TEntryPoint>(string appSettingsPath)
        {
            if (!string.IsNullOrWhiteSpace(AppSettingsEntryPoint)) throw new InvalidOperationException($"'{nameof(AppSettingsEntryPoint)}' is already configured");

            // this actually is the runtime directory i.e.
            // C:\Users\<someUser>\source\repos\CoSyMaNext\Testing\bin\Debug\netcoreapp3.0
            var @assembly = Assembly.GetAssembly(typeof(TEntryPoint));
            var path = Path.GetDirectoryName(@assembly.Location);

            AppSettingsTestProject = Path.Combine(path, appSettingsPath);
            return this;
        }

        public TestServerBuilder<T> WithWebHostBuilder(Action<IWebHostBuilder> webHostBuilder)
        {
            WebHostBuilder = webHostBuilder;
            return this;
        }

        public WebApplicationFactory<T> Build()
        {
            var factory = new WebApplicationFactory<T>()
                .WithWebHostBuilder(WebHostBuilder);

            factory.Server.BaseAddress = BaseAddress;

            Reset();
            return factory;
        }


        public virtual TestServerBuilder<T> ArrangeData<TDbContext>(Action<TDbContext> dataFactory)
            where TDbContext : DbContext
        {
            Action<IServiceCollection> action = serviceCollection =>
            {
                var serviceProvider = serviceCollection.BuildServiceProvider();
                // Remove the app's ApplicationDbContext registration.
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    using var rpContext = scopedServices.GetRequiredService<TDbContext>();
                    rpContext.Database.EnsureCreated();

                    dataFactory.Invoke(rpContext);

                    rpContext.SaveChanges();
                }

            };
            ArrangedData.Add(action);
            return this;
        }
        protected virtual void Reset()
        {
            TestServices = null;
            WebHostBuilder = null;
            AppSettingsEntryPoint = null;
            AppSettingsTestProject = null;
            BaseAddress = null;
        }
    }
}