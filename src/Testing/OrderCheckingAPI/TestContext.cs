using Microsoft.AspNetCore.Mvc.Testing;

namespace Gherkin.Testing.OrderCheckingAPI
{
    public class OrderCheckingAPITestContext
    {            
        public WebApplicationFactory<Gherkin.Core.OrderCheckingAPI.Startup> OrderCheckingAPIServerFactory;
    }
}
