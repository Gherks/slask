Feature: Round
	Does a bunch of tests on Rounds

@RoundTag
Scenario: Can fetch previous round from round with round predecessor
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
			| Bracket         | Bracket round         | 3       |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type      | Round name            | Best of |
		| Dual tournament | Dual tournament round | 3       |

Scenario: Cannot fetch previous round with first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of |
			| Bracket    | Bracket round | 3       |
	When created round 0 fetches previous round
	Then fetched round 0 in tournament should be invalid

#Scenario: Can fetch winning players from bracket round with several bracket groups
#
#Scenario: Can fetch winning players from dual tournament round with several dual tournament groups
#
#Scenario: Can fetch winning players from round robin round with several round robin groups
#Scenario: Solve round robin ties
#// ADVANCING PLAYERS MUST ALWAYS BE EQUAL OR LESS THAN NUMBER OF GROUPS IN ROUND
#// ADVANCING PLAYERS MUST ALWAYS BE EQUAL OR LESS THAN NUMBER OF GROUPS IN ROUND