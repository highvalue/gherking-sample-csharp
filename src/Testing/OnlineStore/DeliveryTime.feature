Feature: DeliveryTimeForOnlineCustomer
	Checks the delivery time for an online customer.

Scenario: premium delivery time
	Given a customer payment received 
	When items in-stock in local inventory
	Then we have a premium delivery time

Scenario: standard delivery time
	Given a customer payment received 
	When items oft-of-stock in local inventory
	And items in-stock in central inventory
	Then we have a standard delivery time

Scenario: delayed delivery time
	Given a customer payment received 
	When items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in just-in-time mode
	Then we have a delayed delivery time

Scenario: critical delivery time
	Given a customer payment received 
	When items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in late mode
	Then we have a critical delivery time

Scenario: lost sale
	Given a customer payment received 
	When items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in refusal mode
	Then we have a lost sale
