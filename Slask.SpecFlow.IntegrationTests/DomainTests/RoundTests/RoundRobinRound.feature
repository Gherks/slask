Feature: RoundRobinRound
	Does a bunch of tests on Round robin rounds

@RoundRobinRoundTag
Scenario: Can create round robin round
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name        | Best of | Advancing per group count |
		| Round robin | Round robin round | 3       | 1                         |
	Then created rounds in tournament should be valid with values:
		| Round type  | Round name        | Best of | Advancing per group count |
		| Round robin | Round robin round | 3       | 1                         |

Scenario: Cannot create round robin round without name
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name | Best of | Advancing per group count |
		| Round robin |            | 3       | 1                         |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create round robin round with zero advancers
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name        | Best of | Advancing per group count |
		| Round robin | Round robin round | 3       | 0                         |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create round robin round with less than zero advancers
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name          | Best of | Advancing per group count |
		| Round robin | Round robin round 1 | 3       | -1                        |
		| Round robin | Round robin round 2 | 3       | -2                        |
		| Round robin | Round robin round 3 | 3       | -3                        |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with even best ofs
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name          | Best of | Advancing per group count |
		| Round robin | Round robin round 1 | 0       | 1               |
		| Round robin | Round robin round 2 | 2       | 1               |
		| Round robin | Round robin round 3 | 4       | 1               |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with best ofs less than zero
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name          | Best of | Advancing per group count |
		| Round robin | Round robin round 1 | -1      | 1                         |
		| Round robin | Round robin round 2 | -2      | 1                         |
		| Round robin | Round robin round 3 | -3      | 1                         |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Can fetch bracket round as predecessor of a round robin round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
			| Round robin | Round robin round | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type | Round name    | Best of |
		| Bracket    | Bracket round | 3       |

Scenario: Can fetch dual tournament round as predecessor of a round robin round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
			| Round robin     | Round robin round     | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type      | Round name            | Best of |
		| Dual tournament | Dual tournament round | 3       |

Scenario: Can fetch round robin round as predecessor of a round robin round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name          | Best of |
			| Round robin | Round robin round 1 | 3       |
			| Round robin | Round robin round 2 | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type  | Round name          | Best of |
		| Round robin | Round robin round 1 | 3       |

Scenario: Cannot fetch previous round with first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Round robin | Round robin round | 3       |
	When created round 0 fetches previous round
	Then fetched round 0 in tournament should be invalid
