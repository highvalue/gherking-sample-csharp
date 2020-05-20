using Gherkin.Contract.OrderCheckingAPI;
using Gherkin.Testing.Utils;
using Gherkin.Testing.Utils.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;

namespace Gherkin.Testing.OrderCheckingAPI
{
    [Binding]
    public class CheckOrderOnStockSteps
    {
        private OrderCheckingAPITestContext _testContext;
        private ScenarioContext _scenarioContext;
        public CheckOrderOnStockSteps(OrderCheckingAPITestContext testContext, ScenarioContext scenarioContext)
        {
            _testContext = testContext;
            _scenarioContext = scenarioContext;
        }

        [Given(@"OPC=""(.*)"" is on stock for Lab=""(.*)""")]
        public void GivenOPCIsOnStockForLab(string opc, string lab)
        {
            var existingStockItems = _testContext.OrderCheckingAPIServerFactory
                                .CreateClient()
                                .GetRequest<IEnumerable<StockItem>>("http://localhost:6001/api/OrderChecking","")
                                .Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                                         success: x => x);

            existingStockItems.ShouldNotBeEmpty();
            existingStockItems.ShouldContain(x => (x.Lab == lab && x.OPC == opc));           
        }

        [When(@"the FSV Check is performed with OPC=""(.*)"" and Lab=""(.*)""")]
        public void WhenTheFSVCheckIsPerformedWithOPCAndLab(string opc, string lab)
        {
            var checkResult = _testContext.OrderCheckingAPIServerFactory
                                .CreateClient()
                                .GetRequest<CheckResult>($"http://localhost:6001/api/OrderChecking/{lab}/{opc}", "");
 
            _scenarioContext.Add("result", checkResult);
        }

        [Then(@"the order remains a stock order")]
        public void ThenThenTheOrderRemainsAStockOrder()
        {
            var result = _scenarioContext.Get<Either<RequestResult,CheckResult>>("result");

           var checkResult = result.Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                                         success: x => x);

            checkResult.ShouldNotBeNull();
            checkResult.OnStock.ShouldBeTrue();
        }

        [Given(@"OPC=""(.*)"" is not on stock for Lab=""(.*)""")]
        public void GivenOPCIsNotOnStockForLab(string opc, string lab)
        {
            var existingStockItems = _testContext.OrderCheckingAPIServerFactory
                               .CreateClient()
                               .GetRequest<IEnumerable<StockItem>>("http://localhost:6001/api/OrderChecking", "")
                               .Resolve(fail: f => throw new AssertFailedException($"Expected a positive result, got {f.StatusCode.ToString()}: {f.Content}"),
                                        success: x => x);

            existingStockItems.ShouldNotContain(x => (x.Lab == lab && x.OPC == opc));
        }

        [Then(@"the order must be changed to a RX order")]
        public void ThenTheOrderMustBeChangedToARXOrder()
        {          

            var result = _scenarioContext.Get<Either<RequestResult, CheckResult>>("result");

            var requestResult = result.Resolve(fail: f => f,
                                          success: x => throw new AssertFailedException($"Expected a 404 result!"));
                        
            requestResult.StatusCode.ShouldBe(System.Net.HttpStatusCode.NotFound);
        }


        public void Assert_Long_Running_Background_Result()
        {
            // fake scenario:
            // we wait for a timed backgroundworker to fill the database with stockItems.
            RetryLoop.TriesPerSecond(1).ForSeconds(3).Execute(async () =>
            {
                var httpResponse = await _testContext.OrderCheckingAPIServerFactory
                                .CreateClient()
                                .GetAsync("http://localhost:6001/api/OrderChecking");


                Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);

                var existingStockItems = httpResponse.Content
                .ReadAsStringAsync()
                .Result.FromJsonToType<IEnumerable<StockItem>>();

                existingStockItems.ShouldNotBeEmpty();                
            });
          
        }
    }

}

