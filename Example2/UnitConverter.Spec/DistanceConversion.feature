Feature: Distance
	In order to use the converter appliction
	As a WP7 user
	I want to enter some numbers and see the results

Scenario: Main Page loads with expected default values
	Given my app is clean installed and running
	Then take a picture
	And I see the control "textBlockCategory" has value "LENGTH"
	And I see the values
		| name                 | value  |
		| textBlockCategory    | LENGTH |
		| textBlockInputValue  | 0      |
		| textBlockResultValue | 0      |
		| textBlockInputUnit   | Inches |
		| textBlockResultUnit  | Feet   |

Scenario: Keypad button 5 updates the input and Result fields
	Given my app is clean installed and running
	Then take a picture
	And I see the control "textBlockInputValue" has value "0"
	Then I press the control "btnKeypadKey5"
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | LENGTH      |
		| textBlockInputUnit   | Inches      |
		| textBlockResultUnit  | Feet        |
		| textBlockInputValue  | 5           |
		| textBlockResultValue | 0.416666667 |

Scenario: Multiple keypad buttons update the input fields
	Given my app is clean installed and running
	Then take a picture
	And I see the control "textBlockInputValue" has value "0"
	Then I press the control "btnKeypadKey2"
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | LENGTH      |
		| textBlockInputUnit   | Inches      |
		| textBlockResultUnit  | Feet        |
		| textBlockInputValue  | 2           |
		| textBlockResultValue | 0.166666667 |
	Then I press the control "btnKeypadKey4"
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | LENGTH      |
		| textBlockInputUnit   | Inches      |
		| textBlockResultUnit  | Feet        |
		| textBlockInputValue  | 24           |
		| textBlockResultValue | 2 |
	Then I press the control "btnKeypadKeyPoint"
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | LENGTH      |
		| textBlockInputUnit   | Inches      |
		| textBlockResultUnit  | Feet        |
		| textBlockInputValue  | 24.           |
		| textBlockResultValue | 2 |
	Then I press the control "btnKeypadKey1"
	Then I press the control "btnKeypadKey2"
	Then take a picture
	And I see the values
		| name                 | value  |
		| textBlockCategory    | LENGTH |
		| textBlockInputUnit   | Inches |
		| textBlockResultUnit  | Feet   |
		| textBlockInputValue  | 24.12  |
		| textBlockResultValue | 2.01   |

Scenario: The switch button works
	Given my app is clean installed and running
	Then take a picture
	And I see the control "textBlockInputValue" has value "0"
	Then I press the control "btnKeypadKey5"
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | LENGTH      |
		| textBlockInputUnit   | Inches      |
		| textBlockResultUnit  | Feet        |
		| textBlockInputValue  | 5           |
		| textBlockResultValue | 0.416666667 |
	Then I press the control "btnKeypadSwitchSourceTargetUnit"
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | LENGTH      |
		| textBlockInputUnit   | Feet        |
		| textBlockResultUnit  | Inches      |
		| textBlockInputValue  | 0.416666667 |
		| textBlockResultValue | 5.000000004 |
