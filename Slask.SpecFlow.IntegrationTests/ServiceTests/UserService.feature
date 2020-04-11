Feature: UserService
	Does a bunch of tests on UserService

@UserServiceTag
Scenario: Can create user
	When a user named "Stålberto" has been created
	Then created user 0 should be valid with name: "Stålberto"
	
Scenario: Cannot create user with empy name
	When a user named "" has been created
	Then created user 0 should be invalid

Scenario: Cannot create user with name already in use no matter letter casing
	Given a user named "Stålberto" has been created
	When a user named "StålBERTO" has been created
	Then created user 1 should be invalid

Scenario: Can fetch user by id
	Given a user named "Stålberto" has been created
	When fetching created user 0 by user id
	Then fetched user 0 should be valid with name: "Stålberto"

Scenario: Returns null when fetching non-existent user by id
	When fetching user with user id: 00000000-0000-0000-0000-000000000000
	Then fetched user 0 should be invalid

Scenario: Can fetch user by name no matter letter casing
	Given a user named "Stålberto" has been created
	When fetching user by user name: "StålBeRTO"
	Then fetched user 0 should be valid with name: "Stålberto"

Scenario: Returns null when fetching non-existent user by name
	When fetching user by user name: "my-god-thats-jason-bourne"
	Then fetched user 0 should be invalid
