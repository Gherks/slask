Feature: RoundRobinRound
	Does a bunch of tests on Round robin rounds

@RoundRobinRoundTag
Scenario: Can create round robin round
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |
	Then created rounds in tournament should be valid with values:
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |

Scenario: Cannot create round robin round without name
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name | Best of | Advancing amount |
		| Round robin |            | 3       | 1                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create round robin round with zero advancers
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name        | Best of | Advancing amount |
		| Round robin     | Round robin round | 3       | 0                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create round robin round with less than zero advancers
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name          | Best of | Advancing amount |
		| Round robin     | Round robin round 1 | 3       | -1               |
		| Round robin     | Round robin round 2 | 3       | -2               |
		| Round robin     | Round robin round 3 | 3       | -3               |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with even best ofs
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type      | Round name          | Best of | Advancing amount |
		| Round robin     | Round robin round 1 | 0       | 1                |
		| Round robin     | Round robin round 2 | 2       | 1                |
		| Round robin     | Round robin round 3 | 4       | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with best ofs less than zero
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type  | Round name          | Best of | Advancing amount |
		| Round robin | Round robin round 1 | -1      | 1                |
		| Round robin | Round robin round 2 | -2      | 1                |
		| Round robin | Round robin round 3 | -3      | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid
