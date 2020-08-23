using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence.Services;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.TournamentServiceTests
{
    public class RoundTests : TournamentServiceTestBase
    {
        [Fact]
        public void CanAddBracketRoundToTournament()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().BeEmpty();

                tournamentService.AddBracketRoundToTournament(tournament);
                tournamentService.Save();

                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<BracketRound>();
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<BracketRound>();
            }
        }

        [Fact]
        public void CanAddDualTournamentRoundToTournament()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().BeEmpty();

                tournamentService.AddDualTournamentRoundToTournament(tournament);
                tournamentService.Save();

                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<DualTournamentRound>();
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<DualTournamentRound>();
            }
        }

        [Fact]
        public void CanAddRoundRobinRoundToTournament()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().BeEmpty();

                tournamentService.AddRoundRobinRoundToTournament(tournament);
                tournamentService.Save();

                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<RoundRobinRound>();
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<RoundRobinRound>();
            }
        }

        [Fact]
        public void CanRemoveBracketRoundFromTournamentById()
        {
            List<Guid> roundIds = new List<Guid>();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                RoundBase round = tournamentService.AddBracketRoundToTournament(tournament);
                roundIds.Add(round.Id);

                round = tournamentService.AddDualTournamentRoundToTournament(tournament);
                roundIds.Add(round.Id);

                round = tournamentService.AddRoundRobinRoundToTournament(tournament);
                roundIds.Add(round.Id);

                tournamentService.Save();
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (Guid roundId in roundIds)
                {
                    bool removeResult = tournamentService.RemoveRoundFromTournament(tournament, roundId);
                    tournamentService.Save();

                    removeResult.Should().BeTrue();
                }

                tournament.Rounds.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanRemoveBracketRoundFromTournamentByName()
        {
            List<string> roundNames = new List<string>();

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                RoundBase round = tournamentService.AddBracketRoundToTournament(tournament);
                roundNames.Add(round.Name);

                round = tournamentService.AddDualTournamentRoundToTournament(tournament);
                roundNames.Add(round.Name);

                round = tournamentService.AddRoundRobinRoundToTournament(tournament);
                roundNames.Add(round.Name);

                tournamentService.Save();
            }

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);

                foreach (string roundName in roundNames)
                {
                    bool removeResult = tournamentService.RemoveRoundFromTournament(tournament, roundName);
                    tournamentService.Save();

                    removeResult.Should().BeTrue();
                }

                tournament.Rounds.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanRenameRoundsInTournament()
        {
            string newName = "Round No Spamerino In The Chatterino";

            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddBracketRoundToTournament(tournament);
                round.Name.Should().Be("Round A");

                bool renameResult = tournamentService.RenameRoundInTournament(round, newName);

                renameResult.Should().BeTrue();
                round.Name.Should().Be(newName);
            }
        }

        [Fact]
        public void CannotChangeAdvancingPerGroupCountInBracketRound()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddBracketRoundToTournament(tournament);

                const int newAdvancingCount = 3;
                const int expectedAdvancingCount = 1;

                bool changeResult = tournamentService.SetAdvancingPerGroupCountInRound(round, newAdvancingCount);
                
                changeResult.Should().BeFalse();
                round.AdvancingPerGroupCount.Should().Be(expectedAdvancingCount);
            }
        }

        [Fact]
        public void CannotChangeAdvancingPerGroupCountInDualTournamentRound()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddDualTournamentRoundToTournament(tournament);

                const int newAdvancingCount = 3;
                const int expectedAdvancingCount = 2;

                bool changeResult = tournamentService.SetAdvancingPerGroupCountInRound(round, newAdvancingCount);

                changeResult.Should().BeFalse();
                round.AdvancingPerGroupCount.Should().Be(expectedAdvancingCount);
            }
        }

        [Fact]
        public void CanChangeAdvancingPerGroupCountInRoundRobinRound()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddRoundRobinRoundToTournament(tournament);

                const int newAdvancingCount = 3;
                const int expectedAdvancingCount = newAdvancingCount;

                bool changeResult = tournamentService.SetAdvancingPerGroupCountInRound(round, newAdvancingCount);

                changeResult.Should().BeTrue();
                round.AdvancingPerGroupCount.Should().Be(expectedAdvancingCount);
            }
        }

        [Fact]
        public void CanChangePlayersPerGroupCountInBracketRound()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddRoundRobinRoundToTournament(tournament);

                const int newPlayersPerGroupCount = 7;
                const int expectedPlayersPerGroupCount = newPlayersPerGroupCount;

                bool changeResult = tournamentService.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);

                changeResult.Should().BeTrue();
                round.PlayersPerGroupCount.Should().Be(expectedPlayersPerGroupCount);
            }
        }

        [Fact]
        public void CannotChangePlayersPerGroupCountInDualTournamentRound()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddDualTournamentRoundToTournament(tournament);

                const int newPlayersPerGroupCount = 3;
                const int expectedPlayersPerGroupCount = 4;

                bool changeResult = tournamentService.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);

                changeResult.Should().BeFalse();
                round.PlayersPerGroupCount.Should().Be(expectedPlayersPerGroupCount);
            }
        }

        [Fact]
        public void CanChangePlayersPerGroupCountInRoundRobinRound()
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                RoundBase round = tournamentService.AddRoundRobinRoundToTournament(tournament);

                const int newPlayersPerGroupCount = 7;
                const int expectedPlayersPerGroupCount = newPlayersPerGroupCount;

                bool changeResult = tournamentService.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);

                changeResult.Should().BeTrue();
                round.PlayersPerGroupCount.Should().Be(expectedPlayersPerGroupCount);
            }
        }
    }
}
