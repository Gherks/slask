Feature: Better
	Does a bunch of tests on Betters

@BetterTag
Scenario: Can fetch correct better standings from tournament
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito, Kimmieboi" added to it
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
			| Bracket    | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 0           | 0           | 0           | Maru        |
			| Stålberto   | 0           | 0           | 1           | Taeja       |
			| Stålberto   | 0           | 1           | 0           | Bomber      |
			| Stålberto   | 0           | 1           | 1           | Stephano    |
			| Bönis       | 0           | 0           | 0           | Maru        |
			| Bönis       | 0           | 0           | 1           | Taeja       |
			| Bönis       | 0           | 1           | 0           | FanTaSy     |
			| Bönis       | 0           | 1           | 1           | Thorzain    |
			| Guggelito   | 0           | 0           | 0           | Stork       |
			| Guggelito   | 0           | 0           | 1           | Rain        |
			| Guggelito   | 0           | 1           | 0           | FanTaSy     |
			| Guggelito   | 0           | 1           | 1           | Stephano    |
			| Kimmieboi   | 0           | 0           | 0           | Stork       |
			| Kimmieboi   | 0           | 0           | 1           | Rain        |
			| Kimmieboi   | 0           | 1           | 0           | Bomber      |
			| Kimmieboi   | 0           | 1           | 1           | Thorzain    |
	When groups within tournament is played out
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	Then player standings in tournament 0 from first to last looks like this
		| Better name | Points |
		| Stålberto   | 3      |
		| Guggelito   | 2      |
		| Kimmieboi   | 2      |
		| Bönis       | 1      |
