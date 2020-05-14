﻿Feature: Group
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

Scenario: PlayState is set to NotBegun before any match has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain" is registered to round 0
	Then play state of group 0 is set to "NotBegun"
	
Scenario: PlayState is set to IsPlaying when at least one match has started but not all
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
	When score is added to players in given matches in created groups
		| Group index | Match index | Scoring player | Added score |
		| 0           | 0           | Maru           | 1           |
	Then play state of group 0 is set to "IsPlaying"

Scenario: PlayState set to IsFinished when all matches has finished
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
	When score is added to players in given matches in created groups
		| Group index | Match index | Scoring player | Added score |
		| 0           | 0           | Maru           | 2           |
		| 0           | 1           | Taeja          | 2           |
		| 0           | 2           | Maru           | 2           |
	Then play state of group 0 is set to "IsFinished"
