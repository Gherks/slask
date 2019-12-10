Feature: RoundRobinGroup
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

Scenario: Round robin tournament progression goes as expected
	#
	#
	#
	Given a tournament named "GSL 2019" with users "Stålberto, Bönis, Guggelito" added to it
		And created tournament 0 adds rounds
			| Round type             | Round name        | Best of | Advancing amount |
			| Round robin tournament | Round robin round | 3       | 3                |
		And group is added to created round 0
		And players "Maru, Stork, Taeja, Rain, Bomber" is added to created group 0
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then advancing players in created group 0 is exactly "Bomber, Taeja, Stork"