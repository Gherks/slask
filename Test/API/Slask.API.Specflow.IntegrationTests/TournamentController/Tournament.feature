Feature: TournamentController
	Makes sure all api calls for the tournament controller are running correctly

Background: 
	Given Content-Type is set to "application/json" and Accept is set to "application/json"

@TournamentControllerTag
Scenario: Can fetch no tournaments with OK response
	When GET request is sent to "api/tournaments"
	Then response return with status code "200"

Scenario: Can create a tournament
	When POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |
	Then response return with status code "201"

Scenario: Can fetch existing tournaments with OK response
		And POST request is sent to "api/tournaments"
			| TournamentName   |
			| Homestory Cup XX |
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2019       |
		And GET request is sent to "api/tournaments"
		And response return with status code "200"
	When bare tournament DTOs are extracted from response
	Then all bare tournament DTOs should be valid with names "Homestory Cup XX, GSL 2019"
	
Scenario: Can fetch tournament by id
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2019       |
		And GET request is sent to "api/tournaments/IdReplacement0"
			| IdReplacement0 | DtoType0      |
			| GSL 2019       | TournamentDto |
		And response return with status code "200"
	When tournament DTOs are extracted from response
	Then tournament DTO named "GSL 2019" should be valid with 0 betters, 0 rounds, and 0 issues
#		And tournament DTO named "GSL 2020" should contain betters "Stålberto, Bönis, Guggelito, Kimmieboi"
#		And tournament DTO named "GSL 2020" should contain rounds
#			| Contest type | Round name |
#			| Bracket      | Round A    |
#		And tournament DTO named "GSL 2020" should contain groups
#			| Round index | Group index | Contest type | Sort order | Group name |
#			| 0           | 0           | Bracket      | 0          | Group A    |
#		And tournament DTO named "GSL 2020" should contain matches
#			| Round index | Group index | Match index | Sort order | Best of | Player1 name | Player1 score | Player2 name | Player2 score |
#			| 0           | 0           | 0           | 0          | 3       | Maru         | 2             | Stork        | 0             |
#			| 0           | 0           | 1           | 1          | 3       | Taeja        | 2             | Rain         | 0             |
#			| 0           | 0           | 2           | 2          | 3       | Maru         | 0             | Taeja        | 2             |
#		And tournament DTO named "GSL 2020" should contain no issues
#		And tournament DTO named "GSL 2020" should contain better standings
#			| Better name | Points |
#			| Stålberto   | 3      |
#			| Guggelito   | 2      |
#			| Kimmieboi   | 1      |
#			| Bönis       | 0      |

Scenario: Can fetch tournament by name
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2019       |
		And GET request is sent to "api/tournaments/GSL 2019"
		And response return with status code "200"
	When tournament DTOs are extracted from response
	Then tournament DTO named "GSL 2019" should be valid with 0 betters, 0 rounds, and 0 issues
#		And tournament DTO named "GSL 2020" should contain betters "Stålberto, Bönis, Guggelito, Kimmieboi"
#		And tournament DTO named "GSL 2020" should contain rounds
#			| Contest type | Round name |
#			| Bracket      | Round A    |
#		And tournament DTO named "GSL 2020" should contain groups
#			| Round index | Group index | Contest type | Sort order | Group name |
#			| 0           | 0           | Bracket      | 0          | Group A    |
#		And tournament DTO named "GSL 2020" should contain matches
#			| Round index | Group index | Match index | Sort order | Best of | Player1 name | Player1 score | Player2 name | Player2 score |
#			| 0           | 0           | 0           | 0          | 3       | Maru         | 2             | Stork        | 0             |
#			| 0           | 0           | 1           | 1          | 3       | Taeja        | 2             | Rain         | 0             |
#			| 0           | 0           | 2           | 2          | 3       | Maru         | 0             | Taeja        | 2             |
#		And tournament DTO named "GSL 2020" should contain no issues
#		And tournament DTO named "GSL 2020" should contain better standings
#			| Better name | Points |
#			| Stålberto   | 3      |
#			| Guggelito   | 2      |
#			| Kimmieboi   | 1      |
#			| Bönis       | 0      |

Scenario: Can rename tournament by id
		And POST request is sent to "api/tournaments"
			| TournamentName   |
			| Homestory Cup XX |
	When PUT request is sent to "api/tournaments/IdReplacement0"
			| IdReplacement0   | NewName       |
			| Homestory Cup XX | BHA Open 2020 |
	Then response return with status code "204"

Scenario: Can rename tournament by name
		And POST request is sent to "api/tournaments"
			| TournamentName   |
			| Homestory Cup XX |
	When PUT request is sent to "api/tournaments/Homestory Cup XX"
			| NewName       |
			| BHA Open 2020 |
	Then response return with status code "204"

Scenario: Can remove created tournament by id
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |
	When DELETE request is sent to "api/tournaments/IdReplacement0"
			| IdReplacement0 |
			| GSL 2020       |
	Then response return with status code "204"

Scenario: Can remove created tournament by name
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |
	When DELETE request is sent to "api/tournaments/GSL 2020"
	Then response return with status code "204"

Scenario: Can return HEAD response when fetching all tournaments
	When HEAD request is sent to "api/tournaments"
	Then response return with status code "200"

Scenario: Can return HEAD response when fetching specific tournament by id
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |
	When HEAD request is sent to "api/tournaments/IdReplacement0"
		| IdReplacement0 |
		| GSL 2020       |
	Then response return with status code "200"

Scenario: Can return HEAD response when fetching specific tournament by name
		And POST request is sent to "api/tournaments"
			| TournamentName |
			| GSL 2020       |
	When HEAD request is sent to "api/tournaments/GSL 2020"
	Then response return with status code "200"

Scenario: Can provide allowed request types for tournaments endpoint
	When OPTIONS request is sent to "api/tournaments"
	Then response return with status code "200"
		And response header contain endpoints "GET, HEAD, POST, PUT, DELETE, OPTIONS"
