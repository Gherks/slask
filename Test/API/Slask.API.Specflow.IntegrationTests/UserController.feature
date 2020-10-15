Feature: UserController
	Makes sure all api calls for the user controller are running correctly

@UserControllerTag
Scenario: Can create a user
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When POST request is sent to "/api/users" containing body
		| Username  |
		| Stålberto |
	Then response should return with status code "201"

Scenario: Can fetch a user
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When GET request is sent to "/api/users"
	Then response should return with status code "200"
