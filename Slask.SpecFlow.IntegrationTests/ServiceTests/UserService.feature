Feature: UserService
	Does a bunch of tests on UserService

@UserServiceTag
Scenario: Can create user
	Given a UserService has been created
	And a user named "Stålberto" has been created
	Then user should be valid and named "Stålberto"
	
Scenario: Cannot create user with empy name
	Given a UserService has been created
	And a user named "" has been created
	Then user should be null
