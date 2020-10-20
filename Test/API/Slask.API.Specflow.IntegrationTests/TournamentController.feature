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
	When GET request is sent to fetch tournaments
	Then response return with status code "200"
		And response should contain tournaments "Homestory Cup XX, GSL 2019"