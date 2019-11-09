﻿Feature: TournamentService
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
	When fetching tournament with tournament id: 00000000-0000-0000-0000-000000000000
	Then fetched tournament 0 should be invalid

Scenario: Can fetch tournament by id
	Given a tournament named "GSL 2019" has been created
	When fetching created tournament 0 by tournament id
	Then fetched tournament 0 should be valid with name: "GSL 2019"

Scenario: Can fetch tournament by name no matter letter casing
	Given a tournament named "GSL 2019" has been created
	When fetching tournament by tournament name: "Gsl 2019"
	Then fetched tournament 0 should be valid with name: "GSL 2019"

Scenario: Can add user to tournament with user service
	Given a user named "Stålberto" has been created
		And a tournament named "GSL 2019" has been created
	When user "Stålberto" is added to created tournament "GSL 2019"
	Then created tournament 0 better 0 should be valid with name: "Stålberto"

#Scenario: Can only add user to tournament once
#
#Scenario: Can fetch all betters in tournament by tournament id
#
#Scenario: Can fetch all betters in tournament by tournament name
#
#Scenario: Player references are added to tournament when new players are added to tournament
#
#Scenario: Can fetch all player references in tournament by tournament id
#
#Scenario: Can fetch all player references in tournament by tournament name
