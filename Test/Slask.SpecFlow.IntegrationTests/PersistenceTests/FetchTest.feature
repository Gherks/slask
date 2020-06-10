Feature: FetchTest
	Makes sure a tournament is still intact after being played to completion, stored in database, and fetched from database. 

@FetchTestTag
Scenario: Can fetch completed tournament in correct way
	Given a tournament named "Homestory Cup XX" has been created with users "Stålberto, Bönis, Guggelito, Kimmieboi" added to it
		And tournament 0 adds rounds
			| Round type      | Advancing per group count | Players per group count |
			| Round robin     | 4                         | 5                       |
			| Dual tournament | 2                         | 4                       |
			| Bracket         | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain, TY, Cure" is registered to round 0
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 0           | 0           | 0           | Maru        |
			| Stålberto   | 0           | 0           | 1           | Bomber      |
			| Stålberto   | 0           | 0           | 2           | Rain        |
			| Stålberto   | 0           | 0           | 3           | Taeja       |
			| Stålberto   | 0           | 0           | 4           | Taeja       |
			| Stålberto   | 0           | 0           | 5           | Rain        |
			| Stålberto   | 0           | 0           | 6           | Stork       |
			| Stålberto   | 0           | 0           | 7           | Bomber      |
			| Stålberto   | 0           | 0           | 8           | Maru        |
			| Stålberto   | 0           | 0           | 9           | Taeja       |
			| Stålberto   | 0           | 1           | 0           | FanTaSy     |
			| Stålberto   | 0           | 1           | 1           | Stephano    |
			| Stålberto   | 0           | 1           | 2           | TY          |
			| Stålberto   | 0           | 1           | 3           | FanTaSy     |
			| Stålberto   | 0           | 1           | 4           | Cure        |
			| Stålberto   | 0           | 1           | 5           | TY          |
			| Stålberto   | 0           | 1           | 6           | Thorzain    |
			| Stålberto   | 0           | 1           | 7           | Cure        |
			| Stålberto   | 0           | 1           | 8           | Stephano    |
			| Stålberto   | 0           | 1           | 9           | Thorzain    |
			| Bönis       | 0           | 0           | 0           | Rain        |
			| Bönis       | 0           | 0           | 1           | Bomber      |
			| Bönis       | 0           | 0           | 2           | Bomber      |
			| Bönis       | 0           | 0           | 3           | Taeja       |
			| Bönis       | 0           | 0           | 4           | Taeja       |
			| Bönis       | 0           | 0           | 5           | Stork       |
			| Bönis       | 0           | 0           | 6           | Stork       |
			| Bönis       | 0           | 0           | 7           | Maru        |
			| Bönis       | 0           | 0           | 8           | Maru        |
			| Bönis       | 0           | 0           | 9           | Rain        |
			| Bönis       | 0           | 1           | 0           | TY          |
			| Bönis       | 0           | 1           | 1           | Cure        |
			| Bönis       | 0           | 1           | 2           | Cure        |
			| Bönis       | 0           | 1           | 3           | FanTaSy     |
			| Bönis       | 0           | 1           | 4           | Cure        |
			| Bönis       | 0           | 1           | 5           | TY          |
			| Bönis       | 0           | 1           | 6           | Thorzain    |
			| Bönis       | 0           | 1           | 7           | FanTaSy     |
			| Bönis       | 0           | 1           | 8           | FanTaSy     |
			| Bönis       | 0           | 1           | 9           | TY          |
			| Guggelito   | 0           | 0           | 0           | Maru        |
			| Guggelito   | 0           | 0           | 1           | Bomber      |
			| Guggelito   | 0           | 0           | 2           | Rain        |
			| Guggelito   | 0           | 0           | 3           | Taeja       |
			| Guggelito   | 0           | 0           | 4           | Bomber      |
			| Guggelito   | 0           | 0           | 5           | Stork       |
			| Guggelito   | 0           | 0           | 6           | Taeja       |
			| Guggelito   | 0           | 0           | 7           | Maru        |
			| Guggelito   | 0           | 0           | 8           | Stork       |
			| Guggelito   | 0           | 0           | 9           | Rain        |
			| Guggelito   | 0           | 1           | 0           | FanTaSy     |
			| Guggelito   | 0           | 1           | 1           | Stephano    |
			| Guggelito   | 0           | 1           | 2           | TY          |
			| Guggelito   | 0           | 1           | 3           | Thorzain    |
			| Guggelito   | 0           | 1           | 4           | Thorzain    |
			| Guggelito   | 0           | 1           | 5           | TY          |
			| Guggelito   | 0           | 1           | 6           | Stephano    |
			| Guggelito   | 0           | 1           | 7           | FanTaSy     |
			| Guggelito   | 0           | 1           | 8           | FanTaSy     |
			| Guggelito   | 0           | 1           | 9           | TY          |
			| Kimmieboi   | 0           | 0           | 0           | Maru        |
			| Kimmieboi   | 0           | 0           | 1           | Bomber      |
			| Kimmieboi   | 0           | 0           | 2           | Rain        |
			| Kimmieboi   | 0           | 0           | 3           | Maru        |
			| Kimmieboi   | 0           | 0           | 4           | Taeja       |
			| Kimmieboi   | 0           | 0           | 5           | Rain        |
			| Kimmieboi   | 0           | 0           | 6           | Stork       |
			| Kimmieboi   | 0           | 0           | 7           | Bomber      |
			| Kimmieboi   | 0           | 0           | 8           | Stork       |
			| Kimmieboi   | 0           | 0           | 9           | Rain        |
			| Kimmieboi   | 0           | 1           | 0           | FanTaSy     |
			| Kimmieboi   | 0           | 1           | 1           | Stephano    |
			| Kimmieboi   | 0           | 1           | 2           | TY          |
			| Kimmieboi   | 0           | 1           | 3           | FanTaSy     |
			| Kimmieboi   | 0           | 1           | 4           | Cure        |
			| Kimmieboi   | 0           | 1           | 5           | Stephano    |
			| Kimmieboi   | 0           | 1           | 6           | Stephano    |
			| Kimmieboi   | 0           | 1           | 7           | FanTaSy     |
			| Kimmieboi   | 0           | 1           | 8           | FanTaSy     |
			| Kimmieboi   | 0           | 1           | 9           | TY          |
		And groups within tournament is played out
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 13     |
			| Stålberto   | 11     |
			| Bönis       | 10     |
			| Guggelito   | 7      |

		# Bet on first set of matches in first dual tournament group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 1           | 2           | 0           | Bomber      |
			| Stålberto   | 1           | 2           | 1           | Rain        |
			| Bönis       | 1           | 2           | 0           | Maru        |
			| Bönis       | 1           | 2           | 1           | Stork       |
			| Guggelito   | 1           | 2           | 0           | Bomber      |
			| Guggelito   | 1           | 2           | 1           | Rain        |
			| Kimmieboi   | 1           | 2           | 0           | Maru        |
			| Kimmieboi   | 1           | 2           | 1           | Stork       |
		# Play first set of matches in first dual tournament group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 2           | 0           | Bomber         | 2           |
			| 2           | 1           | Rain           | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Stålberto   | 13     |
			| Kimmieboi   | 13     |
			| Bönis       | 10     |
			| Guggelito   | 9      |

		# Bet on winners and losers matches in first dual tournament group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 1           | 2           | 2           | Rain        |
			| Stålberto   | 1           | 2           | 3           | Stork       |
			| Bönis       | 1           | 2           | 2           | Bomber      |
			| Bönis       | 1           | 2           | 3           | Maru        |
			| Guggelito   | 1           | 2           | 2           | Bomber      |
			| Guggelito   | 1           | 2           | 3           | Stork       |
			| Kimmieboi   | 1           | 2           | 2           | Bomber      |
			| Kimmieboi   | 1           | 2           | 3           | Maru        |
		# Play winners and losers matches in first dual tournament group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 2           | 2           | Bomber         | 2           |
			| 2           | 3           | Maru           | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 15     |
			| Stålberto   | 13     |
			| Bönis       | 12     |
			| Guggelito   | 10     |

		# Bet on decider match in first dual tournament group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 1           | 2           | 4           | Maru        |
			| Bönis       | 1           | 2           | 4           | Rain        |
			| Guggelito   | 1           | 2           | 4           | Maru        |
			| Kimmieboi   | 1           | 2           | 4           | Rain        |
		# Play decider match in first dual tournament group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 2           | 4           | Maru           | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 15     |
			| Stålberto   | 14     |
			| Bönis       | 12     |
			| Guggelito   | 11     |

		# Bet on first set of matches in second dual tournament group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 1           | 3           | 0           | Cure        |
			| Stålberto   | 1           | 3           | 1           | Stephano    |
			| Bönis       | 1           | 3           | 0           | FanTaSy     |
			| Bönis       | 1           | 3           | 1           | Thorzain    |
			| Guggelito   | 1           | 3           | 0           | FanTaSy     |
			| Guggelito   | 1           | 3           | 1           | Thorzain    |
			| Kimmieboi   | 1           | 3           | 0           | FanTaSy     |
			| Kimmieboi   | 1           | 3           | 1           | Stephano    |
		# Play first set of matches in second dual tournament group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 3           | 0           | Cure           | 2           |
			| 3           | 1           | Stephano       | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Stålberto   | 16     |
			| Kimmieboi   | 16     |
			| Bönis       | 12     |
			| Guggelito   | 11     |

		# Bet on winners and losers matches in second dual tournament group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 1           | 3           | 2           | Stephano    |
			| Stålberto   | 1           | 3           | 3           | Thorzain    |
			| Bönis       | 1           | 3           | 2           | Cure        |
			| Bönis       | 1           | 3           | 3           | FanTaSy     |
			| Guggelito   | 1           | 3           | 2           | Stephano    |
			| Guggelito   | 1           | 3           | 3           | FanTaSy     |
			| Kimmieboi   | 1           | 3           | 2           | Cure        |
			| Kimmieboi   | 1           | 3           | 3           | Thorzain    |
		# Play winners and losers matches in second dual tournament groups
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 3           | 2           | Cure           | 2           |
			| 3           | 3           | FanTaSy        | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 17     |
			| Stålberto   | 16     |
			| Bönis       | 14     |
			| Guggelito   | 12     |
			
		# Bet on decider match in second dual tournament group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 1           | 3           | 4           | Stephano    |
			| Bönis       | 1           | 3           | 4           | FanTaSy     |
			| Guggelito   | 1           | 3           | 4           | FanTaSy     |
			| Kimmieboi   | 1           | 3           | 4           | Stephano    |
		# Play decider match in second dual tournament group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 3           | 4           | FanTaSy        | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 17     |
			| Stålberto   | 16     |
			| Bönis       | 15     |
			| Guggelito   | 13     |
		
		# Bet on semifinal matches in bracket group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 2           | 4           | 0           | Maru        |
			| Stålberto   | 2           | 4           | 1           | Cure        |
			| Bönis       | 2           | 4           | 0           | Bomber      |
			| Bönis       | 2           | 4           | 1           | Cure        |
			| Guggelito   | 2           | 4           | 0           | Bomber      |
			| Guggelito   | 2           | 4           | 1           | Cure        |
			| Kimmieboi   | 2           | 4           | 0           | Bomber      |
			| Kimmieboi   | 2           | 4           | 1           | FanTaSy     |
		# Play semifinal matches in bracket group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 4           | 0           | Bomber         | 2           |
			| 4           | 1           | Cure           | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 18     |
			| Stålberto   | 17     |
			| Bönis       | 17     |
			| Guggelito   | 15     |
		
		# Bet on final match in bracket group
		And betters places match bets
			| Better name | Round index | Group index | Match index | Player name |
			| Stålberto   | 2           | 4           | 2           | Bomber      |
			| Bönis       | 2           | 4           | 2           | Cure        |
			| Guggelito   | 2           | 4           | 2           | Cure        |
			| Kimmieboi   | 2           | 4           | 2           | Bomber      |
		# Play final match in bracket group
		And score is added to players in given matches in groups
			| Group index | Match index | Scoring player | Added score |
			| 4           | 2           | Bomber         | 2           |
		And better standings in tournament 0 from first to last looks like this
			| Better name | Points |
			| Kimmieboi   | 19     |
			| Stålberto   | 18     |
			| Bönis       | 17     |
			| Guggelito   | 15     |
