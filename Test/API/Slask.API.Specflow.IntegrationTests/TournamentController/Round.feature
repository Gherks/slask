Feature: TournamentControllerRound
	Makes sure all api calls for the tournament controller relatiing to rounds are running correctly

Background: 
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |

@TournamentControllerRoundTag
Scenario: Can add bracket round to tournament
	When PUT request is sent to "api/tournaments/IdReplacement/rounds"
		| IdReplacement | DtoType       | RoundType |
		| GSL 2020      | TournamentDto | Bracket   |
	Then response return with status code "204"

Scenario: Can add dual tournament round to tournament
	When PUT request is sent to "api/tournaments/IdReplacement/rounds"
		| IdReplacement | DtoType       | RoundType       |
		| GSL 2020      | TournamentDto | Dual tournament |
	Then response return with status code "204"

Scenario: Can add round robin round to tournament
	When PUT request is sent to "api/tournaments/IdReplacement/rounds"
		| IdReplacement | DtoType       | RoundType   |
		| GSL 2020      | TournamentDto | Round robin |
	Then response return with status code "204"
