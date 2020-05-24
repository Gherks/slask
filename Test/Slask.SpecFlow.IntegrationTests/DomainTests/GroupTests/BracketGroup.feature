Feature: BracketGroup
	Does a bunch of tests on Bracket group

@BracketGroupTag
Scenario: Adding group to bracket round creates bracket group
	Given a tournament named "GSL 2019" has been created
	When tournament 0 adds rounds
		| Round type | Best of |
		| Bracket    | 3       |
	Then group 0 should be valid of type "Bracket"

Scenario: Start time in matches in bracket group is spaced with one hour upon creation
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 8                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then minutes between matches in group 0 should be 60

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
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 8                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
	Then participating players in group 0 should be mapped accordingly
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
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 5                       |
	When players "First, Second, Third, Fourth, Fifth" is registered to round 0
	Then participating players in group 0 should be mapped accordingly
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
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 7                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh" is registered to round 0
	Then participating players in group 0 should be mapped accordingly
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
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 9                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth" is registered to round 0
	Then participating players in group 0 should be mapped accordingly
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
	# | 5 vs 6 |	| 6 vs 8 |
	#
	#								 Match 7
	#								| 1 vs 6 |
	#
	#  Match 2		 Match 5
	# | 3 vs 4 |	| 1 vs 4 |
	#
	#  Match 1
	# | 1 vs 2 |
	#
	Given a tournament named "GSL 2019" has been created with users "Stålberto, Bönis, Guggelito" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 8                       |
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	Then advancing players in group 0 is exactly "Eighth"
		And participating players in group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | First         | Second        |
			| 1           | Third         | Fourth        |
			| 2           | Fifth         | Sixth         |
			| 3           | Seventh       | Eighth        |
			| 4           | First         | Fourth        |
			| 5           | Fifth         | Eighth        |
			| 6           | First         | Eighth        |
