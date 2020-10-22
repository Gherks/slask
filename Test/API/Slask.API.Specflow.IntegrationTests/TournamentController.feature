Feature: TournamentController
	Makes sure all api calls for the tournament controller are running correctly

Background: 
	Given Content-Type is set to "application/json" and Accept is set to "application/json"

@TournamentControllerTag
Scenario: Can create a tournament
	When POST request is sent to create a tournament named "GSL 2020"
	Then response return with status code "201"

Scenario: Can fetch no tournaments with OK response
	When GET request is sent to fetch tournaments
	Then response return with status code "200"

Scenario: Can fetch existing tournaments with OK response
		And POST request is sent to create a tournament named "Homestory Cup XX"
		And POST request is sent to create a tournament named "GSL 2019"
		And GET request is sent to fetch tournaments
		And response return with status code "200"
	When bare tournament DTOs are extracted from response
	Then all bare tournament DTOs should be valid with names "Homestory Cup XX, GSL 2019"
	
Scenario: Can fetch tournament by id
		And POST request is sent to create a tournament named "GSL 2019"
		And GET request is sent to fetch tournament named "GSL 2019" by user id
		And response return with status code "200"
	When tournament DTOs are extracted from response
	Then tournament DTO named "GSL 2019" should be valid with 0 betters, 0 rounds, and 0 issues

Scenario: Can fetch tournament by name
		And POST request is sent to create a tournament named "GSL 2019"
		And GET request is sent to fetch tournament named "GSL 2019"
		And response return with status code "200"
	When tournament DTOs are extracted from response
	Then tournament DTO named "GSL 2019" should be valid with 0 betters, 0 rounds, and 0 issues
