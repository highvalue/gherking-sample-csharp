@ignore
Feature: DeliveryTime
	Checks the delivery time for an physical store customer.

Scenario:  standard delivery time - central
	Given ordering items from central inventory
	When items in-stock in central inventory
	Then items will be available by standard delivery time

Scenario:  standard delivery time - supplier (directly delivered to store)
	Given ordering items from central inventory
	When items out-of-stock in central inventory
	And supplier in just-in-time mode
	Then items will be available by standard delivery time

Scenario:  delayed delivery time
	Given ordering items from central inventory
	When items out-of-stock in central inventory
	And supplier in late mode
	Then items will be available by delayed delivery time
	
Scenario:  critical delivery time
	Given ordering items from central inventory
	When items out-of-stock in central inventory
	And supplier in refusal mode
	Then items will be available by critical delivery time