Feature: Group
	Does a bunch of tests on Group

@GroupTag
Scenario: Rounds can create groups
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Bracket         | Bracket round         | 3       | 1                |
		| Dual tournament | Dual tournament round | 3       | 1                |
		| Round robin     | Round robin round     | 3       | 1                |
	When created rounds 0 to 2 creates 1 groups each
	Then created rounds 0 to 2 should contain 1 groups each

Scenario: Adding group to bracket round creates bracket group
	Given a tournament creates rounds
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Bracket"

Scenario: Adding group to dual tournament round creates bracket group
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 1                |
	When group is added to created round 0
	Then group 0 should be valid of type "Dual tournament"

Scenario: Adding group to round robin round creates bracket group
	Given a tournament creates rounds
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 0                |
	When group is added to created round 0
	Then group 0 should be valid of type "Round robin"
