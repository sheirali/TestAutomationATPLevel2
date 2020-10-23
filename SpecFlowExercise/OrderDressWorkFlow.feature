@chrome
Feature: Order Dress WorkFlow
	As an online shopper
	I want to be able to compare and order dresses

Scenario: Add dresses to Compare
	When I navigate to Dress Page
	And add dresses
	| Url                                       | ProductId |
	| /index.php?controller=product&id_product= | 3         |
	| /index.php?controller=product&id_product= | 4         |
	| /index.php?controller=product&id_product= | 5         |
	Then assert 3 dresses added to compare

Scenario: QuickView dresses
	When I navigate to Dress Page
	And pick dresses for quickView
	| ProductId |
	| 4         |

