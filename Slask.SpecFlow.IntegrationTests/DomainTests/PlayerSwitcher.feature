Feature: PlayerSwitcher
	Does a bunch of tests on PlayerSwitcher

@PlayerSwitcherTag
Scenario: Can switch player with other player in same bracket group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Can switch player with other player in same dual tournament group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in same round robin group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Round robin | Round robin round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Can switch player with other player that resides in different groups within same bracket round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Can switch player with other player that resides in different groups within same dual tournament round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player that resides in different groups within same round robin round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Round robin | Round robin round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player that resides in a bracket group with player in different round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player that resides in a dual tournament group with player in different round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player that resides in a round robin group with player in different round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Round robin | Round robin round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in same bracket group that has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in same dual tournament group that has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in same round robin group that has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name        | Best of |
			| Round robin     | Round robin round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in different group within same bracket round that has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in different group within same dual tournament round that has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player with other player in different group within same round robin round that has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name        | Best of |
			| Round robin     | Round robin round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player in bracket group that has started with other player in different round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of |
			| Bracket     | Bracket round     | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player in dual tournament group that has started with other player in different round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0

Scenario: Cannot switch player in round robin group that has started with other player in different round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type      | Round name        | Best of |
			| Round robin     | Round robin round | 3       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
