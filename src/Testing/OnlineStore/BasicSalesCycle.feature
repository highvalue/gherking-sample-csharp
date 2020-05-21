Feature: BasicSalesCycle
	Checks the basic steps of the sales cycle.

Scenario: lead
	Given a Customer visiting the OnlineShop
	When Customer puts items into the shopping cart
	Then we have a lead

Scenario: pending sale
	Given a Customer visiting the OnlineShop
	And items in the shopping cart
	When shopping cart session hos NOT timed out
	Then we have a pending sale

Scenario: lost sale
	Given a Customer visiting the OnlineShop
	And items in the shopping cart
	When shopping cart session times out
	Then we have a lost sale

Scenario: a sale
	Given a Customer visiting the OnlineShop
	And items in the shopping cart
	When payment received
	Then we have a sale

Scenario: in-delivery status
	Given a Customer visiting the OnlineShop
	And items in the shopping cart
	When payment received
	And items in-stock
	Then we have an in-delivery status

Scenario: in-delivery status
	Given a Customer visiting the OnlineShop
	And items in the shopping cart
	When payment received
	And items in-stock
	And items have been delivered
	Then we have an been-delivered status

