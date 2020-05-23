﻿using FluentAssertions;
using Slask.Domain;
using Xunit;

namespace Slask.Xunit.UnitTests.DomainTests
{
    public class PlayerReferenceTests
    {
        private readonly Tournament tournament;

        public PlayerReferenceTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void CanCreatePlayerReference()
        {
            string name = "Maru";

            PlayerReference playerReference = PlayerReference.Create(name, tournament);

            playerReference.Should().NotBeNull();
            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(name);
            playerReference.TournamentId.Should().Be(tournament.Id);
            playerReference.Tournament.Should().Be(tournament);
        }

        [Fact]
        public void CannotCreatePlayerReferenceWithEmptyname()
        {
            PlayerReference playerReference = PlayerReference.Create("", tournament);

            playerReference.Should().BeNull();
        }

        [Fact]
        public void CannotCreatePlayerReferenceWithNullTournament()
        {
            PlayerReference playerReference = PlayerReference.Create("Maru", null);

            playerReference.Should().BeNull();
        }

        [Fact]
        public void PlayerReferenceCanBeRenamed()
        {
            PlayerReference playerReference = PlayerReference.Create("Maru", tournament);

            playerReference.RenameTo("Idra");

            playerReference.Name.Should().Be("Idra");
        }

        [Fact]
        public void PlayerReferenceCannotBeRenamedToEmptyName()
        {
            PlayerReference playerReference = PlayerReference.Create("Maru", tournament);

            playerReference.RenameTo("");

            playerReference.Name.Should().Be("Maru");
        }
    }
}
