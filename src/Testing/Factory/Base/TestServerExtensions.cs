using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Gherkin.Testing.Factory.Base
{
    public static class TestServerExtensions
    {
        public static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);            
            return services;
        }        
    }
}
