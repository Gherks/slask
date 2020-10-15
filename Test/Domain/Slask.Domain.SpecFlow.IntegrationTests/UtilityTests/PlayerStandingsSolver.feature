Feature: PlayerStandingsSolver
	Does a bunch of tests on PlayerStandingsSolver

@PlayerStandingsSolverTag
Scenario: Fetches correctly ordered list of player standing entries
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| RoundRobin   | 1                         | 8                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then player standings in group 0 from first to last should be "Bomber, FanTaSy, Maru, Rain, Stephano, Stork, Taeja, Thorzain"
