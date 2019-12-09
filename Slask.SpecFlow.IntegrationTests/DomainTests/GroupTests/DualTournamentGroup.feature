﻿Feature: DualTournamentGroup
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

Scenario: Dual tournament progression goes as expected
	#
	# First player in all matches is the winner in this example
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
		And players "Maru, Stork, Taeja, Rain" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is exactly "Maru, Taeja"
#
#Scenario: Can clear dual tournament group
#
#Scenario: Can undo steps in
#
#Scenario: Creates proper dual tournament layout upon group creation