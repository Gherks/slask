Feature: RoundInteraction
	Does a bunch of tests on winning players advancements between different types of rounds

@BracketRoundInteractionTag
Scenario: Can fetch all winning players in a bracket round that contains several bracket groups
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Advancing per group count | Players per group count |
			| Bracket    | 3       | 1                         | 4                       |
			| Bracket    | 3       | 1                         | 2                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	Then fetched advancing players in round 0 should be exactly "First, Eighth"

@BracketRoundInteractionTag
Scenario: Cannot fetch winning players from bracket group in a bracket round that has not played out
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Players per group count |
			| Bracket    | 3       | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	Then fetched advancing players in round 0 should yield null

@BracketRoundInteractionTag
Scenario: A bracket round with a predecessor bracket round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type | Best of | Advancing per group count | Players per group count |
			| Bracket    | 3       | 1                         | 4                       |
			| Bracket    | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth, Eleventh, Twelfth, Thirteenth, Fourteenth, Fifteenth, Sixteenth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
		| 0                | 0           | 2           |
		| 0                | 0           | 3           |
	Then participating players in group 4 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Eighth        |
		| 1           | Eleventh      | Fifteenth     |

@BracketRoundInteractionTag
Scenario: A bracket round with a predecessor dual tournament round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Players per group count |
			| Dual tournament | 3       | 4                       |
			| Bracket         | 3       | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
	Then participating players in group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First        | Fourth         |
		| 1           | Fifth        | Eighth         |

@BracketRoundInteractionTag
Scenario: A bracket round with a predecessor round robin round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type  | Best of | Advancing per group count | Players per group count |
			| Round robin | 3       | 2                         | 5                       |
			| Bracket     | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
	Then participating players in group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fifth         | First         |
		| 1           | Eighth        | Ninth         |

################################################################################################################

@DualTournamentRoundInteractionTag
Scenario: Can fetch all winning players in a dual tournament round that contains several dual tournament groups
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Advancing per group count | Players per group count |
			| Dual tournament | 3       | 2                         | 4                       |
			| Bracket         | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	Then fetched advancing players in round 0 should be exactly "Fourth, First, Eighth, Fifth"

@DualTournamentRoundInteractionTag
Scenario: Cannot fetch winning players from dual tournament group in a dual tournament round that has not played out
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Players per group count |
			| Dual tournament | 3       | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	Then fetched advancing players in round 0 should yield null

@DualTournamentRoundInteractionTag
Scenario: A dual tournament round with a predecessor bracket round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Advancing per group count | Players per group count |
			| Bracket         | 3       | 1                         | 4                       |
			| Dual tournament | 3       | 2                         | 4                       |
			| Bracket         | 3       | 1                         | 2                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth, Eleventh, Twelfth, Thirteenth, Fourteenth, Fifteenth, Sixteenth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
		| 0                | 0           | 2           |
		| 0                | 0           | 3           |
	Then participating players in group 4 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Eighth        |
		| 1           | Eleventh      | Fifteenth     |

@DualTournamentRoundInteractionTag
Scenario: A dual tournament round with a predecessor dual tournament round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Advancing per group count | Players per group count |
			| Dual tournament | 3       | 2                         | 4                       |
			| Dual tournament | 3       | 2                         | 4                       |
			| Bracket         | 3       | 1                         | 2                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
	Then participating players in group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Fourth        |
		| 1           | Fifth         | Eighth        |

@DualTournamentRoundInteractionTag
Scenario: A dual tournament round with a predecessor round robin round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Advancing per group count | Players per group count |
			| Round robin     | 3       | 2                         | 5                       |
			| Dual tournament | 3       | 1                         | 4                       |
			| Bracket         | 3       | 1                         | 2                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
	Then participating players in group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fifth         | First         |
		| 1           | Eighth        | Ninth         |

################################################################################################################

@RoundRobinRoundInteractionTag
Scenario: Can fetch all winning players in a round robin round that contains several round robin groups
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type  | Best of | Advancing per group count | Players per group count |
			| Round robin | 3       | 2                         | 4                       |
			| Bracket     | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	Then fetched advancing players in round 0 should be exactly "Fourth, First, Eighth, Fifth"

@RoundRobinRoundInteractionTag
Scenario: Cannot fetch winning players from round robin group in a round robin round that has not played out
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type  | Best of | Advancing per group count | Players per group count |
			| Round robin | 3       | 1                         | 2                       |
			| Round robin | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	Then fetched advancing players in round 0 should yield null

@RoundRobinRoundInteractionTag
Scenario: A round robin round with a predecessor bracket round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type  | Best of | Advancing per group count | Players per group count |
			| Bracket     | 3       | 1                         | 4                       |
			| Round robin | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth, Eleventh, Twelfth, Thirteenth, Fourteenth, Fifteenth, Sixteenth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
		| 0                | 0           | 2           |
		| 0                | 0           | 3           |
	Then participating players in group 4 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Eleventh      |
		| 1           | Eighth        | Fifteenth     |
		| 2           | First         | Fifteenth     |
		| 3           | Eleventh      | Eighth        |
		| 4           | First         | Eighth        |
		| 5           | Fifteenth     | Eleventh      |

@RoundRobinRoundInteractionTag
Scenario: A round robin round with a predecessor dual tournament round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type      | Best of | Advancing per group count | Players per group count |
			| Dual tournament | 3       | 2                         | 4                       |
			| Round robin     | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
	Then participating players in group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | First         | Fifth         |
		| 1           | Fourth        | Eighth        |
		| 2           | First         | Eighth        |
		| 3           | Fifth         | Fourth        |
		| 4           | First         | Fourth        |
		| 5           | Eighth        | Fifth         |

@RoundRobinRoundInteractionTag
Scenario: A round robin round with a predecessor round robin round is set up using only winners of that predecessor round
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type  | Best of | Advancing per group count | Players per group count |
			| Round robin | 3       | 2                         | 5                       |
			| Round robin | 3       | 1                         | 4                       |
	When players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth, Ninth, Tenth" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
		| 0                | 0           | 1           |
	Then participating players in group 2 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Fifth         | Eighth        |
		| 1           | First         | Ninth         |
		| 2           | Fifth         | Ninth         |
		| 3           | Eighth        | First         |
		| 4           | Fifth         | First         |
		| 5           | Ninth         | Eighth        |
