Feature: TournamentControllerBetter
	Makes sure all api calls for the tournament controller regarding betters are running correctly

Background: 
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to "api/users"
			| Username  |
			| Stålberto |
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |

@TournamentControllerBetterTag	
Scenario: Can add better by name to tournament by id
	When PUT request is sent to "api/tournaments/IdReplacement0/betters"
		| IdReplacement0 | UserIdentifier |
		| GSL 2020       | Stålberto      |
	Then response return with status code "204"
	
Scenario: Can add better by name to tournament by name
	When PUT request is sent to "api/tournaments/GSL 2020/betters"
		| UserIdentifier |
		| Stålberto      |
	Then response return with status code "204"

#Scenario: Can remove better from tournament by id 
#		And POST request is sent to create a tournament named "GSL 2020"
#		And POST request is sent to add user to tournament named "Stålberto"
#	When DELETE request is sent to delete better named "Stålberto" by id
#	Then response return with status code "204"
#
#Scenario: Can remove better from tournament by name
#		And POST request is sent to create a tournament named "GSL 2020"
#		And PUT request is sent to add user to tournament named "Stålberto"
#	When DELETE request is sent to delete better named "Stålberto"
#	Then response return with status code "204"
#
#Scenario: Can place a match bet for a better
#		And POST request is sent to create a tournament named "GSL 2020"
#		And POST request is sent to add "Bracket" round to tournament named "GSL 2020"
#		And POST request is sent to add player named "Maru" to tournament named "GSL 2020"
#		And POST request is sent to add player named "Serral" to tournament named "GSL 2020"
#		And PUT request is sent to change start time of match 0 to 0 hours in the future in tournament named "GSL 2020"
#	When POST request is sent to add match bet to tournament named "GSL 2020"
#		| Better    | Match index | Player |
#		| Stålberto | 0           | Maru   |
#	Then response return with status code "204"
