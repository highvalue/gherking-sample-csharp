Feature: DeliveryTime
	Checks the delivery time for an online customer.

@DeliveryTime
Scenario: premium delivery time
	Given a customer payment received 
	And items in-stock in local inventory
	When the delivery time is requested
	Then we have a premium delivery time

@DeliveryTime
Scenario: standard delivery time
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items in-stock in central inventory
	When the delivery time is requested
	Then we have a standard delivery time

@DeliveryTime
Scenario: delayed delivery time
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in just-in-time mode
	When the delivery time is requested
	Then we have a delayed delivery time

@DeliveryTime
Scenario: critical delivery time
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in late mode
	When the delivery time is requested
	Then we have a critical delivery time
	 
@DeliveryTime
Scenario: lost sale
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in refusal mode
	When the delivery time is requested
	Then we have a lost sale
