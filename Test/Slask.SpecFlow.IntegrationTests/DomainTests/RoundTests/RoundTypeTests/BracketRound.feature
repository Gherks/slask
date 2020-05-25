Feature: BracketRound
	Does a bunch of tests on bracket rounds

@BracketRoundTag
Scenario: Cannot reconfigure players per group count in bracket round when it has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When players per group count in round 0 is set to 2
	Then players per group count in round 0 should be 4

Scenario: Cannot reconfigure best of in bracket round when it has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When best of in round 0 is set to 5
	Then best of in round 0 should be 3
