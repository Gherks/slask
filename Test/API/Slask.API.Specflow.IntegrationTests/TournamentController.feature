Feature: TournamentController
	Makes sure all api calls for the tournament controller are running correctly

@TournamentControllerTag
Scenario: Can create a tournament
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When POST request is sent to create a tournament named "GSL 2020"
	Then response should return with status code "201"