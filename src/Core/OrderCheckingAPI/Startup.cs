using Gherkin.Core.OrderCheckingAPI.Core;
using Gherkin.Core.OrderCheckingAPI.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Gherkin.Core.OrderCheckingAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();          
            services.AddScoped<IOrderCheckingService, OrderCheckingService>();
            services.AddScoped<IOrderCheckingRepo, OrderCheckingEfCoreRepo>();
            services.AddHttpClient<ILabProvider, LabProvider>();

            // we replace this DbContext in tests with an inmemory implementation
            services.AddDbContext<OrderCheckingContext>(op =>
            {
                op.UseSqlServer(Configuration.GetValue<string>("MSSql:ConnectionString"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
