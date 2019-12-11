Feature: Group
	Does a bunch of tests on Group

@GroupTag
Scenario: Rounds can create groups
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing amount |
			| Bracket         | Bracket round         | 3       | 1                |
			| Dual tournament | Dual tournament round | 3       | 1                |
			| Round robin     | Round robin round     | 3       | 1                |
	When created rounds 0 to 2 creates 1 groups each
	Then created rounds 0 to 2 should contain 1 groups each

Scenario: Player reference is added to tournament when new player is added to group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain" is added to created group 0
	Then created tournament 0 should contain exactly these player references with names: "Maru, Stork, Taeja, Rain"

#Scenario: Cannot add new player to groups not within first round
#Scenario: Cannot add new player to group after first match has started
#Scenario: Cannot increase score before match has started in round robin group
#Scenario: Cannot increase score before match has started in dual tournament group
#Scenario: Cannot increase score before match has started in bracket group
#Scenario: Can clear round robin group of matches - unit test?
#Scenario: Can clear dual tournament group of matches - unit test?
#Scenario: Can clear bracket group of matches - unit test?
#
#
#// Create tests for GetPlayState
#
#// CAN CHANGE LAST EXISTING PLAYER REF TO NULL AND IT IS REMOVED FROM GROUP
#// CAN CHANGE LAST EXISTING PLAYER REF TO ANOTHER PLAYER REF
#
#
#Scenario: Fetching advancing players in round robin group returns at least number of players set by parent round
#Scenario: Fetching advancing players in round robin group returns all players if number of advancing players is greater than players participating
#Scenario: Fetching advancing players in dual tournament group only returns top two players
#Scenario: Fetching advancing players in bracket group only returns bracket winner
#Scenario: Cannot fetch advancing players before group is played out
#Scenario: Player is removed from participant list when not assigned to a single match