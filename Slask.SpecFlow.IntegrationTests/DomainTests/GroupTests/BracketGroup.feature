Feature: BracketGroup
	Does a bunch of tests on bracket groups

@BracketGroupTag
Scenario: Adding group to bracket round creates bracket group
	Given a tournament creates rounds
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Bracket"

Scenario: Start time in matches in bracket group is spaced with one hour upon creation
	Given a tournament creates rounds
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |
		And group is added to created round 0
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is added to created group 0
	Then minutes between matches in created group 0 should be 60


#Scenario: Can construct bracket match layout
#Scenario: Can clear bracket group