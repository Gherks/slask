Feature: UserController
	Makes sure all api calls for the user controller are running correctly

Background: 
	Given Content-Type is set to "application/json" and Accept is set to "application/json"

@UserControllerTag
Scenario: Can create a user
	When POST request is sent to create a user named "Stålberto"
	Then response return with status code "201"

Scenario: Can fetch no users with OK response
	When GET request is sent to fetch users
	Then response return with status code "200"

Scenario: Can fetch existing users with OK response
		And POST request is sent to create a user named "Stålberto"
		And POST request is sent to create a user named "Bönis"
	When GET request is sent to fetch users
	Then response return with status code "200"
		And response should contain users "Stålberto, Bönis"

Scenario: Can fetch created user by name
		And POST request is sent to create a user named "Stålberto"
	When GET request is sent to fetch user named "Stålberto"
	Then response return with status code "200"
		And response should contain users "Stålberto"

Scenario: Can fetch created user by id
		And POST request is sent to create a user named "Stålberto"
	When GET request is sent to fetch user named "Stålberto" by user id
	Then response return with status code "200"
		And response should contain users "Stålberto"
