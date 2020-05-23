﻿Feature: AdvancingPlayersSolver
	Does a bunch of tests on AdvancingPlayersSolver

@AdvancingPlayersSolverTag
Scenario: Can fetch advancing players from finished round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 5                         | 8                       |
			| Bracket     | Bracket round     | 3       | 1                         | 5                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then advancing players from round 0 should be "Bomber, FanTaSy, Maru, Rain, Stephano"

Scenario: Can fetch advancing players from finished group
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 2                         | 4                       |
			| Bracket     | Bracket round     | 3       | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then advancing players from group 0 should be "Maru, Rain"

Scenario: Cannot fetch advancing players from round that is unfinished
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 2                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then advancing players from round 0 should be ""

Scenario: Cannot fetch advancing players from group that is unfinished
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 2                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then advancing players from group 1 should be ""
