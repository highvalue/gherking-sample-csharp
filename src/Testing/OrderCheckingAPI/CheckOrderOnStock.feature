Feature: CheckOrderOnStock
	The FVS Check is used to clarifiy, if an 'RX order' has to be created or the order remains a stock order.
	This is achieved by checking the combination of OPC and Lab in table x.

	########################################################################
	# Scenario 1 : FVS Check by Lab - Happy Path
	########################################################################
 @OrderCheckingAPI
Scenario: FVS Check by Lab
	Given OPC="<OPC>" is on stock for Lab="<Lab>"
	When the FSV Check is performed with OPC="<OPC>" and Lab="<Lab>"
	Then the order remains a stock order 
	
	Examples: 
	| OPC             | Lab        |
	| 0005770284      |ID 3 DE     |
	| 0005770285      |ID 6 IN     |


    ########################################################################
	# Scenario 2 : FVS Check by Lab - Sad Path
	########################################################################
 @OrderCheckingAPI
Scenario: FVS Check by Lab - Sad Path
	Given OPC="<OPC>" is not on stock for Lab="<Lab>"
	When the FSV Check is performed with OPC="<OPC>" and Lab="<Lab>"
	Then the order must be changed to a RX order 
	
	Examples: 
	| OPC             | Lab        |
	| 0005770289      |ID 3 DE     |
	| 0005770280      |ID 6 IN     |