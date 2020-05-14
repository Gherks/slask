Feature: Group
	Does a bunch of tests on Groups

@GroupTag
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
