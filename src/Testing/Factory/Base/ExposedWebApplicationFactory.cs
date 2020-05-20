using System;
using System.Collections.Generic;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Gherkin.Testing.Factory.Base
{
    class ExposedWebApplicationFactory<TStartUp> : WebApplicationFactory<TStartUp> where TStartUp : class
    {
        private readonly IEnumerable<Action<IServiceCollection>> _configureTestServices;

        public ExposedWebApplicationFactory(IEnumerable<Action<IServiceCollection>> configureTestServices): base()
        {
            _configureTestServices = configureTestServices;
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseStartup<TStartUp>();            
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(".");
            base.ConfigureWebHost(builder);

            builder.ConfigureTestServices(collection =>
            {
                foreach(var testService in _configureTestServices)
                {
                    testService.Invoke(collection);
                }               
            });
        }
    }
}
