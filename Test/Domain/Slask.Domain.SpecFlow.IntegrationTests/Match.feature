Feature: Match
	Does a bunch of tests on Matches

@MatchTag
Scenario: Can reconfigure best of in match before it has started but tournament has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When best of in match 0 in group 1 is set to 5
	Then best of in match 0 in group 1 should be 5

Scenario: Cannot reconfigure best of in match when it has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When best of in match 0 in group 0 is set to 5
	Then best of in match 0 in group 0 should be 3

Scenario: Match returns NotBegun state before match is played
	When a round robin tournament with users and players has been created
	Then match 0 in group 0 should be in state "NotBegun"

Scenario: Match returns Ongoing state when it has started but not finished
	Given a round robin tournament with users and players has been created
	When score is added to players in given matches in groups
		| Group index | Match index | Scoring player | Added score |
		| 0           | 0           | First          | 1           |
	Then match 0 in group 0 should be in state "Ongoing"

Scenario: Match returns Finished state when match is played out
	Given a round robin tournament with users and players has been created
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then match 0 in group 0 should be in state "Finished"

Scenario: Can return winning and losing players when match is finished
	Given a round robin tournament with users and players has been created
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then winning player can be fetched from match 0 in group 0
		And losing player can be fetched from match 0 in group 0

Scenario: Returns null when getting winning and losing players before match is finished
	Given a round robin tournament with users and players has been created
	When score is added to players in given matches in groups
		| Group index | Match index | Scoring player | Added score |
		| 0           | 0           | First          | 1           |
	Then winning player cannot be fetched from match 0 in group 0
		And losing player cannot be fetched from match 0 in group 0
