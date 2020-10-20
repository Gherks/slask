Feature: TournamentController
	Makes sure all api calls for the tournament controller are running correctly

@TournamentControllerTag
Scenario: Can create a tournament
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When POST request is sent to create a tournament named "GSL 2020"
	Then response return with status code "201"

Scenario: Can fetch no tournaments with OK response
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When GET request is sent to fetch tournaments
	Then response return with status code "200"

Scenario: Can fetch existing tournaments with OK response
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to create a tournament named "Homestory Cup XX"
		And POST request is sent to create a tournament named "GSL 2019"
		And GET request is sent to fetch tournaments
		And response return with status code "200"
	When bare tournament DTOs are extracted from response
	Then all bare tournament DTOs should be valid with names "Homestory Cup XX, GSL 2019"

Scenario: Can fetch created tournament by name
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to create a tournament named "GSL 2019"
		And GET request is sent to fetch tournament named "GSL 2019"
		And response return with status code "200"
	When tournament DTOs are extracted from response
	Then tournament DTO named "GSL 2019" should be valid with 0 betters, 0 rounds, and 0 issues
	
Scenario: Can fetch created tournament by id
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to create a tournament named "GSL 2019"
		And GET request is sent to fetch tournament named "GSL 2019" by user id
		And response return with status code "200"
	When tournament DTOs are extracted from response
	Then tournament DTO named "GSL 2019" should be valid with 0 betters, 0 rounds, and 0 issues
