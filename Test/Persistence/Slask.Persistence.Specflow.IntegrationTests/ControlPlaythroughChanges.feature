Feature: ControlPlaythroughChanges
	Makes sure tournament entities can be changed when they are supposed to, and not change when they are not supposed to.

@ControlPlaythroughChangesTag
Scenario: As tournament is played, only the appropriate entities can be changed
	Given a tournament named "Homestory Cup XX" has been created with users "" added to it
		And tournament named "Homestory Cup XX" adds rounds
			| Round type      | Advancing per group count | Players per group count |
			| Round robin     | 4                         | 5                       |
			| Dual tournament | 2                         | 4                       |
			| Bracket         | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure" is registered to tournament named "Homestory Cup XX"
		# ROUND ROBIN ROUND BEGINS
		And groups within tournament named "Homestory Cup XX" is played out
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
		And groups within tournament named "Homestory Cup XX" is played out
			| Tournament index | Round index | Group index |
			| 0                | 0           | 1           |

		# DUAL TOURNAMENT ROUND BEGINS
		# Play first set of matches in first dual tournament group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 1           | 0           | 0           | Bomber         | 2           |
			| 1           | 0           | 1           | Rain           | 2           |
		# Play winners and losers matches in first dual tournament group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 1           | 0           | 2           | Bomber         | 2           |
			| 1           | 0           | 3           | Maru           | 2           |
		# Play decider match in first dual tournament group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 1           | 0           | 4           | Maru           | 2           |
		# Play first set of matches in second dual tournament group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 1           | 1           | 0           | Cure           | 2           |
			| 1           | 1           | 1           | Stephano       | 2           |
		# Play winners and losers matches in second dual tournament groups
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 1           | 1           | 2           | Cure           | 2           |
			| 1           | 1           | 3           | FanTaSy        | 2           |
		# Play decider match in second dual tournament group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 1           | 1           | 4           | FanTaSy        | 2           |
		
		# BRACKET ROUND BEGINS
		# Play semifinal matches in bracket group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 2           | 0           | 0           | Bomber         | 2           |
			| 2           | 0           | 1           | Cure           | 2           |
		# Play final match in bracket group
		And score is added to players in given matches within groups in tournament named "Homestory Cup XX"
			| Round index | Group index | Match index | Scoring player | Added score |
			| 2           | 0           | 2           | Bomber         | 2           |
	Then tournament named "Homestory Cup XX" should contain rounds
		| Round type      |
		| Round robin     |
		| Dual tournament |
		| Bracket         |
		And groups within round 0 in tournament named "Homestory Cup XX" is of type "Round robin"
		And groups within round 1 in tournament named "Homestory Cup XX" is of type "Dual tournament"
		And groups within round 2 in tournament named "Homestory Cup XX" is of type "Bracket"