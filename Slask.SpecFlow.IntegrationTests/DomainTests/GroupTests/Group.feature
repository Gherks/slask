﻿Feature: Group
	Does a bunch of tests on Groups

@GroupTag
Scenario: Rounds can create groups
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing per group count |
			| Bracket         | Bracket round         | 3       | 1                         |
			| Dual tournament | Dual tournament round | 3       | 1                         |
			| Round robin     | Round robin round     | 3       | 1                         |
	Then created rounds 0 to 2 should contain 1 groups each

Scenario: Player reference is added to tournament when new player is added to group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing per group count |
			| Bracket    | Bracket round | 3       | 1                         |
	When players "Maru, Stork, Taeja, Rain" is registered to round 0
	Then created tournament 0 should contain exactly these player references with names: "Maru, Stork, Taeja, Rain"

Scenario: Cannot add new players to groups not within first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name   | Best of | Advancing per group count | Players per group count |
			| Round robin | First round  | 3       | 4                         | 4                       |
			| Round robin | Second round | 3       | 4                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
	When players "Bomber, FanTaSy, Stephano, Thorzain" is registered to round 1
	Then created group 0 should contain exactly these player references with names: "Maru, Stork, Taeja, Rain"
		And created group 1 should contain exactly these player references with names: ""

#Scenario: Cannot add new player to group after first match has started
#Scenario: Cannot increase score before match has started in round robin group
#Scenario: Cannot increase score before match has started in dual tournament group
#Scenario: Cannot increase score before match has started in bracket group


#// Create tests for GetPlayState
#
#
#
#Scenario: Fetching advancing players in round robin group returns at least number of players set by parent round
#Scenario: Fetching advancing players in round robin group returns all players if number of advancing players is greater than players participating
#Scenario: Fetching advancing players in dual tournament group only returns top two players
#Scenario: Fetching advancing players in bracket group only returns bracket winner
#Scenario: Cannot fetch advancing players before group is played out

# CanOnlyRemovePlayerReferencesFromGroupsWithinFirstRound
