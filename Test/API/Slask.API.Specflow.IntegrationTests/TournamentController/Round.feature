Feature: TournamentControllerRound
	Makes sure all api calls for the tournament controller relatiing to rounds are running correctly

Background: 
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |

@TournamentControllerRoundTag
Scenario: Can add bracket round to tournament
	When PUT request is sent to "api/tournaments/IdReplacement0/rounds"
		| IdReplacement0 | RoundType |
		| GSL 2020       | Bracket   |
	Then response return with status code "204"

Scenario: Can add dual tournament round to tournament
	When PUT request is sent to "api/tournaments/IdReplacement0/rounds"
		| IdReplacement0 | RoundType       |
		| GSL 2020       | Dual tournament |
	Then response return with status code "204"

Scenario: Can add round robin round to tournament
	When PUT request is sent to "api/tournaments/IdReplacement0/rounds"
		| IdReplacement0 | RoundType   |
		| GSL 2020       | Round robin |
	Then response return with status code "204"
