Feature: Round
	Does a bunch of tests on Round

@RoundTag
Scenario: PlayState is set to NotBegun before any group has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
	Then play state of round 0 is set to "NotBegun"
	
Scenario: PlayState is set to Ongoing when at least one group has started but not all
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
			| Bracket      | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then play state of round 0 is set to "Ongoing"

Scenario: PlayState set to Finished when all groups has finished
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
			| Bracket      | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
	When groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	Then play state of round 0 is set to "Finished"
