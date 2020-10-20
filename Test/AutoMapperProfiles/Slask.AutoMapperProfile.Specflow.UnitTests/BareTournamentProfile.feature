Feature: BareTournamentProfile
	Making sure the bare tournament profile maps bare tournaments correctly with auto-mapper

@BareTournamentProfileTag
Scenario: Can map a domain tournament to a bare tournament DTO
	Given a tournament named "GSL 2019" has been created
	When automapper maps domain tournament "GSL 2019" to a bare tournament DTO
	Then all bare tournament DTOs should be valid