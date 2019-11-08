Feature: UserService
	Does a bunch of tests on UserService
	
	Given, And = To prepare a test
	When = The actual task to test
	Then = What the expected result should be

	Never pair When with And's, and never more than one When

@UserServiceTag
Scenario: Can create user
	Given a UserService has been created
	When a user named "Stålberto" has been created
	Then user should be valid and named "Stålberto"
	
Scenario: Cannot create user with empy name
	Given a UserService has been created
	When a user named "" has been created
	Then user should be invalid

Scenario: Cannot create user with name already in use no matter letter casing
	Given a UserService has been created
		And a user named "Stålberto" has been created
	When a user named "StålBERTO" has been created
	Then user should be invalid

Scenario: Can get user by id
	Given a UserService has been created
		And a user named "Stålberto" has been created
	When getting user by user id
	Then user should be valid and named "Stålberto"

Scenario: Returns null when fetching non-existent user by id
	Given a UserService has been created
		And getting user by user id
	Then user should be invalid

Scenario: Can get user by name no matter letter casing
	Given a UserService has been created
		And a user named "Stålberto" has been created
	When getting user by user name "Stålberto"
	Then user should be valid and named "Stålberto"

Scenario: Returns null when fetching non-existent user by name
	Given a UserService has been created
	When getting user by user name "my-god-thats-jason-bourne"
	Then user should be invalid
