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

Scenario: Creates proper bracket layout upon group creation
	#
	#  Match 4
	# | 7 vs 8 |
	#
	#  Match 3		 Match 6
	# | 5 vs 6 |	| - vs - |
	#
	#								 Match 7
	#								| - vs - |
	#
	#  Match 2		 Match 5
	# | 3 vs 4 |	| - vs - |
	#
	#  Match 1
	# | 1 vs 2 |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is added to created group 0
	Then pariticpating players in created group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |
		| 2           | Fifth         | Sixth         |
		| 3           | Seventh       | Eighth        |

Scenario: When bracket has five participants the first match should contain two players and parent should contain one
	#
	#  Match 1		 Match 3		 Match 4
	# | 1 vs 2 |	| 5 vs - |		| - vs - |
	#
	#				 Match 2
	#				| 3 vs 4 |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "First, Second, Third, Fourth, Fifth" is added to created group 0
	Then pariticpating players in created group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |
		| 2           | Fifth         |               |

Scenario: When bracket has seven participants the third match should contain two players and parent should contain one
	#
	#  Match 3
	# | 5 vs 6 |
	#
	#  Match 2		 Match 5
	# | 3 vs 4 |	| - vs - |
	#
	#								 Match 6
	#								| - vs - |
	#
	#  Match 1		 Match 4
	# | 1 vs 2 |	| 7 vs - |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh" is added to created group 0
	Then pariticpating players in created group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |
		| 2           | Fifth         | Sixth         |
		| 3           | Seventh       |               |

Scenario: When bracket has nine participants the first match should contain two players and parent should contain one
	#
	#  Match 1		 Match 2
	# | 1 vs 2 |	| 9 vs - |
	#
	#				 Match 4		 Match 7
	#				| 7 vs 8 |		| - vs - |
	#
	#											 Match 8
	#											| - vs - |
	#
	#				 Match 3		 Match 6
	#				| 5 vs 6 |		| - vs - |
	#
	#				 Match 2
	#				| 3 vs 4 |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth" is added to created group 0
	Then pariticpating players in created group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Second        |
		| 1           | Third         | Fourth        |
		| 2           | Fifth         | Sixth         |
		| 3           | Seventh       | Eighth        |
		| 4           | Ninth         |               |

Scenario: Bracket progression goes as expected
	#
	#  Match 4
	# | 7 vs 8 |
	#
	#  Match 3		 Match 6
	# | 5 vs 6 |	| 5 vs 7 |
	#
	#								 Match 7
	#								| 1 vs 5 |
	#
	#  Match 2		 Match 5
	# | 3 vs 4 |	| 1 vs 3 |
	#
	#  Match 1
	# | 1 vs 2 |
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is exactly "First"
		And pariticpating players in created group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 4           | First         | Third         |
			| 5           | Fifth         | Seventh       |
			| 6           | First         | Fifth         |

#Scenario: Can clear bracket group
# Create tests for GetPlayState