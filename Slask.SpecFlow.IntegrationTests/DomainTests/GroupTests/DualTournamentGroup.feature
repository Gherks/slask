Feature: DualTournamentGroup
	Does a bunch of tests on Dual tournament group

@DualTournamentGroupTag
Scenario: Adding group to dual tournament round creates bracket group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing amount |
			| Dual tournament | Dual tournament round | 3       | 2                |
	When group is added to created round 0
	Then group 0 should be valid of type "Dual tournament"

Scenario: Start time in matches in dual tournament group is spaced with one hour upon creation
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing amount |
			| Dual tournament | Dual tournament round | 3       | 2                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60

Scenario: Creates proper dual tournament layout upon group creation
	#
	#  Match 1		 Winners
	# | 1 vs 2 |	| - vs - |   Decider
	#							| - vs - |
	#  Match 2		 Losers
	# | 3 vs 4 |	| - vs - |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And group is added to created round 0
	When players "First, Second, Third, Fourth" is added to created group 0
	Then pariticpating players in created group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |

Scenario: Dual tournament progression goes as expected
	#
	#  Match 1		 Winners
	# | 1 vs 2 |	| 1 vs 3 |   Decider
	#							| 3 vs 2 |
	#  Match 2		 Losers
	# | 3 vs 4 |	| 2 vs 4 |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing amount |
			| Dual tournament | Dual tournament round | 3       | 1                |
		And group is added to created round 0
		And players "First, Second, Third, Fourth" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is exactly "First, Third"
		And pariticpating players in created group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | First         | Second        |
			| 1           | Third         | Fourth        |
			| 2           | First         | Third         |
			| 3           | Second        | Fourth        |
			| 4           | Third         | Second        |
#
#Scenario: Can clear dual tournament group
#
#Scenario: Can undo steps in
#
#Scenario: Creates proper dual tournament layout upon group creation
# Create tests for GetPlayState