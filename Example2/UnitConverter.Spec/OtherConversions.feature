Feature: Beyond Distance and Temperature
	In order to use the converter appliction
	As a WP7 user
	I want to convert Speed, Time, Volume, Angle, Weight, Area

Scenario: I can change to Speed
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
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I tap in the middle of the control "From:Units_Speed_MilesPerHour"
	Then I tap in the middle of the control "To:Units_Speed_KilometerPerHour"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	And I see the values
		| name                 | value     |
		| textBlockCategory    | SPEED     |
		| textBlockInputValue  | 24        |
		| textBlockResultValue | 38.624256 |
		| textBlockInputUnit   | MPH       |
		| textBlockResultUnit  | Km/H      |


Scenario: I can change to Time
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
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I tap in the middle of the control "From:Units_Time_Hours"
	Then I tap in the middle of the control "To:Units_Time_Seconds"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	And I see the values
		| name                 | value   |
		| textBlockCategory    | TIME    |
		| textBlockInputValue  | 24      |
		| textBlockResultValue | 86400   |
		| textBlockInputUnit   | Hours   |
		| textBlockResultUnit  | Seconds |


Scenario: I can change to Volume
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
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I tap in the middle of the control "From:Units_Volume_Quarts"
	Then I tap in the middle of the control "To:Units_Volume_Liters"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | VOLUME      |
		| textBlockInputValue  | 24          |
		| textBlockResultValue | 22.71247068 |
		| textBlockInputUnit   | Quarts      |
		| textBlockResultUnit  | Liters      |


Scenario: I can change to Angle
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
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I tap in the middle of the control "From:Units_Angle_Radians"
	Then I tap in the middle of the control "To:Units_Angle_Degrees"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	And I see the values
		| name                 | value       |
		| textBlockCategory    | ANGLE       |
		| textBlockInputValue  | 24          |
		| textBlockResultValue | 1375.098708 |
		| textBlockInputUnit   | Radians     |
		| textBlockResultUnit  | Degrees     |



Scenario: I can change to Weight
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
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I tap in the middle of the control "From:Units_Weight_MetricTons"
	Then I tap in the middle of the control "To:Units_Weight_Grams"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	#Is this a bug?!
	And I see the values
		| name                 | value       |
		| textBlockCategory    | WEIGHT      |
		| textBlockInputValue  | 24          |
		| textBlockResultValue | 24000000.02 |
		| textBlockInputUnit   | MTons       |
		| textBlockResultUnit  | Grams       |

Scenario: I can change to Area
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
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I move the pivot right
	Then I wait 1 seconds
	Then take a picture
	Then I tap in the middle of the control "From:Units_Area_Acre"
	Then I tap in the middle of the control "To:Units_Area_SqMeter"
	Then take a picture
	Then I press the control "buttonDone"
	Then I wait 1 seconds
	Then take a picture
	#Is this a bug?!
	And I see the values
		| name                 | value       |
		| textBlockCategory    | AREA        |
		| textBlockInputValue  | 24          |
		| textBlockResultValue | 97124.55443 |
		| textBlockInputUnit   | Acre        |
		| textBlockResultUnit  | SqM         |

