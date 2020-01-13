Feature: RoundInteraction
	Does a bunch of tests on winning players advancements between different types of rounds

@BracketRoundInteractionTag
Scenario: Can fetch all winning players in a bracket round that contains several bracket groups
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of |
			| Bracket    | Bracket round | 3       |
		And created round 0 adds 2 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And created groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
			| 0                        | 0           | 1           |
	Then fetched advancing players in created round 0 should be exactly "Fourth, Eighth"

@BracketRoundInteractionTag
Scenario: Cannot fetch winning players from bracket group in a bracket round that has not played out
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of |
			| Bracket    | Bracket round | 3       |
		And created round 0 adds 2 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And created groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then fetched advancing players in created round 0 should yield null

@BracketRoundInteractionTag
Scenario: A bracket round with a predecessor bracket round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of |
			| Bracket    | Bracket round 1 | 3       |
			| Bracket    | Bracket round 2 | 3       |
		And created round 0 adds 4 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And players "Ninth, Tenth, Eleventh, Twelfth" is added to created group 2
		And players "Thirteenth, Fourteenth, Fifteenth, Sixteenth" is added to created group 3
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
		| 0                        | 0           | 2           |
		| 0                        | 0           | 3           |
	Then participating players in created group 4 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fourth        | Eighth        |
		| 1           | Twelfth       | Sixteenth     |

@BracketRoundInteractionTag
Scenario: A bracket round with a predecessor dual tournament round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
			| Bracket         | Bracket round         | 3       |
		And created round 0 adds 2 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
	Then participating players in created group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First        | Fourth         |
		| 1           | Fifth        | Eighth         |

@BracketRoundInteractionTag
Scenario: A bracket round with a predecessor round robin round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing amount |
			| Round robin | Round robin round | 3       | 2                |
			| Bracket     | Bracket round     | 3       | 1                |
		And created round 0 adds 2 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth, Fifth" is added to created group 0
		And players "Sixth, Seventh, Eighth, Ninth, Tenth" is added to created group 1
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
	Then participating players in created group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fifth         | Second        |
		| 1           | Tenth         | Seventh       |

#Scenario: Players that has switched match positions is kept when fetching tournament again

################################################################################################################

@DualTournamentRoundInteractionTag
Scenario: Can fetch all winning players in a dual tournament round that contains several dual tournament groups
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And created round 0 adds 2 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And created groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
			| 0                        | 0           | 1           |
	Then fetched advancing players in created round 0 should be exactly "Fourth, First, Eighth, Fifth"

@DualTournamentRoundInteractionTag
Scenario: Cannot fetch winning players from dual tournament group in a dual tournament round that has not played out
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Dual tournament | Dual tournament round | 3       |
		And created round 0 adds 2 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And created groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then fetched advancing players in created round 0 should yield null

@DualTournamentRoundInteractionTag
Scenario: A dual tournament round with a predecessor bracket round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of |
			| Bracket         | Bracket round         | 3       |
			| Dual tournament | Dual tournament round | 3       |
		And created round 0 adds 4 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And players "Ninth, Tenth, Eleventh, Twelfth" is added to created group 2
		And players "Thirteenth, Fourteenth, Fifteenth, Sixteenth" is added to created group 3
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
		| 0                        | 0           | 2           |
		| 0                        | 0           | 3           |
	Then participating players in created group 4 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fourth        | Eighth        |
		| 1           | Twelfth       | Sixteenth     |

@DualTournamentRoundInteractionTag
Scenario: A dual tournament round with a predecessor dual tournament round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name              | Best of |
			| Dual tournament | Dual tournament round 1 | 3       |
			| Dual tournament | Dual tournament round 2 | 3       |
		And created round 0 adds 2 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
	Then participating players in created group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Fourth        |
		| 1           | Fifth         | Eighth        |

@DualTournamentRoundInteractionTag
Scenario: A dual tournament round with a predecessor round robin round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing amount |
			| Round robin     | Round robin round     | 3       | 2                |
			| Dual tournament | Dual tournament round | 3       | 1                |
		And created round 0 adds 2 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth, Fifth" is added to created group 0
		And players "Sixth, Seventh, Eighth, Ninth, Tenth" is added to created group 1
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
	Then participating players in created group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fifth         | Second        |
		| 1           | Tenth         | Seventh       |

################################################################################################################

@RoundRobinRoundInteractionTag
Scenario: Can fetch all winning players in a round robin round that contains several round robin groups
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing amount |
			| Round robin | Round robin round | 3       | 2                |
		And created round 0 adds 2 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And created groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
			| 0                        | 0           | 1           |
	Then fetched advancing players in created round 0 should be exactly "Fourth, First, Eighth, Fifth"

@RoundRobinRoundInteractionTag
Scenario: Cannot fetch winning players from round robin group in a round robin round that has not played out
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing amount |
			| Round robin | Round robin round | 3       | 2                |
		And created round 0 adds 2 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And created groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then fetched advancing players in created round 0 should yield null

@RoundRobinRoundInteractionTag
Scenario: A round robin round with a predecessor bracket round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing amount |
			| Bracket     | Bracket round     | 3       | 1                |
			| Round robin | Round robin round | 3       | 1                |
		And created round 0 adds 4 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
		And players "Ninth, Tenth, Eleventh, Twelfth" is added to created group 2
		And players "Thirteenth, Fourteenth, Fifteenth, Sixteenth" is added to created group 3
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
		| 0                        | 0           | 2           |
		| 0                        | 0           | 3           |
	Then participating players in created group 4 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fourth        | Twelfth       |
		| 1           | Eighth        | Sixteenth     |
		| 2           | Fourth        | Sixteenth     |
		| 3           | Twelfth       | Eighth        |
		| 4           | Fourth        | Eighth        |
		| 5           | Sixteenth     | Twelfth       |

@RoundRobinRoundInteractionTag
Scenario: A round robin round with a predecessor dual tournament round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type      | Round name            | Best of | Advancing amount |
			| Dual tournament | Dual tournament round | 3       | 2                |
			| Round robin     | Round robin round     | 3       | 1                |
		And created round 0 adds 2 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth" is added to created group 0
		And players "Fifth, Sixth, Seventh, Eighth" is added to created group 1
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
	Then participating players in created group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Fifth         |
		| 1           | Fourth        | Eighth        |
		| 2           | First         | Eighth        |
		| 3           | Fifth         | Fourth        |
		| 4           | First         | Fourth        |
		| 5           | Eighth        | Fifth         |

@RoundRobinRoundInteractionTag
Scenario: A round robin round with a predecessor round robin round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type  | Round name          | Best of | Advancing amount |
			| Round robin | Round robin round 1 | 3       | 2                |
			| Round robin | Round robin round 2 | 3       | 1                |
		And created round 0 adds 2 groups
		And created round 1 adds 1 groups
		And players "First, Second, Third, Fourth, Fifth" is added to created group 0
		And players "Sixth, Seventh, Eighth, Ninth, Tenth" is added to created group 1
	When created groups within created tournament is played out and betted on
		| Created tournament index | Round index | Group index |
		| 0                        | 0           | 0           |
		| 0                        | 0           | 1           |
	Then participating players in created group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fifth         | Tenth         |
		| 1           | Second        | Seventh       |
		| 2           | Fifth         | Seventh       |
		| 3           | Tenth         | Second        |
		| 4           | Fifth         | Second        |
		| 5           | Seventh       | Tenth         |
