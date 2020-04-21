Feature: RoundRobinGroup
	Does a bunch of tests on Round robin group

@RoundRobinGroupTag
Scenario: Adding group to round robin round creates bracket group
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name        | Best of | Advancing per group count |
		| Round robin | Round robin round | 3       | 1                         |
	Then group 0 should be valid of type "Round robin"

Scenario: Start time in matches in round robin groups is spaced with one hour upon creation
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round | 3       | 1                         | 8                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then minutes between matches in created group 0 should be 60

Scenario: Creates proper round robin layout upon group creation
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
	When players "First, Second, Third, Fourth" is registered to round 0
	Then participating players in created group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |

Scenario: Round robin progression with four players goes as expected
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type             | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin tournament | Round robin round | 3       | 2                         | 4                       |
	When players "First, Second, Third, Fourth" is registered to round 0
		And created groups within created tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	Then advancing players in created group 0 is exactly "Fourth, First"
		And participating players in created group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | First         | Third         |
			| 1           | Second        | Fourth        |
			| 2           | First         | Fourth        |
			| 3           | Third         | Second        |
			| 4           | First         | Second        |
			| 5           | Fourth        | Third         |

Scenario: Round robin progression with five players goes as expected
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type             | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin tournament | Round robin round | 3       | 3                         | 5                       |
	When players "First, Second, Third, Fourth, Fifth" is registered to round 0
	When created groups within created tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then advancing players in created group 0 is exactly "Fifth, Third, Second"
		And participating players in created group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | First         | Fourth        |
			| 1           | Second        | Fifth         |
			| 2           | Fourth        | Fifth         |
			| 3           | First         | Third         |
			| 4           | Fifth         | Third         |
			| 5           | Fourth        | Second        |
			| 6           | Third         | Second        |
			| 7           | Fifth         | First         |
			| 8           | Second        | First         |
			| 9           | Third         | Fourth        |
			
# Create tests for GetPlayState
#SolveRoundRobinTies