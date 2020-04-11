Feature: DualTournamentRound
	Does a bunch of tests on Dual tournament rounds

@DualTournamentRoundTag
Scenario: Can create dual tournament round
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name            | Best of |
		| Dual tournament | Dual tournament round | 3       |
	Then created rounds in tournament should be valid with values:
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 2                |

Scenario: Advancing amount in dual tournament rounds cannot be anything other than two
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
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
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name | Best of |
		| Dual tournament |            | 3       |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create dual tournament round with even best ofs
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name              | Best of |
		| Dual tournament | Dual tournament round 1 | 0       |
		| Dual tournament | Dual tournament round 2 | 2       |
		| Dual tournament | Dual tournament round 3 | 4       |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create dual tournament round with best ofs less than zero
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name              | Best of |
		| Dual tournament | Dual tournament round 1 | -1      |
		| Dual tournament | Dual tournament round 2 | -2      |
		| Dual tournament | Dual tournament round 3 | -3      |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Can fetch bracket round as predecessor of a dual tournament round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name                 | Best of |
			| Bracket         | Bracket round         | 3       |
			| Dual tournament | Dual tournament round | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type | Round name    | Best of |
		| Bracket    | Bracket round | 3       |

Scenario: Can fetch dual tournament round as predecessor of a dual tournament round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name              | Best of |
			| Dual tournament | Dual tournament round 1 | 3       |
			| Dual tournament | Dual tournament round 2 | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type      | Round name              | Best of |
		| Dual tournament | Dual tournament round 1 | 3       |

Scenario: Can fetch round robin round as predecessor of a dual tournament round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Round robin     | Round robin round     | 3       |
			| Dual tournament | Dual tournament round | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type  | Round name        | Best of |
		| Round robin | Round robin round | 3       |

Scenario: Cannot fetch previous round with first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
	When created round 0 fetches previous round
	Then fetched round 0 in tournament should be invalid
