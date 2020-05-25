Feature: Tournament
	Does a bunch of tests on a tournament as a whole

@TournamentTag
Scenario: Cannot register new players references when tournament has begun
	Given a tournament named "GSL 2019" has been created with users "Stålberto" added to it
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0 
		And groups within tournament is played out and betted on 
			| Tournament index | Round index | Group index | 
			| 0                | 0           | 0           | 
	When players "FailedRegistration" is registered to round 0 
	Then tournament 0 should contain exactly these player references with names: "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth"

Scenario: Cannot add new rounds when tournament has begun
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
			| Bracket    | 1                         | 2                       |
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0 
		And groups within tournament is played out and betted on 
			| Tournament index | Round index | Group index | 
			| 0                | 0           | 0           | 
	When tournament 0 adds rounds
		| Round type | Advancing per group count | Players per group count |
		| Bracket    | 1                         | 2                       |
	Then tournament 0 contains 2 rounds

Scenario: Cannot remove rounds when tournament has begun
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 2                         | 4                       |
			| Bracket    | 2                         | 4                       |
			| Bracket    | 1                         | 2                       |
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0 
		And groups within tournament is played out and betted on 
			| Tournament index | Round index | Group index | 
			| 0                | 0           | 0           | 
	When tournament 0 removes round 0
	Then tournament 0 contains 3 rounds

Scenario: Existing player references are moved to next round if first round is successfully removed
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 2                         | 4                       |
			| Bracket    | 2                         | 4                       |
			| Bracket    | 1                         | 2                       |
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And tournament 0 contains 3 rounds
	When tournament 0 removes round 0
	Then players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0
		And tournament 0 contains 2 rounds

Scenario: Can exclude players within first round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
	When players "Maru" is excluded from round 0
	Then participating players in round 0 should be exactly "Stork, Taeja, Rain"

Scenario: Cannot exclude players within rounds other than first round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 2                       |
			| Bracket    | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	When players "Maru" is excluded from round 1
	Then participating players in round 1 should be exactly "Maru, Rain"

Scenario: Cannot exclude players references when tournament has begun
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 2                       |
			| Bracket    | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain" is registered to round 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When players "Maru" is excluded from round 0
	Then participating players in round 0 should be exactly "Maru, Stork, Taeja, Rain"

Scenario: When first round is removed the existing player references are transfered to the new first round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Round robin | 2                         | 4                       |
			| Round robin | 2                         | 4                       |
			| Round robin | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When tournament 0 removes round 0
	Then participating players in round 0 should be exactly "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain"
		And tournament 0 contains 2 rounds

Scenario: PlayState is set to NotBegun before any round has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then play state of tournament 0 is set to "NotBegun"
	
Scenario: PlayState is set to Ongoing when at least one round has started but not all
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
			| Bracket    | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When groups within tournament is played out and betted on
		| Tournament index | Round index | Group index |
		| 0                | 0           | 0           |
	Then play state of tournament 0 is set to "Ongoing"

Scenario: PlayState set to Finished when all rounds has finished
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Round type | Advancing per group count | Players per group count |
			| Bracket    | 1                         | 4                       |
			| Bracket    | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	When groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
			| 0                | 1           | 0           |
	Then play state of tournament 0 is set to "Finished"
