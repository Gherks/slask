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
	Then created tournament 0, better 0, should be valid with name: "Stålberto"

Scenario: Can only add user to tournament once
	Given users "Stålberto, Bönis, Guggelito" has been added to tournament with name: "GSL 2019"
	When user "Stålberto" is added to created tournament "GSL 2019"
	Then created tournament 0 should have 3 betters

Scenario: Can fetch all betters in tournament by tournament id
	Given users "Stålberto, Bönis, Guggelito" has been added to tournament with name: "GSL 2019"
	When fetching betters from created tournament 0 by tournament id
	Then created tournament 0, better 0, should be valid with name: "Stålberto"
		And created tournament 0, better 1, should be valid with name: "Bönis"
		And created tournament 0, better 2, should be valid with name: "Guggelito"

Scenario: Can fetch all betters in tournament by tournament name
	Given users "Stålberto, Bönis, Guggelito" has been added to tournament with name: "GSL 2019"
	When fetching betters from tournament by tournament name: "GSL 2019"
	Then created tournament 0, better 0, should be valid with name: "Stålberto"
		And created tournament 0, better 1, should be valid with name: "Bönis"
		And created tournament 0, better 2, should be valid with name: "Guggelito"




#WHEN a bracket round has been added to tournament: "GSL 2019"
#WHEN a dual tournament round has been added to tournament: "GSL 2019"
#WHEN a round robin round has been added to tournament: "GSL 2019"
#WHEN adding 4 groups to round 0 in created tournament 0
#WHEN adding players "Maru, Stork, Taeja, Rain" to group 0, in round 0, in created tournament 0
#
##Round WhenAddedBracketRoundToTournament(Tournament tournament, string name, int bestOf)
##Round WhenAddedDualTournamentRoundToTournament(Tournament tournament, string name, int bestOf)
##Round WhenAddedRoundRobinRoundToTournament(Tournament tournament, string name, int bestOf, int advanceAmount)
##GroupBase WhenAddedGroupToRound(Round round)
##void WhenAddedPlayerReferenceToGroup(GroupBase group, string name)
#
#Scenario: Player references are added to tournament when new players are added to tournament
#	Given a tournament named "GSL 2019" has been created
#	When added players "Maru, Stork, Taeja, Rain" to tournament with name "GSL 2019"
#	Then created tournament 0 should have 4 player references with names: "Maru, Stork, Taeja, Rain"
#
#Scenario: Can fetch all player references in tournament by tournament id
#	Given a tournament named "GSL 2019" has been created
#		And added players "Maru, Stork, Taeja, Rain" to tournament with name "GSL 2019"
#	When fetching player references from created tournament 0 by tournament id
#	Then created tournament 0 should have 4 player references with names: "Maru, Stork, Taeja, Rain"
#
#Scenario: Can fetch all player references in tournament by tournament name
#	Given a tournament named "GSL 2019" has been created
#		And added players "Maru, Stork, Taeja, Rain" to tournament with name "GSL 2019"
#	When fetching player references from tournament by tournament name: "GSL 2019"
#	Then created tournament 0 should have 4 player references with names: "Maru, Stork, Taeja, Rain"
