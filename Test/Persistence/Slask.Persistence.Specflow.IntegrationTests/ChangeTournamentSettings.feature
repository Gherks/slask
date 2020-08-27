Feature: ChangeTournamentSettings
	Makes sure tournament entities can be changed when they are supposed to, and not change when they are not supposed to.

@ChangeTournamentSettingsTag
Scenario: Can only remove a round before tournament has started
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type      | Round name   | Advancing per group count | Players per group count |
			| Round robin     | Round Uno    | 4                         | 5                       |
			| Round robin     | Round Dos    | 4                         | 4                       |
			| Dual tournament | Round Tres   | 2                         | 4                       |
			| Bracket         | Round Cuatro | 1                         | 4                       |
		And round named "Round Dos" is removed from tournament named "Homestory Cup XX"
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure" is registered to tournament named "Homestory Cup XX"
		And groups within tournament named "Homestory Cup XX" is played out
			| Round index | Group index |
			| 0           | 0           |
	When round named "Round Uno" is removed from tournament named "Homestory Cup XX"
		And round named "Round Cuatro" is removed from tournament named "Homestory Cup XX"
	Then round layout in tournament named "Homestory Cup XX" is exactly as follows:
		| Round type      | Round name   | Advancing per group count | Players per group count |
		| Round robin     | Round Uno    | 4                         | 5                       |
		| Dual tournament | Round Tres   | 2                         | 4                       |
		| Bracket         | Round Cuatro | 1                         | 4                       |

Scenario: Can only change advancing players per group count before tournament has started
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type  | Round name | Advancing per group count | Players per group count |
			| Round robin | Round Uno  | 4                         | 6                       |
			| Bracket     | Round Dos  | 1                         | 8                       |
		And round named "Round Uno" changes advancing players per group count to "2" in tournament named "Homestory Cup XX"
		And round named "Round Dos" changes players per group count to "4" in tournament named "Homestory Cup XX"
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure, Stats, Rogue" is registered to tournament named "Homestory Cup XX"
		And groups within tournament named "Homestory Cup XX" is played out
			| Round index | Group index |
			| 0           | 0           |
		And round named "Round Uno" changes advancing players per group count to "6" in tournament named "Homestory Cup XX"
		And round named "Round Dos" changes players per group count to "8" in tournament named "Homestory Cup XX"
	Then round layout in tournament named "Homestory Cup XX" is exactly as follows:
		| Round type  | Round name | Advancing per group count | Players per group count |
		| Round robin | Round Uno  | 2                         | 6                       |
		| Bracket     | Round Dos  | 1                         | 4                       |

Scenario: Can only change players per group count before tournament has started
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type  | Round name | Advancing per group count | Players per group count |
			| Bracket     | Round Uno  | 1                         | 6                       |
			| Round robin | Round Dos  | 1                         | 2                       |
		And round named "Round Uno" changes players per group count to "4" in tournament named "Homestory Cup XX"
		And round named "Round Dos" changes players per group count to "3" in tournament named "Homestory Cup XX"
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure, Stats, Rogue" is registered to tournament named "Homestory Cup XX"
		And groups within tournament named "Homestory Cup XX" is played out
			| Round index | Group index |
			| 0           | 0           |
		And round named "Round Uno" changes players per group count to "6" in tournament named "Homestory Cup XX"
		And round named "Round Dos" changes players per group count to "2" in tournament named "Homestory Cup XX"
	Then round layout in tournament named "Homestory Cup XX" is exactly as follows:
		| Round type  | Round name | Advancing per group count | Players per group count |
		| Bracket     | Round Uno  | 1                         | 4                       |
		| Round robin | Round Dos  | 1                         | 3                       |

Scenario: Can change best of settings in a match that has not started
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type  | Round name | Advancing per group count | Players per group count |
			| Bracket     | Round Uno  | 1                         | 6                       |
			| Round robin | Round Dos  | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure, Stats, Rogue" is registered to tournament named "Homestory Cup XX"
		And groups within tournament named "Homestory Cup XX" is played out
			| Round index | Group index |
			| 0           | 0           |
	When matches in tournament named "Homestory Cup XX" changes best of setting
		| Round index | Group index | Match index | Best of |
		| 0           | 0           | 0           | 5       |
		| 0           | 1           | 0           | 5       |
	Then best of for matches in tournament named "Homestory Cup XX" should be set to
		| Round index | Group index | Match index | Best of |
		| 0           | 0           | 0           | 3       |
		| 0           | 1           | 0           | 5       |
		| 0           | 1           | 1           | 3       |

Scenario: Can change start time on match in tournament before it has started
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type  | Round name | Advancing per group count | Players per group count |
			| Round robin | Round Uno  | 2                         | 12                      |
			| Bracket     | Round Dos  | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure, Stats, Rogue" is registered to tournament named "Homestory Cup XX"
	When move start time three hours forward for matches in tournament named "Homestory Cup XX"
		| Round index | Group index | Match index |
		| 0           | 0           | 0           |
	Then start time has been moved forward three hours for matches in tournament named "Homestory Cup XX"
		| Round index | Group index | Match index |
		| 0           | 0           | 0           |

Scenario: Two different matches can switch player references with each other
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type  | Advancing per group count | Players per group count |
			| Bracket     | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament named "Homestory Cup XX"
		And player layout for matches in tournament named "Homestory Cup XX" looks like this
			| Round index | Group index | Match index | Player index | Player name |
			| 0           | 0           | 0           | 0            | Maru        |
			| 0           | 0           | 0           | 1            | Stork       |
			| 0           | 0           | 1           | 0            | Taeja       |
			| 0           | 0           | 1           | 1            | Rain        |
	When matches in tournament named "Homestory Cup XX" switches player references
		| Round index | Group index 1 | Match index 1 | Player name 1 | Group index 2 | Match index 2 | Player name 2 |
		| 0           | 0             | 0             | Maru          | 0             | 1             | Rain          |
	Then player layout for matches in tournament named "Homestory Cup XX" looks like this
		| Round index | Group index | Match index | Player index | Player name |
		| 0           | 0           | 0           | 0            | Rain        |
		| 0           | 0           | 0           | 1            | Stork       |
		| 0           | 0           | 1           | 0            | Taeja       |
		| 0           | 0           | 1           | 1            | Maru        |
