Feature: BracketRound
	Does a bunch of tests on Bracket rounds

@BracketRoundTag
Scenario: Can create bracket round
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type | Round name    | Best of |
		| Bracket    | Bracket round | 3       |
	Then created rounds in tournament should be valid with values:
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |

Scenario: Advancing amount in bracket rounds cannot be anything other than two
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type | Round name      | Best of | Advancing amount |
		| Bracket    | Bracket round 1 | 3       | 0                |
		| Bracket    | Bracket round 2 | 3       | 2                |
		| Bracket    | Bracket round 3 | 3       | 3                |
	Then created rounds in tournament should be valid with values:
		| Round type | Round name      | Best of | Advancing amount |
		| Bracket    | Bracket round 1 | 3       | 1                |
		| Bracket    | Bracket round 2 | 3       | 1                |
		| Bracket    | Bracket round 3 | 3       | 1                |

Scenario: Cannot create bracket round without name
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type | Round name | Best of |
		| Bracket    |            | 3       |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create bracket round with even best ofs
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type | Round name      | Best of |
		| Bracket    | Bracket round 1 | 0       |
		| Bracket    | Bracket round 2 | 2       |
		| Bracket    | Bracket round 3 | 4       |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create bracket round with best ofs less than zero
	Given a tournament named "GSL 2019" has been created
	When created tournament 0 adds rounds
		| Round type | Round name      | Best of |
		| Bracket    | Bracket round 1 | -1      |
		| Bracket    | Bracket round 2 | -2      |
		| Bracket    | Bracket round 3 | -3      |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid


#CanFetchPreviousRoundFromRoundWithRoundPredecessor
#OnlyWinningPlayersCanAdvanceToNextRound
#ADVANCING PLAYERS MUST ALWAYS BE EQUAL OR LESS THAN NUMBER OF GROUPS IN ROUND