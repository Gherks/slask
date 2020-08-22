Feature: RoundRobinRound
	Does a bunch of tests on round robin rounds

@RoundRobinRoundTag
Scenario: Cannot reconfigure players per group count in round robin round when it has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When players per group count in round 0 is set to 5
	Then players per group count in round 0 should be 4

Scenario: Cannot reconfigure advancing count in round robin round when it has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When advancing per group count in round 0 is set to 2
	Then advancing per group count in round 0 should be 1

Scenario: PlayState is set to Ongoing when round has finished with a problematic tie
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 3                       |
			| Round robin | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When score is added to players in given matches in groups
		| Group index | Match index | Scoring player | Added score |
		| 1           | 0           | Rain           | 2           |
		| 1           | 1           | FanTaSy        | 2           |
		| 1           | 2           | Bomber         | 2           |
	Then play state of round 0 is set to "Ongoing"

Scenario: Can detect several groups with problematic ties
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 3                       |
			| Round robin | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy" is registered to tournament 0
	When score is added to players in given matches in groups
		| Group index | Match index | Scoring player | Added score |
		| 0           | 0           | Maru           | 2           |
		| 0           | 1           | Taeja          | 2           |
		| 0           | 2           | Stork          | 2           |
		| 1           | 0           | Rain           | 2           |
		| 1           | 1           | FanTaSy        | 2           |
		| 1           | 2           | Bomber         | 2           |
	Then round 0 has 2 problematic tie(s)

Scenario: Does not transfer any players to next round when group has problematic ties
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 3                       |
			| Round robin | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy" is registered to tournament 0
	When score is added to players in given matches in groups
		| Group index | Match index | Scoring player | Added score |
		| 0           | 0           | Maru           | 2           |
		| 0           | 1           | Taeja          | 2           |
		| 0           | 2           | Stork          | 2           |
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 1           |
	Then advancing players in group 0 is exactly ""

 Scenario: Can solve tie
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 2                         | 3                       |
			| Bracket     | 1                         | 2                       |
		And players "Maru, Stork, Taeja" is registered to tournament 0
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 0           | 0           | Maru           | 2           |
			| 0           | 1           | Taeja          | 2           |
			| 0           | 2           | Stork          | 2           |
	When tie in group 0 is solved by choosing "Stork"
		And tie in group 0 is solved by choosing "Taeja"
	Then advancing players in group 0 is exactly "Stork, Taeja"
