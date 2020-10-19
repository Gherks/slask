Feature: UserController
	Makes sure all api calls for the user controller are running correctly

@UserControllerTag
Scenario: Can create a user
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When POST request is sent to create a user named "Stålberto"
	Then response should return with status code "201"

Scenario: Can fetch no users with OK response
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
	When GET request is sent to fetch users
	Then response should return with status code "200"

Scenario: Can fetch existing users with OK response
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to create a user named "Stålberto"
		And POST request is sent to create a user named "Bönis"
	When GET request is sent to fetch users
	Then response should return with status code "200"
		And response should contain users "Stålberto, Bönis"

Scenario: Can fetch created user by name
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to create a user named "Stålberto"
	When GET request is sent to fetch user named "Stålberto"
	Then response should return with status code "200"
		And response should contain users "Stålberto"

Scenario: Can fetch created user by id
	Given Content-Type is set to "application/json" and Accept is set to "application/json"
		And POST request is sent to create a user named "Stålberto"
	When GET request is sent to fetch user named "Stålberto" by user id
	Then response should return with status code "200"
		And response should contain users "Stålberto"
