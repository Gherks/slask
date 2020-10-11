Feature: UserProfile
	Making sure the user profile maps users correctly with auto-mapper

@UserProfileTag
Scenario: Can map a domain user to a user DTO
	Given users "Stålberto" has been created
	When automapper maps domain user "Stålberto" to a user DTO
	Then all automapped user DTOs should be valid