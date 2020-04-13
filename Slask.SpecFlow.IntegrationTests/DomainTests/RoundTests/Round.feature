﻿Feature: Round
	Does a bunch of general tests on rounds

@RoundTag
Scenario: Cannot register new players references when tournament has begun
	Given a tournament named "GSL 2019" with users "Stålberto" added to it
		And players "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth" is registered to round 0 
		And created tournament 0 adds rounds
			| Round type | Round name    | Best of | 
			| Bracket    | Bracket round | 3       | 
		And created round 0 adds 2 groups
		And created groups within created tournament is played out and betted on 
			| Created tournament index | Round index | Group index | 
			| 0                        | 0           | 0           | 
		And players "FailedRegistration" is registered to round 0 
	Then registered players in tournament 0 should be exactly "First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eighth"
