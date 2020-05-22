# Testing Ressources

- [Getting Started with SpecFlow](https://specflow.org/getting-started/)
- [SpecFlow VisualStudio2019 Installer](https://marketplace.visualstudio.com/items?itemName=TechTalkSpecFlowTeam.SpecFlowForVisualStudio)
- [Integration tests in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0)

# Project structure & guidelines
The provided project structure can be used as a template for complex testing scenarios. It separates the test scenarios themselves from tooling and service configurations.
## Factory
- Contains the basic building blocks for the TestServerBuilder, which functions as builder API above the WebApplicationFactory 
- Specific configuration, test data and specialised TestServerBuilder variations for each API

 ## Mocks
Contains general use mock objects

## OrderCheckingAPI
Contains the Specflow feature, hook and step files. Different services/APIs should have separate folders like this. Consider creating separate folder for each feature, so the step files to get convoluted.


## Utils
Contains tooling to make testing easier and reduce boilerplate code when testing services. Most tools are used in the tests.

 **Utils provided in this solution are examples** - they are by no means feature complete, fully documented or guaranteed bug free. 


## Dependency Injection & Startup
In an ASP.NET Core project the Startup.cs handles the DI at startup. For proper testing a DI strategy has to be used. WebApplicationFactory in ASP.NET Core 3.1 handles DI in this order

1) startup.ConfigureServices
2) builder.ConfigureServices
3) builder.ConfigureTestServices

Either
- Make sure startup.ConfigureServices from the project can not fail at startup because of external dependecies.
- Use Environment switches in the startup
- Use a mock Startup
- Create a base class with a virtual overridable method containing the service registrations that must be replaced because they would fail.