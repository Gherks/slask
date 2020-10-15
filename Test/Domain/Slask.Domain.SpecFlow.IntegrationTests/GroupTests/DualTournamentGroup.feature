Feature: DualTournamentGroup
	Does a bunch of tests on DualTournament group

@DualTournamentGroupTag
Scenario: Adding group to dual tournament round creates bracket group
	Given a tournament named "GSL 2019" has been created
	When tournament 0 adds rounds
		| Contest type   | Advancing per group count | Players per group count |
		| DualTournament | 2                         | 4                       |
	Then group 0 should be valid of type "DualTournament"

Scenario: Start time in matches in dual tournament group is spaced with one hour upon creation
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
	Then minutes between matches in group 0 should be 60

Scenario: Creates proper dual tournament layout upon group creation
	#
	#  Match 1		 Winners
	# | 1 vs 2 |	| - vs - |   Decider
	#							| - vs - |
	#  Match 2		 Losers
	# | 3 vs 4 |	| - vs - |
	#
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
	When players "First, Second, Third, Fourth" is registered to tournament 0
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |

Scenario: DualTournament progression goes as expected
	#
	#  Match 1		 Winners
	# | 1 vs 2 |	| 1 vs 4 |   Decider
	#							| 1 vs 3 |
	#  Match 2		 Losers
	# | 3 vs 4 |	| 2 vs 3 |
	#
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
			| Bracket        | 1                         | 2                       |
		And players "First, Second, Third, Fourth" is registered to tournament 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then advancing players in group 0 is exactly "First, Fourth"
		And participating players in group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | First         | Second        |
			| 1           | Third         | Fourth        |
			| 2           | First         | Fourth        |
			| 3           | Second        | Third         |
			| 4           | Fourth        | Second        |
