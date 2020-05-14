Feature: Tournament
	Does a bunch of tests on a tournament as a whole

@TournamentTag
Scenario: Cannot register new players references when tournament has begun
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | 
			| Bracket    | Bracket round | 3       | 
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0 
		And created groups within created tournament is played out and betted on 
			| Tournament index | Round index | Group index | 
			| 0                | 0           | 0           | 
		And players "FailedRegistration" is registered to round 0 
	Then created tournament 0 should contain exactly these player references with names: "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth"

Scenario: Cannot exclude players references when tournament has begun
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | 
			| Bracket    | Bracket round | 3       | 
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0 
		And created groups within created tournament is played out and betted on 
			| Tournament index | Round index | Group index | 
			| 0                | 0           | 0           | 
		And players "First" is excluded from round 0 
	Then created tournament 0 should contain exactly these player references with names: "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth"

Scenario: When first round is removed the existing player references are transfered to the new first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name          | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round 1 | 3       | 2                         | 4                       |
			| Round robin | Round robin round 2 | 3       | 2                         | 4                       |
			| Round robin | Round robin round 3 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When round 0 is removed from tournament 0
	Then participating players in round 0 should be exactly "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain"
		And tournament 0 contains 2 rounds

Scenario: PlayState is set to NotBegun before any round has started
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then play state of tournament 0 is set to "NotBegun"
	
Scenario: PlayState is set to IsPlaying when at least one round has started but not all
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
			| Bracket    | Bracket round 2 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When created groups within created tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then play state of tournament 0 is set to "IsPlaying"

Scenario: PlayState set to IsFinished when all rounds has finished
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type | Round name      | Best of | Advancing per group count | Players per group count |
			| Bracket    | Bracket round 1 | 3       | 1                         | 4                       |
			| Bracket    | Bracket round 2 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When created groups within created tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
			| 0                | 1           | 0           |
	Then play state of tournament 0 is set to "IsFinished"