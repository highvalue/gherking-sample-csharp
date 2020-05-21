Feature: LostSales
	Checks different types of lost sales.

Scenario: one lost sale
	Given a customer in physical store
	When shelve is empty
	Then we have a lost sale

Scenario: two lost sales
	Given a customer in physical store
	And customer wants to buy apples and bananas
	When apples shelve is empty
	And bananas shelve is empty
	Then we have a two lost sales

Scenario: three-strikes-and-you’re-out
	Given a customer in physical store
	And customer wants to buy apples and bananas and tomatoes
	When apples shelve is empty
	And bananas shelve is empty
	And tomatoes shelve is empty
	Then we have a lost customer
	