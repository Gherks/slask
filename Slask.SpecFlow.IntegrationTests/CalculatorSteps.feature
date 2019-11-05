Feature: CalculatorSteps
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given I have entered 50 into the calculator
	And I have also entered 70 into the calculator
	When I press add
	Then the result should be 120 on the screen

Scenario: Subract two numbers
	Given I have entered 70 into the calculator
	And I have also entered 50 into the calculator
	When I press subract
	Then the result should be 20 on the screen
