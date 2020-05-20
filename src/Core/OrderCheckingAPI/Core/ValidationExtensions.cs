using System;
using System.Collections.Generic;
using Gherkin.Contract.OrderCheckingAPI;

namespace Gherkin.Core.OrderCheckingAPI.Core
{
    public static class ValidationExtensions
    {
        public static void Validate(this CheckOrderOnStockCommand checkOrder, List<string> validLabs)
        {
            // check if we have a valid lab          

            if (!validLabs.Contains(checkOrder.Lab))
            {
                throw new ArgumentOutOfRangeException($"The lab '{checkOrder.Lab}' does not exist.");              
            }            
        }
    }
}
