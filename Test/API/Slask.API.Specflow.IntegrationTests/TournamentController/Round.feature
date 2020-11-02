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

Scenario: Returns correct response when attempting to add non-existent round type to tournament
	When PUT request is sent to "api/tournaments/IdReplacement0/rounds"
		| IdReplacement0 | RoundType      |
		| GSL 2020       | Robert Paulson |
	Then response return with status code "400"

Scenario: Can add round to tournament by name
	When PUT request is sent to "api/tournaments/GSL 2020/rounds"
		| RoundType |
		| Bracket   |
	Then response return with status code "204"

Scenario: Can remove round by id from tournament by id
		And PUT request is sent to "api/tournaments/GSL 2020/rounds"
			| RoundType   |
			| Round robin |
	When DELETE request is sent to "api/tournaments/IdReplacement0/rounds/IdReplacement1"
			| IdReplacement0 | IdReplacement1 |
			| GSL 2020       | Round A        |
	Then response return with status code "204"

Scenario: Can remove round by id from tournament by name
		And PUT request is sent to "api/tournaments/GSL 2020/rounds"
			| RoundType   |
			| Round robin |
	When DELETE request is sent to "api/tournaments/GSL 2020/rounds/IdReplacement0"
			| IdReplacement0 |
			| Round A        |
	Then response return with status code "204"

Scenario: Can remove round by name from tournament by id
		And PUT request is sent to "api/tournaments/GSL 2020/rounds"
			| RoundType   |
			| Round robin |
	When DELETE request is sent to "api/tournaments/IdReplacement0/rounds/Round A"
			| IdReplacement0 |
			| GSL 2020       |
	Then response return with status code "204"

Scenario: Can remove round by name from tournament by name
		And PUT request is sent to "api/tournaments/GSL 2020/rounds"
			| RoundType   |
			| Round robin |
	When DELETE request is sent to "api/tournaments/GSL 2020/rounds/Round A"
	Then response return with status code "204"