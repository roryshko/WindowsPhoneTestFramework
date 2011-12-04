Feature: Temperature
	In order to use the converter appliction
	As a WP7 user
	I want to switch to Temperature, enter some numbers and see the results

Scenario: I can change to Temperature
	Given my app is clean installed and running
	Then take a picture
	And I see the control "textBlockCategory" has value "LENGTH"
	Then I press the control "btnConversions"
	Then I wait 1 seconds
	Then take a picture
	Then I wait for the control "pivot" to appear
	Then I flick "RightToLeft"
	Then I wait 1 seconds
	Then I tap in the middle of the control "From:Units_Temp_Kelvin"
	Then I tap in the middle of the control "To:Units_Temp_Fahrenheit"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	And I see the values
		| name                 | value      |
		| textBlockCategory    | TEMP       |
		| textBlockInputValue  | 0          |
		| textBlockResultValue | -459.67    |
		| textBlockInputUnit   | Kelvin     |
		| textBlockResultUnit  | Fahrenheit |

Scenario: Data is not cleared when changing to temperature
	Given my app is clean installed and running
	Then take a picture
	And I see the control "textBlockCategory" has value "LENGTH"
	Then I press the control "btnKeypadKey2"
	And I press the control "btnKeypadKey4"
	And I see the values
		| name                 | value  |
		| textBlockCategory    | LENGTH |
		| textBlockInputUnit   | Inches |
		| textBlockResultUnit  | Feet   |
		| textBlockInputValue  | 24     |
		| textBlockResultValue | 2      |
	Then I press the control "btnConversions"
	Then I wait 1 seconds
	Then take a picture
	Then I wait for the control "pivot" to appear
	Then I flick "RightToLeft"
	Then I wait 1 seconds
	Then I tap in the middle of the control "From:Units_Temp_Kelvin"
	Then I tap in the middle of the control "To:Units_Temp_Fahrenheit"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	And I see the values
		| name                 | value      |
		| textBlockCategory    | TEMP       |
		| textBlockInputValue  | 24         |
		| textBlockResultValue | -416.47    |
		| textBlockInputUnit   | Kelvin     |
		| textBlockResultUnit  | Fahrenheit |

