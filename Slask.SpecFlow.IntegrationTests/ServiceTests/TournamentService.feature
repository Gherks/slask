Feature: TournamentService
	Does a bunch of tests on TournamentService

@TournamentServiceTag
Scenario: Can create tournament
	When a tournament named "GSL 2019" has been created
	Then created tournament 0 should be valid with name: "GSL 2019"
	
Scenario: Cannot create tournament with empty name
	When a tournament named "" has been created
	Then created tournament 0 should be invalid

Scenario: Cannot create tournament with name already in use no matter letter casing
	Given a tournament named "GSL 2019" has been created
	When a tournament named "Gsl 2019" has been created
	Then created tournament 1 should be invalid

Scenario: Can rename tournament
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 is renamed to: "Homestory Cup XX"
	Then created tournament 0 should be valid with name: "Homestory Cup XX"

Scenario: Cannot rename tournament to empty name
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 is renamed to: ""
	Then created tournament 0 should be valid with name: "GSL 2019"

Scenario: Cannot rename tournament to name already in use no matter letter casing
	Given a tournament named "GSL 2019" has been created
		And a tournament named "Homestory Cup XX" has been created
	When created tournament 0 is renamed to: "HomeSTORY Cup XX"
	Then created tournament 0 should be valid with name: "GSL 2019"

Scenario: Cannot rename non-existing tournament
	When fetching created tournament by tournament id: 00000000-0000-0000-0000-000000000000
	Then fetched tournament 0 should be invalid

Scenario: Can fetch tournament by id
	Given a tournament named "GSL 2019" has been created
	When fetching created tournament 0 by tournament id
	Then fetched tournament 0 should be valid with name: "GSL 2019"

Scenario: Can fetch tournament by name no matter letter casing
	Given a tournament named "GSL 2019" has been created
	When fetching created tournament by tournament name: "Gsl 2019"
	Then fetched tournament 0 should be valid with name: "GSL 2019"

Scenario: Can add user to tournament with user service
	Given a user named "Stålberto" has been created
		And a tournament named "GSL 2019" has been created
	When users "Stålberto" is added to created tournament "GSL 2019"
	Then created tournament 0 should contain valid betters with names: "Stålberto"

Scenario: Can only add user to tournament once
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
	When users "Stålberto" is added to created tournament "GSL 2019"
	Then better amount in created tournament 0 should be 3

Scenario: Can fetch all betters in tournament by tournament id
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
	When fetching betters from created tournament 0 by tournament id
	Then created tournament 0 should contain valid betters with names: "Stålberto, Bönis, Guggelito"

Scenario: Can fetch all betters in tournament by tournament name
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
	When fetching betters from tournament by tournament name: "GSL 2019"
	Then created tournament 0 should contain valid betters with names: "Stålberto, Bönis, Guggelito"

Scenario: Can fetch all player references in tournament by tournament id
	When a tournament named "GSL 2019" with player references "Maru, Stork, Taeja, Rain" added to it
	Then created tournament 0 should contain exactly these player references with names: "Maru, Stork, Taeja, Rain"

Scenario: Can fetch all player references in tournament by tournament name
	Given a tournament named "GSL 2019" with player references "Maru, Stork, Taeja, Rain" added to it
	When fetching player references from tournament by tournament name: "GSL 2019"
	Then fetched player references should be exactly these player references with names: "Maru, Stork, Taeja, Rain"
