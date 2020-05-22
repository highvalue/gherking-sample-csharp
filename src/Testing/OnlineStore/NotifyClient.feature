@ignore
Feature: NotifyClient
	Notifies the client when supplier changes its mode.

Scenario: supplier from just-in-time to late 
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in just-in-time mode
	When supplier changes mode from just-in-time to late 
	Then we change delivery time from delayed to critical
	And notify the customer

Scenario: supplier from just-in-time to refusal 
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in just-in-time mode
	When supplier changes mode from just-in-time to refusal 
	Then we change delivery time from delayed to canceled
	And refund the items
	And notify the customer

Scenario: supplier from late to refusal 
	Given a customer payment received 
	And items out-of-stock in local inventory
	And items out-of-stock in central inventory
	And supplier in late mode
	When supplier changes mode from late to refusal 
	Then we change delivery time from delayed to canceled
	And refund the items
	And notify the customer

