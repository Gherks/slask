Feature: PlayerStandingsSolver
	Does a bunch of tests on PlayerStandingsSolver

@PlayerStandingsSolverTag
Scenario: Fetches correctly ordered list of player standing entries
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Players per group count |
			| Round robin | Round robin round | 3       | 8                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When created groups within created tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then player standings in group 0 from first to last should be "Bomber, FanTaSy, Maru, Rain, Stephano, Stork, Taeja, Thorzain"
