Feature: Match
	Does a bunch of tests on Matches

@MatchTag
Scenario: Match returns NotBegun state before match is played
	When a round robin tournament with users and players has been created
	Then match 0 in created group 0 should be in state "NotBegun"

Scenario: Match returns IsPlaying state when it has started but not finished
	Given a round robin tournament with users and players has been created
	When score is added to players in given matches in created groups
		| Created group index | Match index | Scoring player | Added score |
		| 0                   | 0           | First          | 1           |
	Then match 0 in created group 0 should be in state "IsPlaying"

Scenario: Match returns IsFinished state when match is played out
	Given a round robin tournament with users and players has been created
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then match 0 in created group 0 should be in state "IsFinished"

Scenario: Can return winning and losing players when match is finished
	Given a round robin tournament with users and players has been created
		And groups within created tournament is played out and betted on
			| Created tournament index | Round index | Group index |
			| 0                        | 0           | 0           |
	Then winning player can be fetched from match 0 in created group 0
		And losing player can be fetched from match 0 in created group 0

Scenario: Returns null when getting winning and losing players before match is finished
	Given a round robin tournament with users and players has been created
	When score is added to players in given matches in created groups
		| Created group index | Match index | Scoring player | Added score |
		| 0                   | 0           | First          | 1           |
	Then winning player cannot be fetched from match 0 in created group 0
		And losing player cannot be fetched from match 0 in created group 0
