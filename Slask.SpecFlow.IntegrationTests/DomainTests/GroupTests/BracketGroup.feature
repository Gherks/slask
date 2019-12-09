Feature: BracketGroup
	Does a bunch of tests on Bracket group

@BracketGroupTag
Scenario: Adding group to bracket round creates bracket group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Bracket"

Scenario: Start time in matches in bracket group is spaced with one hour upon creation
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60

Scenario: Bracket progression goes as expected
	#
	#  Match 1	
	# | 1 vs 2 |
	#
	#  Match 2		 Match 5		 Match 7
	# | 3 vs 4 |	| 1 vs 3 |		| 1 vs 5 |
	#
	#  Match 3		 Match 6
	# | 5 vs 6 |	| 5 vs 7 |
	#
	#  Match 4
	# | 7 vs 8 |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is "Maru"


#Scenario: Can construct bracket match layout
#Scenario: Can clear bracket group
