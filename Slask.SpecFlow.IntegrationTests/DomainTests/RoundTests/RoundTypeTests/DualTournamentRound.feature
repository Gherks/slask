Feature: DualTournamentRound
	Does a bunch of tests on dual tournament rounds

@DualTournamentRoundTag
Scenario: Cannot reconfigure best of in dual tournament round when it has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type      | Best of | Advancing per group count | Players per group count |
			| Dual tournament | 3       | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When best of in round 0 is set to 5
	Then best of in round 0 should be 3
