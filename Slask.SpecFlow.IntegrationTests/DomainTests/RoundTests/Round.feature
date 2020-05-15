Feature: Round
	Does a bunch of tests on Round

@RoundTag
Scenario: Can only exclude player references from groups within first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
			| Bracket    | Bracket round 2 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
		And created groups within created tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	When players "Rain" is excluded from round 1
	Then participating players in round 1 should be exactly "Rain, Thorzain"

Scenario: PlayState is set to NotBegun before any group has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then play state of round 0 is set to "NotBegun"
	
Scenario: PlayState is set to Ongoing when at least one group has started but not all
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
			| Bracket    | Bracket round 2 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When created groups within created tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then play state of round 0 is set to "Ongoing"

Scenario: PlayState set to Finished when all groups has finished
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
			| Bracket    | Bracket round 2 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When created groups within created tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	Then play state of round 0 is set to "Finished"
