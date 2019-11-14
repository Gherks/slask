Feature: Group
	Does a bunch of tests on Group

@GroupTag
Scenario: Rounds can create groups
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Bracket         | Bracket round         | 3       | 1                |
		| Dual tournament | Dual tournament round | 3       | 1                |
		| Round robin     | Round robin round     | 3       | 1                |
	When created rounds 0 to 2 creates 1 groups each
	Then created rounds 0 to 2 should contain 1 groups each

Scenario: Adding group to bracket round creates bracket group
	Given a tournament creates rounds
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Bracket"

Scenario: Adding group to dual tournament round creates bracket group
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Dual tournament"

Scenario: Adding group to round robin round creates bracket group
	Given a tournament creates rounds
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Round robin"

Scenario: Start time in matches in round robin groups is spaced with one hour upon creation
	Given a tournament creates rounds
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60

Scenario: Start time in matches in dual tournament group is spaced with one hour upon creation
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60

Scenario: Start time in matches in bracket group is spaced with one hour upon creation
	Given a tournament creates rounds
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60

Scenario: Player reference is added to tournament when new player is added to group
	Given a tournament creates rounds
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain" is added to created group 0
	Then created tournament 0 should have 4 player references with names: "Maru, Stork, Taeja, Rain"
	
# Given a bracket round has been played out
# Given a dual tournament round has been played out
# Given a round robin round has been played out

#Scenario: Cannot add new player to groups not within first round
#Scenario: Can undo finished matches ''''concurrently'''' (Things to undo: Better scores, player scores, 

#Scenario: Cannot add new player to group after first match has started
#Scenario: Cannot increase score before match has started in round robin group
#Scenario: Cannot increase score before match has started in dual tournament group
#Scenario: Cannot increase score before match has started in bracket group
#Scenario: Can clear round robin group of matches - unit test?
#Scenario: Can clear dual tournament group of matches - unit test?
#Scenario: Can clear bracket group of matches - unit test?
#
#// ALL MATCHES MUST BE ORDERED DESCENDING BY STARTDATETIME
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
