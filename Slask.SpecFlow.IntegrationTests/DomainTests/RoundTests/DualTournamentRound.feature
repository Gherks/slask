Feature: DualTournamentRound
	Does a bunch of tests on Dual tournament rounds

@DualTournamentRoundTag
Scenario: Can create dual tournament round
	When a tournament creates rounds
		| Round type      | Round name            | Best of |
		| Dual tournament | Dual tournament round | 3       |
	Then created rounds in tournament should be valid with values:
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 2                |

Scenario: Advancing amount in dual tournament rounds cannot be anything other than two
	When a tournament creates rounds
		| Round type      | Round name              | Best of | Advancing amount |
		| Dual tournament | Dual tournament round 1 | 3       | 0                |
		| Dual tournament | Dual tournament round 2 | 3       | 1                |
		| Dual tournament | Dual tournament round 3 | 3       | 3                |
	Then created rounds in tournament should be valid with values:
		| Round type      | Round name              | Best of | Advancing amount |
		| Dual tournament | Dual tournament round 1 | 3       | 2                |
		| Dual tournament | Dual tournament round 2 | 3       | 2                |
		| Dual tournament | Dual tournament round 3 | 3       | 2                |

Scenario: Cannot create dual tournament round without name
	When a tournament creates rounds
		| Round type      | Round name | Best of |
		| Dual tournament |            | 3       |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create dual tournament round with even best ofs
	When a tournament creates rounds
		| Round type      | Round name              | Best of |
		| Dual tournament | Dual tournament round 1 | 0       |
		| Dual tournament | Dual tournament round 2 | 2       |
		| Dual tournament | Dual tournament round 3 | 4       |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create dual tournament round with best ofs less than zero
	When a tournament creates rounds
		| Round type       | Round name              | Best of |
		|  Dual tournament | Dual tournament round 1 | -1      |
		|  Dual tournament | Dual tournament round 2 | -2      |
		|  Dual tournament | Dual tournament round 3 | -3      |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid
