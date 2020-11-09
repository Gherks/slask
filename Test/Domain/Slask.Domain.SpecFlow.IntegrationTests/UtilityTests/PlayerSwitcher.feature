Feature: PlayerSwitcher
	Does a bunch of tests on PlayerSwitcher

@PlayerSwitcherTag
Scenario: Can switch player with other player in same bracket group
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
	When player "Maru" in group 0 and player "Taeja" in group 0 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Taeja         | Stork         |
		| 1           | Maru          | Rain          |

Scenario: Can switch player with other player in same dual tournament group
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
	When player "Maru" in group 0 and player "Taeja" in group 0 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Taeja         | Stork         |
		| 1           | Maru          | Rain          |

Scenario: Cannot switch player with other player in same round robin group
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| RoundRobin   | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
	When player "Maru" in group 0 and player "Taeja" in group 0 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Taeja         |
		| 1           | Stork         | Rain          |
		| 2           | Maru          | Rain          |
		| 3           | Taeja         | Stork         |
		| 4           | Maru          | Stork         |
		| 5           | Rain          | Taeja         |

Scenario: Can switch player with other player that resides in different groups within same bracket round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
	When player "Maru" in group 0 and player "Taeja" in group 1 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Taeja          | Stork        |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Maru          | Rain          |

Scenario: Can switch player with other player that resides in different groups within same dual tournament round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
	When player "Maru" in group 0 and player "Thorzain" in group 1 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Thorzain      | Stork         |
		| 1           | Taeja         | Rain          |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Bomber        | FanTaSy       |
			| 1           | Stephano      | Maru          |

Scenario: Cannot switch player with other player that resides in different groups within same round robin round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| RoundRobin   | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
	When player "Maru" in group 0 and player "Taeja" in group 1 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Taeja         | Rain          |

Scenario: Cannot switch player that resides in a bracket group with player in different round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
			| Bracket        | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	When player "Maru" in group 0 and player "Bomber" in group 2 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		| 1           | Taeja         | Rain          |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Bomber        | FanTaSy       |
			| 1           | Stephano      | Thorzain      |
		And participating players in group 2 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Maru          | Rain          |
			| 1           | Bomber        | FanTaSy       |

Scenario: Cannot switch player that resides in a dual tournament group with player in different round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 2                       |
			| Bracket      | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
			| 0                | 0           | 2           |
			| 0                | 0           | 3           |
	When player "Maru" in group 0 and player "Bomber" in group 4 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Taeja         | Rain          |
		And participating players in group 2 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Bomber        | FanTaSy       |
		And participating players in group 3 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Stephano      | Thorzain      |
		And participating players in group 4 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Maru          | Rain          |
			| 1           | Bomber        | Stephano      |

Scenario: Cannot switch player that resides in a round robin group with player in different round
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
			| RoundRobin   | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
			| 0                | 0           | 1           |
	When player "Stork" in group 0 and player "Bomber" in group 2 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		| 1           | Taeja         | Rain          |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Bomber        | FanTaSy       |
			| 1           | Stephano      | Thorzain      |
		And participating players in group 2 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Maru          | Bomber        |

Scenario: Cannot switch player with other player in same bracket group that has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When player "Maru" in group 0 and player "Taeja" in group 0 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		| 1           | Taeja         | Rain          |

Scenario: Cannot switch player with other player in same dual tournament group that has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When player "Maru" in group 0 and player "Taeja" in group 0 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		| 1           | Taeja         | Rain          |

Scenario: Cannot switch player with other player in same round robin group that has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| RoundRobin   | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When player "Maru" in group 0 and player "Taeja" in group 0 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Taeja         |
		| 1           | Stork         | Rain          |
		| 2           | Maru          | Rain          |
		| 3           | Taeja         | Stork         |
		| 4           | Maru          | Stork         |
		| 5           | Rain          | Taeja         |

Scenario: Cannot switch player with other player in different group within same bracket round that has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| Bracket      | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When player "Maru" in group 0 and player "Taeja" in group 1 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Stork         |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Taeja         | Rain          |

Scenario: Cannot switch player with other player in different group within same dual tournament round that has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type   | Advancing per group count | Players per group count |
			| DualTournament | 2                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When player "Maru" in group 0 and player "Thorzain" in group 1 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru      | Stork         |
		| 1           | Taeja     | Rain          |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Bomber        | FanTaSy       |
			| 1           | Stephano      | Thorzain      |

Scenario: Cannot switch player with other player in different group within same round robin round that has started
	Given a tournament named "GSL 2019" has been created
		And tournament 0 adds rounds
			| Contest type | Advancing per group count | Players per group count |
			| RoundRobin   | 1                         | 4                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to tournament 0
		And groups within tournament is played out and betted on
			| Tournament index | Round index | Group index |
			| 0                | 0           | 0           |
	When player "Maru" in group 0 and player "Thorzain" in group 1 switches matches
	Then participating players in group 0 should be mapped accordingly
		| Match index | Player 1 name | Player 2 name |
		| 0           | Maru          | Taeja         |
		| 1           | Stork         | Rain          |
		| 2           | Maru          | Rain          |
		| 3           | Taeja         | Stork         |
		| 4           | Maru          | Stork         |
		| 5           | Rain          | Taeja         |
		And participating players in group 1 should be mapped accordingly
			| Match index | Player 1 name | Player 2 name |
			| 0           | Bomber        | Stephano      |
			| 1           | FanTaSy       | Thorzain      |
			| 2           | Bomber        | Thorzain      |
			| 3           | Stephano      | FanTaSy       |
			| 4           | Bomber        | FanTaSy       |
			| 5           | Thorzain      | Stephano      |
