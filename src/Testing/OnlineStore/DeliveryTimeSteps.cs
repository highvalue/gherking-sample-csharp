using Gherkin.Core.CentralHub.Provider;
using Gherkin.Testing.Utils;
using Gherkin.Testing.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Shouldly;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Gherkin.Contract.OnlineStoreApi;
using Gherkin.Contract.Supplier;
using TechTalk.SpecFlow;

namespace Gherkin.Testing.OnlineStore
{
    [Binding]
    public class DeliveryTimeSteps
    {
        private DeliveryTimeTestContext _testContext;
        private ScenarioContext _scenarioContext;

        public DeliveryTimeSteps(DeliveryTimeTestContext testContext, ScenarioContext scenarioContext)
        {
            _testContext = testContext;
            _scenarioContext = scenarioContext;
        }


        //
        // premium delivery time
        //

        [Given(@"a customer payment received")]
        public void GivenACustomerPaymentReceived()
        {
        }
        
        [Given(@"items in-stock in local inventory")]
        public void GivenItemsIn_StockInLocalInventory()
        {
            var localStockItems = new List<LocalStockItem>()
            {
                new LocalStockItem() { Id = 1, CountryCode = "DE", ItemName = "apples", ItemAvailable = true },
                new LocalStockItem() { Id = 2, CountryCode = "DE", ItemName = "bananas", ItemAvailable = true },
                new LocalStockItem() { Id = 3, CountryCode = "DE", ItemName = "tomatoes", ItemAvailable = true }
            };
            _testContext.LocalInventoryContextContainer.GetDbContextAction(context => context.AddRange(localStockItems));
        }

        [When(@"the delivery time is requested")]
        public void WhenTheDeliveryTimeIsRequested()
        {
            var country = "DE";
            var item = "apples";
            var deliveryTimeResult = _testContext.OnlineStoreApiServiceFactory
                .CreateClient()
                .GetRequest<DeliveryTimeResult>($"/api/DeliveryTime/{country}/{item}");

            _scenarioContext.Add("result", deliveryTimeResult);
        }

        [Then(@"we have a premium delivery time")]
        public void ThenWeHaveAPremiumDeliveryTime()
        {
            var result = _scenarioContext.Get<Either<RequestResult, DeliveryTimeResult>>("result");

            var deliveryTimeResult = result.Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                success: x => x);

            deliveryTimeResult.ShouldNotBeNull();
            deliveryTimeResult.DeliveryTime.ShouldBe("premium");
        }
        

        //
        // standard delivery time
        //

        [Given(@"items out-of-stock in local inventory")]
        public void GivenItemsOft_Of_StockInLocalInventory()
        {
            var localStockItems = new List<LocalStockItem>()
            {
                new LocalStockItem() { Id = 1, CountryCode = "DE", ItemName = "apples", ItemAvailable = false },
                new LocalStockItem() { Id = 2, CountryCode = "DE", ItemName = "bananas", ItemAvailable = false },
                new LocalStockItem() { Id = 3, CountryCode = "DE", ItemName = "tomatoes", ItemAvailable = false }
            };
            _testContext.LocalInventoryContextContainer.GetDbContextAction(context => context.AddRange(localStockItems));
        }
        
        [Given(@"items in-stock in central inventory")]
        public void GivenItemsIn_StockInCentralInventory()
        {
            var centralStockItems = new List<CentralStockItem>()
            {
                new CentralStockItem() { Id = 1, ItemName = "apples", ItemAvailable = true },
                new CentralStockItem() { Id = 2, ItemName = "bananas", ItemAvailable = true },
                new CentralStockItem() { Id = 3, ItemName = "tomatoes", ItemAvailable = true }
            };
            _testContext.CentralInventoryContextContainer.GetDbContextAction(context => context.AddRange(centralStockItems));
        }

        [Then(@"we have a standard delivery time")]
        public void ThenWeHaveAStandardDeliveryTime()
        {
            var result = _scenarioContext.Get<Either<RequestResult, DeliveryTimeResult>>("result");

            var deliveryTimeResult = result.Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                success: x => x);

            deliveryTimeResult.ShouldNotBeNull();
            deliveryTimeResult.DeliveryTime.ShouldBe("standard");
        }


        //
        // Supplier in just-in-time mode
        //

        [Given(@"items out-of-stock in central inventory")]
        public void GivenItemsOut_Of_StockInCentralInventory()
        {
            var centralStockItems = new List<CentralStockItem>()
            {
                new CentralStockItem() { Id = 1, ItemName = "apples", ItemAvailable = false },
                new CentralStockItem() { Id = 2, ItemName = "bananas", ItemAvailable = false },
                new CentralStockItem() { Id = 3, ItemName = "tomatoes", ItemAvailable = false }
            };
            _testContext.CentralInventoryContextContainer.GetDbContextAction(context => context.AddRange(centralStockItems));
        }

        [Given(@"supplier in just-in-time mode")]
        public void GivenSupplierInJust_In_TimeMode()
        {
            var supplierModeResult = new SupplierModeResult{SupplierMode = "just-in-time"};
            var json = JsonConvert.SerializeObject(supplierModeResult);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var stringContent = new StringContent(JsonConvert.SerializeObject(supplierModeResult));
            var response = _testContext.SupplierServiceFactory
                .CreateClient()
                .PostAsync("/api/SupplierMode/ArrangeGetSupplierMode", data)
                .GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }

        [Then(@"we have a delayed delivery time")]
        public void ThenWeHaveADelayedDeliveryTime()
        {
            var result = _scenarioContext.Get<Either<RequestResult, DeliveryTimeResult>>("result");

            var deliveryTimeResult = result.Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                success: x => x);

            deliveryTimeResult.ShouldNotBeNull();
            deliveryTimeResult.DeliveryTime.ShouldBe("delayed");
        }

        //
        // Supplier in late mode
        //

        [Given(@"supplier in late mode")]
        public void GivenSupplierInLateMode()
        {
            var supplierModeResult = new SupplierModeResult { SupplierMode = "late" };
            var json = JsonConvert.SerializeObject(supplierModeResult);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var stringContent = new StringContent(JsonConvert.SerializeObject(supplierModeResult));
            var response = _testContext.SupplierServiceFactory
                .CreateClient()
                .PostAsync("/api/SupplierMode/ArrangeGetSupplierMode", data)
                .GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }

        [Then(@"we have a critical delivery time")]
        public void ThenWeHaveACriticalDeliveryTime()
        {
            var result = _scenarioContext.Get<Either<RequestResult, DeliveryTimeResult>>("result");

            var deliveryTimeResult = result.Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                success: x => x);

            deliveryTimeResult.ShouldNotBeNull();
            deliveryTimeResult.DeliveryTime.ShouldBe("critical");
        }

        //
        // Supplier in refusal mode
        //

        [Given(@"supplier in refusal mode")]
        public void GivenSupplierInRefusalMode()
        {
            var supplierModeResult = new SupplierModeResult { SupplierMode = "refusal" };
            var json = JsonConvert.SerializeObject(supplierModeResult);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var stringContent = new StringContent(JsonConvert.SerializeObject(supplierModeResult));
            var response = _testContext.SupplierServiceFactory
                .CreateClient()
                .PostAsync("/api/SupplierMode/ArrangeGetSupplierMode", data)
                .GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();
        }

        [Then(@"we have a lost sale")]
        public void ThenWeHaveALostSale()
        {
            var result = _scenarioContext.Get<Either<RequestResult, DeliveryTimeResult>>("result");

            var deliveryTimeResult = result.Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                success: x => x);

            deliveryTimeResult.ShouldNotBeNull();
            deliveryTimeResult.DeliveryTime.ShouldBe("canceled");
        }
    }
}
