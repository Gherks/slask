using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Persistence.Xunit.IntegrationTests.tournamentRepositoryTests
{
    public class RoundTests : tournamentRepositoryTestBase
    {
        [Fact]
        public void CanAddBracketRoundToTournament()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().BeEmpty();

                tournamentRepository.AddBracketRoundToTournament(tournament);
                tournamentRepository.Save();

                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<BracketRound>();
            }

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<BracketRound>();
            }
        }

        [Fact]
        public void CanAddDualTournamentRoundToTournament()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().BeEmpty();

                tournamentRepository.AddDualTournamentRoundToTournament(tournament);
                tournamentRepository.Save();

                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<DualTournamentRound>();
            }

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<DualTournamentRound>();
            }
        }

        [Fact]
        public void CanAddRoundRobinRoundToTournament()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().BeEmpty();

                tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                tournamentRepository.Save();

                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<RoundRobinRound>();
            }

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                tournament.Rounds.Should().HaveCount(1);
                tournament.Rounds.First().Should().BeOfType<RoundRobinRound>();
            }
        }

        [Fact]
        public void CanRemoveBracketRoundFromTournamentById()
        {
            List<Guid> roundIds = new List<Guid>();

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                RoundBase round = tournamentRepository.AddBracketRoundToTournament(tournament);
                roundIds.Add(round.Id);

                round = tournamentRepository.AddDualTournamentRoundToTournament(tournament);
                roundIds.Add(round.Id);

                round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                roundIds.Add(round.Id);

                tournamentRepository.Save();
            }

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                foreach (Guid roundId in roundIds)
                {
                    bool removeResult = tournamentRepository.RemoveRoundFromTournament(tournament, roundId);
                    tournamentRepository.Save();

                    removeResult.Should().BeTrue();
                }

                tournament.Rounds.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanRemoveBracketRoundFromTournamentByName()
        {
            List<string> roundNames = new List<string>();

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                RoundBase round = tournamentRepository.AddBracketRoundToTournament(tournament);
                roundNames.Add(round.Name);

                round = tournamentRepository.AddDualTournamentRoundToTournament(tournament);
                roundNames.Add(round.Name);

                round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);
                roundNames.Add(round.Name);

                tournamentRepository.Save();
            }

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);

                foreach (string roundName in roundNames)
                {
                    bool removeResult = tournamentRepository.RemoveRoundFromTournament(tournament, roundName);
                    tournamentRepository.Save();

                    removeResult.Should().BeTrue();
                }

                tournament.Rounds.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanRenameRoundsInTournament()
        {
            string newName = "Round No Spamerino In The Chatterino";

            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddBracketRoundToTournament(tournament);
                round.Name.Should().Be("Round A");

                bool renameResult = tournamentRepository.RenameRoundInTournament(round, newName);

                renameResult.Should().BeTrue();
                round.Name.Should().Be(newName);
            }
        }

        [Fact]
        public void CannotChangeAdvancingPerGroupCountInBracketRound()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddBracketRoundToTournament(tournament);

                const int newAdvancingCount = 3;
                const int expectedAdvancingCount = 1;

                bool changeResult = tournamentRepository.SetAdvancingPerGroupCountInRound(round, newAdvancingCount);

                changeResult.Should().BeFalse();
                round.AdvancingPerGroupCount.Should().Be(expectedAdvancingCount);
            }
        }

        [Fact]
        public void CannotChangeAdvancingPerGroupCountInDualTournamentRound()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddDualTournamentRoundToTournament(tournament);

                const int newAdvancingCount = 3;
                const int expectedAdvancingCount = 2;

                bool changeResult = tournamentRepository.SetAdvancingPerGroupCountInRound(round, newAdvancingCount);

                changeResult.Should().BeFalse();
                round.AdvancingPerGroupCount.Should().Be(expectedAdvancingCount);
            }
        }

        [Fact]
        public void CanChangeAdvancingPerGroupCountInRoundRobinRound()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);

                const int newAdvancingCount = 3;
                const int expectedAdvancingCount = newAdvancingCount;

                bool changeResult = tournamentRepository.SetAdvancingPerGroupCountInRound(round, newAdvancingCount);

                changeResult.Should().BeTrue();
                round.AdvancingPerGroupCount.Should().Be(expectedAdvancingCount);
            }
        }

        [Fact]
        public void CanChangePlayersPerGroupCountInBracketRound()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);

                const int newPlayersPerGroupCount = 7;
                const int expectedPlayersPerGroupCount = newPlayersPerGroupCount;

                bool changeResult = tournamentRepository.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);

                changeResult.Should().BeTrue();
                round.PlayersPerGroupCount.Should().Be(expectedPlayersPerGroupCount);
            }
        }

        [Fact]
        public void CannotChangePlayersPerGroupCountInDualTournamentRound()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddDualTournamentRoundToTournament(tournament);

                const int newPlayersPerGroupCount = 3;
                const int expectedPlayersPerGroupCount = 4;

                bool changeResult = tournamentRepository.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);

                changeResult.Should().BeFalse();
                round.PlayersPerGroupCount.Should().Be(expectedPlayersPerGroupCount);
            }
        }

        [Fact]
        public void CanChangePlayersPerGroupCountInRoundRobinRound()
        {
            using (TournamentRepository tournamentRepository = CreatetournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournamentByName(tournamentName);
                RoundBase round = tournamentRepository.AddRoundRobinRoundToTournament(tournament);

                const int newPlayersPerGroupCount = 7;
                const int expectedPlayersPerGroupCount = newPlayersPerGroupCount;

                bool changeResult = tournamentRepository.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);

                changeResult.Should().BeTrue();
                round.PlayersPerGroupCount.Should().Be(expectedPlayersPerGroupCount);
            }
        }
    }
}
