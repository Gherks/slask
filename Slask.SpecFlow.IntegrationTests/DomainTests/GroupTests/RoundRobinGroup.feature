﻿Feature: RoundRobinGroup
	Does a bunch of tests on Round robin group

@RoundRobinGroupTag
Scenario: Adding group to round robin round creates bracket group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing amount |
			| Round robin | Round robin round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Round robin"

Scenario: Start time in matches in round robin groups is spaced with one hour upon creation
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | Advancing amount |
			| Bracket    | Bracket round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60

Scenario: Creates proper round robin layout upon group creation
	# See comment right above 'AssignPlayersToMatches()' in RoundRobinGroup.cs for 
	# explanation of Round robin progression
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

Scenario: Round robin progression with four players goes as expected
	# See comment right above 'AssignPlayersToMatches()' in RoundRobinGroup.cs for 
	# explanation of Round robin progression
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type             | Round name        | Best of | Advancing amount |
			| Round robin tournament | Round robin round | 3       | 2                |
		And group is added to created round 0
		And players "First, Second, Third, Fourth" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is exactly "Fourth, First"
		And pariticpating players in created group 0 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | First         | Third         |
			| 1           | Second        | Fourth        |
			| 2           | First         | Fourth        |
			| 3           | Third         | Second        |
			| 4           | First         | Second        |
			| 5           | Fourth        | Third         |

Scenario: Round robin progression with five players goes as expected
	# See comment right above 'AssignPlayersToMatches()' in RoundRobinGroup.cs for 
	# explanation of Round robin progression
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type             | Round name        | Best of | Advancing amount |
			| Round robin tournament | Round robin round | 3       | 3                |
		And group is added to created round 0
		And players "First, Second, Third, Fourth, Fifth" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is exactly "Fifth, Third, Second"
		And pariticpating players in created group 0 should be mapped accordingly
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