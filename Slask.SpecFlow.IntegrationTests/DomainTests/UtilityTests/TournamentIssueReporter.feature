Feature: TournamentIssueReporter
	Does a bunch of tests on TournamentIssueReporter

@TournamentIssueReporterTag
Scenario: Tournament issue is reported when last round is configured to have more than one advancer
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 2                         | 4                       |
	When players "Maru, Stork, Taeja, Rain" is registered to round 0
	Then tournament 0 reports issues
		| Issue type |
		| Round      |

Scenario: Tournament issue is reported when last round contains more than one group
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then tournament 0 reports issues
		| Issue type |
		| Round      |

Scenario: Tournament issue is reported when a round does not synergize with previous round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name          | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round 1 | 3       | 1                         | 4                       |
			| Round robin | Round robin round 2 | 3       | 1                         | 4                       |
	When players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
	Then tournament 0 reports issues
		| Issue type |
		| Round      |

Scenario: Tournament issue is reported when not enough players has been registered to fill first round
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 1                         | 4                       |
	When players "Maru, Stork" is registered to round 0
	Then tournament 0 reports issues
		| Issue type |
		| Round      |

Scenario: Tournament issue validation is run when new round is added
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name        | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round | 3       | 2                         | 4                       |
	When players "Maru, Stork, Taeja, Rain" is registered to round 0
	Then tournament 0 reports issues
		| Issue type |
		| Round      |

Scenario: Tournament issue validation is run when round is removed
	Given a tournament named "GSL 2019" has been created
		And created tournament 0 adds rounds
			| Round type  | Round name          | Best of | Advancing per group count | Players per group count |
			| Round robin | Round robin round 1 | 3       | 2                         | 4                       |
			| Round robin | Round robin round 2 | 3       | 2                         | 4                       |
			| Round robin | Round robin round 3 | 3       | 1                         | 2                       |
		And players "Maru, Stork, Taeja, Rain, Bomber, FanTaSy, Stephano, Thorzain" is registered to round 0
		And tournament 0 reports issues
			| Issue type |
	When round 1 is removed from tournament 0
	Then tournament 0 reports issues
		| Issue type |
		| Round      |
