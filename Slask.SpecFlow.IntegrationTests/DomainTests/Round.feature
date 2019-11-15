Feature: Round
	Does a bunch of tests on Rounds

@RoundTag
Scenario: Can create bracket round
	When a tournament creates rounds
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |
	Then created rounds in tournament should be valid with values:
		| Round type | Round name    | Best of | Advancing amount |
		| Bracket    | Bracket round | 3       | 1                |

Scenario: Can create dual tournament round
	When a tournament creates rounds
		| Round type      | Round name            | Best of |
		| Dual tournament | Dual tournament round | 3       |
	Then created rounds in tournament should be valid with values:
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 2                |

Scenario: Advancing amount in bracket rounds cannot be anything other than two
	When a tournament creates rounds
		| Round type | Round name      | Best of | Advancing amount |
		| Bracket    | Bracket round 1 | 3       | 0                |
		| Bracket    | Bracket round 2 | 3       | 2                |
		| Bracket    | Bracket round 3 | 3       | 3                |
	Then created rounds in tournament should be valid with values:
		| Round type | Round name      | Best of | Advancing amount |
		| Bracket    | Bracket round 1 | 3       | 1                |
		| Bracket    | Bracket round 2 | 3       | 1                |
		| Bracket    | Bracket round 3 | 3       | 1                |

Scenario: Advancing amount in dual tournament rounds cannot be anything other than two
	When a tournament creates rounds
		| Round type      | Round name              | Best of | Advancing amount |
		| Dual tournament | Dual tournament round 1 | 3       | 0                |
		| Dual tournament | Dual tournament round 2 | 3       | 1                |
		| Dual tournament | Dual tournament round 3 | 3       | 3                |
	Then created rounds in tournament should be valid with values:
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round 1 | 3       | 2                |
		| Dual tournament | Dual tournament round 2 | 3       | 2                |
		| Dual tournament | Dual tournament round 3 | 3       | 2                |

Scenario: Can create round robin round
	When a tournament creates rounds
		| Round type  | Round name    | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |
	Then created rounds in tournament should be valid with values:
		| Round type  | Round name        | Best of | Advancing amount |
		| Round robin | Round robin round | 3       | 1                |

Scenario: Cannot create bracket round without name
	When a tournament creates rounds
		| Round type | Round name | Best of | Advancing amount |
		| Bracket    |            | 3       | 1                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create dual tournament round without name
	When a tournament creates rounds
		| Round type      | Round name | Best of | Advancing amount |
		| Dual tournament |            | 3       | 1                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create round robin round without name
	When a tournament creates rounds
		| Round type | Round name | Best of | Advancing amount |
		| Bracket    |            | 3       | 1                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create bracket round with zero advancers
	When a tournament creates rounds
		| Round type      | Round name    | Best of | Advancing amount |
		| Bracket         | Bracket round | 3       | 0                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create dual tournament round with zero advancers
	When a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 0                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create round robin round with zero advancers
	When a tournament creates rounds
		| Round type      | Round name        | Best of | Advancing amount |
		| Round robin     | Round robin round | 3       | 0                |
	Then created round 0 in tournament should be invalid

Scenario: Cannot create bracket round with less than zero advancers
	When a tournament creates rounds
		| Round type      | Round name      | Best of | Advancing amount |
		| Bracket         | Bracket round 1 | 3       | -1               |
		| Bracket         | Bracket round 2 | 3       | -2               |
		| Bracket         | Bracket round 3 | 3       | -3               |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create dual tournament round with less than zero advancers
	When a tournament creates rounds
		| Round type      | Round name              | Best of | Advancing amount |
		| Dual tournament | Dual tournament round 1 | 3       | -1               |
		| Dual tournament | Dual tournament round 2 | 3       | -2               |
		| Dual tournament | Dual tournament round 3 | 3       | -3               |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with less than zero advancers
	When a tournament creates rounds
		| Round type      | Round name          | Best of | Advancing amount |
		| Round robin     | Round robin round 1 | 3       | -1               |
		| Round robin     | Round robin round 2 | 3       | -2               |
		| Round robin     | Round robin round 3 | 3       | -3               |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create bracket round with even best ofs
	When a tournament creates rounds
		| Round type      | Round name      | Best of | Advancing amount |
		| Bracket         | Bracket round 1 | 0       | 1                |
		| Bracket         | Bracket round 2 | 2       | 1                |
		| Bracket         | Bracket round 3 | 4       | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create dual tournament round with even best ofs
	When a tournament creates rounds
		| Round type      | Round name              | Best of | Advancing amount |
		| Dual tournament | Dual tournament round 1 | 0       | 1                |
		| Dual tournament | Dual tournament round 2 | 2       | 1                |
		| Dual tournament | Dual tournament round 3 | 4       | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with even best ofs
	When a tournament creates rounds
		| Round type      | Round name          | Best of | Advancing amount |
		| Round robin     | Round robin round 1 | 0       | 1                |
		| Round robin     | Round robin round 2 | 2       | 1                |
		| Round robin     | Round robin round 3 | 4       | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create bracket round with best ofs less than zero
	When a tournament creates rounds
		| Round type | Round name      | Best of | Advancing amount |
		| Bracket    | Bracket round 1 | -1      | 1                |
		| Bracket    | Bracket round 2 | -2      | 1                |
		| Bracket    | Bracket round 3 | -3      | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create dual tournament round with best ofs less than zero
	When a tournament creates rounds
		| Round type       | Round name              | Best of | Advancing amount |
		|  Dual tournament | Dual tournament round 1 | -1      | 1                |
		|  Dual tournament | Dual tournament round 2 | -2      | 1                |
		|  Dual tournament | Dual tournament round 3 | -3      | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Cannot create round robin round with best ofs less than zero
	When a tournament creates rounds
		| Round type  | Round name          | Best of | Advancing amount |
		| Round robin | Round robin round 1 | -1      | 1                |
		| Round robin | Round robin round 2 | -2      | 1                |
		| Round robin | Round robin round 3 | -3      | 1                |
	Then created round 0 in tournament should be invalid
		And created round 1 in tournament should be invalid
		And created round 2 in tournament should be invalid

Scenario: Can fetch previous round from round with round predecessor
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 1                |
		| Bracket         | Bracket round         | 3       | 1                |
	When created round 1 fetches previous round
	Then fetched round 0 in tournament should be valid with values:
		| Round type      | Round name            | Best of | Advancing amount |
		| Dual tournament | Dual tournament round | 3       | 1                |

Scenario: Cannot fetch previous round with first round
	Given a tournament creates rounds
		| Round type      | Round name            | Best of | Advancing amount |
		| Bracket         | Bracket round         | 3       | 1                |
	When created round 0 fetches previous round
	Then fetched round 0 in tournament should be invalid



#Scenario: Only winning players can advance to next round
#Scenario: Solve round robin ties
#// ADVANCING PLAYERS MUST ALWAYS BE EQUAL OR LESS THAN NUMBER OF GROUPS IN ROUND


#        // ADVANCING PLAYERS MUST ALWAYS BE EQUAL OR LESS THAN NUMBER OF GROUPS IN ROUND
#
#        [Fact]
#        public void CanCreateRoundRobinRound()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round round = HomestoryCupSetup.Part03_AddRoundRobinRound(services);
#
#            round.Should().NotBeNull();
#            round.Id.Should().NotBeEmpty();
#            round.Name.Should().Be("Round Robin Round");
#            round.Type.Should().Be(RoundType.RoundRobin);
#            round.BestOf.Should().Be(3);
#            round.AdvancingPerGroupAmount.Should().Be(4);
#            round.Groups.Should().BeEmpty();
#            round.TournamentId.Should().NotBeEmpty();
#            round.Tournament.Should().NotBeNull();
#        }
#
#        [Fact]
#        public void CanCreateDualTournamentRound()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round round = BHAOpenSetup.Part03_AddDualTournamentRound(services);
#
#            round.Should().NotBeNull();
#            round.Id.Should().NotBeEmpty();
#            round.Name.Should().Be("Dual Tournament Round");
#            round.Type.Should().Be(RoundType.DualTournament);
#            round.BestOf.Should().Be(3);
#            round.AdvancingPerGroupAmount.Should().Be(2);
#            round.Groups.Should().BeEmpty();
#            round.TournamentId.Should().NotBeEmpty();
#            round.Tournament.Should().NotBeNull();
#        }
#
#        [Fact]
#        public void CanCreateBracketRound()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round round = HomestoryCupSetup.Part10_AddBracketRound(services);
#
#            round.Should().NotBeNull();
#            round.Id.Should().NotBeEmpty();
#            round.Name.Should().Be("Bracket Round");
#            round.Type.Should().Be(RoundType.Bracket);
#            round.BestOf.Should().Be(5);
#            round.AdvancingPerGroupAmount.Should().Be(1);
#            round.Groups.Should().BeEmpty();
#            round.TournamentId.Should().NotBeEmpty();
#            round.Tournament.Should().NotBeNull();
#        }
#
#        [Fact]
#        public void CannotCreateRoundsWithoutName()
#        {
#            TournamentServiceContext services = GivenServices();
#            Tournament tournament = HomestoryCupSetup.Part01_CreateTournament(services);
#            Round roundRobinRound = tournament.AddRoundRobinRound("", 3, 1);
#            Round dualTournamentRound = tournament.AddDualTournamentRound("", 3);
#            Round bracketRound = tournament.AddBracketRound("", 3);
#
#            roundRobinRound.Should().BeNull();
#            dualTournamentRound.Should().BeNull();
#            bracketRound.Should().BeNull();
#        }
#
#        [Fact]
#        public void CannotCreateRoundsWithZeroAdvancers()
#        {
#            TournamentServiceContext services = GivenServices();
#            Tournament tournament = HomestoryCupSetup.Part01_CreateTournament(services);
#            Round roundRobinRound = tournament.AddRoundRobinRound("Round Robin Round", 3, 0);
#            Round dualTournamentRound = tournament.AddDualTournamentRound("Dual Tournament Round", 3);
#            Round bracketRound = tournament.AddBracketRound("Bracket Round", 3);
#
#            roundRobinRound.Should().BeNull();
#            dualTournamentRound.AdvancingPerGroupAmount.Should().NotBe(0);
#            bracketRound.AdvancingPerGroupAmount.Should().NotBe(0);
#        }
#
#        [Fact]
#        public void CannotCreateRoundsWithEvenBestOfs()
#        {
#            TournamentServiceContext services = GivenServices();
#            Tournament tournament = HomestoryCupSetup.Part01_CreateTournament(services);
#
#            for (int bestOf = 0; bestOf < 21; bestOf += 2)
#            {
#                tournament.AddRoundRobinRound("Round Robin Round", bestOf, 4).Should().BeNull();
#                tournament.AddDualTournamentRound("Dual Tournament Round", bestOf).Should().BeNull();
#                tournament.AddBracketRound("Round Robin Round", bestOf).Should().BeNull();
#            }
#        }
#
#        [Fact]
#        public void ReturnsNullWhenFetchingPreviousRoundWithFirstRound()
#        {
#            TournamentServiceContext services = GivenServices();
#            RoundRobinGroup group = HomestoryCupSetup.Part04_AddedGroupToRoundRobinRound(services);
#
#            Round round = group.Round.GetPreviousRound();
#
#            round.Should().BeNull();
#        }
#
#        [Fact]
#        public void CanFetchPreviousRoundFromRoundWithRoundPredecessor()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round currentRound = HomestoryCupSetup.Part10_AddBracketRound(services);
#            Tournament tournament = currentRound.Tournament;
#
#            Round previousRound = currentRound.GetPreviousRound();
#
#            previousRound.Should().NotBeNull();
#            previousRound.Should().Be(tournament.Rounds.First());
#        }
#
#        [Fact]
#        public void OnlyWinningPlayersCanAdvanceToNextRound()
#        {
#            TournamentServiceContext services = GivenServices();
#            BracketGroup group = HomestoryCupSetup.Part12_AddWinningPlayersToBracketGroup(services);
#
#            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "Taeja").Should().NotBeNull();
#            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "FanTaSy").Should().NotBeNull();
#            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "Thorzain").Should().NotBeNull();
#            group.ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == "Rain").Should().NotBeNull();
#        }
#
#        [Fact]
#        public void SolveRoundRobinTies()
#        {
#            throw new NotImplementedException();
#        }
#
#        [Fact]
#        public void CannotAddGroupsToRoundRobinRoundThatDoesNotMatchByType()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round round = HomestoryCupSetup.Part03_AddRoundRobinRound(services);
#            round.AddGroup();
#
#            RoundRobinGroup group = round.Groups.First() as RoundRobinGroup;
#
#            round.Groups.Should().HaveCount(1);
#            group.Should().NotBeOfType<DualTournamentGroup>();
#            group.Should().NotBeOfType<BracketGroup>();
#            group.Should().BeOfType<RoundRobinGroup>();
#        }
#
#        [Fact]
#        public void CannotAddGroupsToDualTournamentRoundThatDoesNotMatchByType()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round round = BHAOpenSetup.Part03_AddDualTournamentRound(services);
#            round.AddGroup();
#
#            DualTournamentGroup group = round.Groups.First() as DualTournamentGroup;
#
#            round.Groups.Should().HaveCount(1);
#            group.Should().NotBeOfType<RoundRobinGroup>();
#            group.Should().NotBeOfType<BracketGroup>();
#            group.Should().BeOfType<DualTournamentGroup>();
#        }
#
#        [Fact]
#        public void CannotAddGroupsToBracketRoundThatDoesNotMatchByType()
#        {
#            TournamentServiceContext services = GivenServices();
#            Round round = HomestoryCupSetup.Part10_AddBracketRound(services);
#            round.AddGroup();
#
#            BracketGroup group = round.Groups.First() as BracketGroup;
#
#            round.Groups.Should().HaveCount(1);
#            group.Should().NotBeOfType<RoundRobinGroup>();
#            group.Should().NotBeOfType<DualTournamentGroup>();
#            group.Should().BeOfType<BracketGroup>();
#        }