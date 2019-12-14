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


#Scenario: Can return winning player when match is finished


#Scenario: Can return losing player when match is finished


#Scenario: Returns null when getting winning player before match is finished


#Scenario: Returns null when getting losing player before match is finished


