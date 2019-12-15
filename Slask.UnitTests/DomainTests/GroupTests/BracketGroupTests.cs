﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.GroupTests
{
    public class BracketGroupTests
    {
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;

        public BracketGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = BracketRound.Create("Bracket round", 3, tournament);
        }

        // CREATE UNIT TESTS FOR BRACKET NODE SYSTEM

        // ALL MATCHES MUST BE ORDERED DESCENDING BY STARTDATETIME

        // Create tests for GetPlayState

        // CAN CHANGE LAST EXISTING PLAYER REF TO NULL AND IT IS REMOVED FROM GROUP
        // CAN CHANGE LAST EXISTING PLAYER REF TO ANOTHER PLAYER REF 

        // CANNOT CREATE GROUPS OF TYPES THAT DOES NOT MATCH ROUND TYPE

        [Fact]
        public void CanCreateGroup()
        {
            BracketGroup group = BracketGroup.Create(bracketRound);

            group.Should().NotBeNull();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }

        [Fact]
        public void CanConstructBracketMatchLayout()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part12AddWinningPlayersToBracketGroup(services);

            // Must dig up the test results...

            throw new NotImplementedException();
        }

        [Fact]
        public void CannotIncreaseScoreBeforeMatchHasStartedInGroup()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part14BetsPlacedOnMatchesInBracketGroup(services);
            Domain.Match match = group.Matches.First();

            match.Player1.IncreaseScore(1);
            match.Player2.IncreaseScore(1);

            match.Player1.Score.Should().Be(0);
            match.Player2.Score.Should().Be(0);
        }

        [Fact]
        public void CannotAddNewPlayerToGroupAfterFirstMatchHasStarted()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);

            SystemTimeMocker.Set(DateTime.Now.AddSeconds(1));

            group.AddPlayerReference("Flash").Should().BeNull();

            foreach (Domain.Match match in group.Matches)
            {
                match.FindPlayer("Flash").Should().BeNull();
            }
            group.ParticipatingPlayers.Should().NotContain(player => player.Name == "Flash");
        }

        [Fact]
        public void FetchingAdvancingPlayersInGroupOnlyReturnsWinners()
        {
            TournamentServiceContext services = GivenServices();
            BracketGroup group = HomestoryCupSetup.Part16CompleteAllMatchesInBracketGroup(services);
            //services.SetMockedTime();// DateTimeMockHelper.SetTime(group.Matches.Last().StartDateTime.AddSeconds(1));

            List<PlayerReference> topPlayers = group.GetAdvancingPlayers();

            topPlayers.Should().HaveCount(1);
            topPlayers[0].Name.Should().Be("");
        }

        [Fact]
        public void CannotFetchAdvancingPlayersBeforeGroupIsPlayedOut()
        {
            TournamentServiceContext services = GivenServices();
            RoundRobinGroup group = HomestoryCupSetup.Part08CompleteFirstMatchInRoundRobinGroup(services);

            List<PlayerReference> topPlayers = group.GetAdvancingPlayers();

            topPlayers.Should().BeEmpty();
        }

        private TournamentServiceContext GivenServices()
        {
            return null;
        }
    }
}
