Feature: UserController
	Makes sure all api calls for the user controller are running correctly

@UserControllerTag
Scenario: Can create a user
	When API POST is called with address "/api/user" containing body
		| Username  |
		| Stålberto |
	Then the POST result should return status code "200"