Feature: TestsIsolated
	In order to avoid a broken test framework breaking
	parallel testing. I want a test to ensure parallel tests
	won't intefere with each other.

Scenario Outline: Tests don't interfere with each other
	Given my app is clean installed and running
	And I store the value "<value>" in the scenario context
	And I wait 10 seconds
	Then I can read the value "<value>" from the scenario context
	Examples: 
		| value        |
		| Test One     |
		| Test Two     |
		| Test Three A |
		| Test Three B |
		| Test Four    |
		| Test Five    |