using FluentAssertions;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.GroupTests
{
    public class BracketGroupTests
    {
        private const string firstPlayerName = "Maru";
        private const string secondPlayerName = "Stork";

        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private BracketGroup bracketGroup;
        private Match match;
        private PlayerReference firstPlayerReference;
        private PlayerReference secondPlayerReference;

        public BracketGroupTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound() as BracketRound;
        }

        [Fact]
        public void CanCreateGroup()
        {
            BracketGroup bracketGroup = BracketGroup.Create(bracketRound);

            bracketGroup.Should().NotBeNull();
            bracketGroup.Id.Should().NotBeEmpty();
            bracketGroup.Matches.Should().BeEmpty();
            bracketGroup.RoundId.Should().Be(bracketRound.Id);
            bracketGroup.Round.Should().Be(bracketRound);
            bracketGroup.BracketNodeSystem.Should().BeNull();
        }

        [Fact]
        public void MatchIsCreatedWhenTwoPlayerReferencesAreAddedToGroup()
        {
            RegisterFirstTwoPlayers();

            bracketGroup.Matches.Should().HaveCount(1);
            bracketGroup.Matches.FirstOrDefault(match => match.Player1.GetName() == firstPlayerName).Should().NotBeNull();
            bracketGroup.Matches.FirstOrDefault(match => match.Player2.GetName() == secondPlayerName).Should().NotBeNull();
        }

        [Fact]
        public void CanConstructBracketLayoutWithEvenPlayers()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            BracketGroup bracketGroup = bracketRound.Groups.First() as BracketGroup;
            bracketGroup.Matches.Should().HaveCount(playerNames.Count - 1);

            foreach (Match match in bracketGroup.Matches)
            {
                match.Should().NotBeNull();
            }

            bracketGroup.Matches[0].Player1.GetName().Should().Be(playerNames[0]);
            bracketGroup.Matches[0].Player2.GetName().Should().Be(playerNames[1]);

            bracketGroup.Matches[1].Player1.GetName().Should().Be(playerNames[2]);
            bracketGroup.Matches[1].Player2.GetName().Should().Be(playerNames[3]);

            bracketGroup.Matches[2].Player1.GetName().Should().Be(playerNames[4]);
            bracketGroup.Matches[2].Player2.GetName().Should().Be(playerNames[5]);

            bracketGroup.Matches[3].Player1.GetName().Should().Be(playerNames[6]);
            bracketGroup.Matches[3].Player2.GetName().Should().Be(playerNames[7]);

            bracketGroup.Matches[4].Player1.PlayerReferenceId.Should().BeEmpty();
            bracketGroup.Matches[4].Player2.PlayerReferenceId.Should().BeEmpty();

            bracketGroup.Matches[5].Player1.PlayerReferenceId.Should().BeEmpty();
            bracketGroup.Matches[5].Player2.PlayerReferenceId.Should().BeEmpty();

            bracketGroup.Matches[6].Player1.PlayerReferenceId.Should().BeEmpty();
            bracketGroup.Matches[6].Player2.PlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CanConstructBracketLayoutWithUnevenPlayers()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            BracketGroup bracketGroup = bracketRound.Groups.First() as BracketGroup;
            bracketGroup.Matches.Should().HaveCount(playerNames.Count - 1);

            foreach (Match match in bracketGroup.Matches)
            {
                match.Should().NotBeNull();
            }

            bracketGroup.Matches[0].Player1.GetName().Should().Be(playerNames[0]);
            bracketGroup.Matches[0].Player2.GetName().Should().Be(playerNames[1]);

            bracketGroup.Matches[1].Player1.GetName().Should().Be(playerNames[2]);
            bracketGroup.Matches[1].Player2.GetName().Should().Be(playerNames[3]);

            bracketGroup.Matches[2].Player1.GetName().Should().Be(playerNames[4]);
            bracketGroup.Matches[2].Player2.GetName().Should().Be(playerNames[5]);

            bracketGroup.Matches[3].Player1.GetName().Should().Be(playerNames[6]);
            bracketGroup.Matches[3].Player2.PlayerReferenceId.Should().BeEmpty();

            bracketGroup.Matches[4].Player1.PlayerReferenceId.Should().BeEmpty();
            bracketGroup.Matches[4].Player2.PlayerReferenceId.Should().BeEmpty();

            bracketGroup.Matches[5].Player1.PlayerReferenceId.Should().BeEmpty();
            bracketGroup.Matches[5].Player2.PlayerReferenceId.Should().BeEmpty();
        }

        [Fact]
        public void CanConstructBracketNodeSystemWithEvenPlayers()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano", "Thorzain" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            bracketGroup = bracketRound.Groups.First() as BracketGroup;

            BracketNode finalNode = bracketGroup.BracketNodeSystem.FinalNode;

            ValidateBracketNodeConnections(finalNode, null, bracketGroup.Matches[5], bracketGroup.Matches[4]);

            BracketNode semifinalNode1 = finalNode.Children[0];
            BracketNode semifinalNode2 = finalNode.Children[1];

            ValidateBracketNodeConnections(semifinalNode1, bracketGroup.Matches[6], bracketGroup.Matches[3], bracketGroup.Matches[2]);
            ValidateBracketNodeConnections(semifinalNode2, bracketGroup.Matches[6], bracketGroup.Matches[1], bracketGroup.Matches[0]);

            BracketNode quarterfinalNode1 = semifinalNode1.Children[0];
            BracketNode quarterfinalNode2 = semifinalNode1.Children[1];

            BracketNode quarterfinalNode3 = semifinalNode2.Children[0];
            BracketNode quarterfinalNode4 = semifinalNode2.Children[1];

            ValidateBracketNodeConnections(quarterfinalNode1, bracketGroup.Matches[5], null, null);
            ValidateBracketNodeConnections(quarterfinalNode2, bracketGroup.Matches[5], null, null);

            ValidateBracketNodeConnections(quarterfinalNode3, bracketGroup.Matches[4], null, null);
            ValidateBracketNodeConnections(quarterfinalNode4, bracketGroup.Matches[4], null, null);
        }

        [Fact]
        public void CanConstructBracketNodeSystemWithUnevenPlayers()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            bracketGroup = bracketRound.Groups.First() as BracketGroup;

            BracketNode finalNode = bracketGroup.BracketNodeSystem.FinalNode;

            ValidateBracketNodeConnections(finalNode, null, bracketGroup.Matches[4], bracketGroup.Matches[3]);

            BracketNode semifinalNode1 = finalNode.Children[0];
            BracketNode semifinalNode2 = finalNode.Children[1];

            ValidateBracketNodeConnections(semifinalNode1, bracketGroup.Matches[5], bracketGroup.Matches[2], bracketGroup.Matches[1]);
            ValidateBracketNodeConnections(semifinalNode2, bracketGroup.Matches[5], bracketGroup.Matches[0], null);

            BracketNode quarterfinalNode1 = semifinalNode1.Children[0];
            BracketNode quarterfinalNode2 = semifinalNode1.Children[1];

            BracketNode quarterfinalNode3 = semifinalNode2.Children[0];
            BracketNode quarterfinalNode4 = semifinalNode2.Children[1];

            ValidateBracketNodeConnections(quarterfinalNode1, bracketGroup.Matches[4], null, null);
            ValidateBracketNodeConnections(quarterfinalNode2, bracketGroup.Matches[4], null, null);

            ValidateBracketNodeConnections(quarterfinalNode3, bracketGroup.Matches[3], null, null);
            quarterfinalNode4.Should().BeNull();
        }

        [Fact]
        public void CreatesMatchTierListWhenBracketMatchesAreAdded()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain", "Bomber", "FanTaSy", "Stephano" };
            bracketRound.SetPlayersPerGroupCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            bracketGroup.BracketNodeSystem.TierCount.Should().Be(3);

            List<BracketNode> finalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(0);
            List<BracketNode> semifinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(1);
            List<BracketNode> quarterfinalTier = bracketGroup.BracketNodeSystem.GetBracketNodesInTier(2);

            finalTier.Count.Should().Be(1);
            finalTier[0].Match.Should().Be(bracketGroup.Matches[5]);

            semifinalTier.Count.Should().Be(2);
            semifinalTier[0].Match.Should().Be(bracketGroup.Matches[4]);
            semifinalTier[1].Match.Should().Be(bracketGroup.Matches[3]);

            quarterfinalTier.Count.Should().Be(3);
            quarterfinalTier[0].Match.Should().Be(bracketGroup.Matches[2]);
            quarterfinalTier[1].Match.Should().Be(bracketGroup.Matches[1]);
            quarterfinalTier[2].Match.Should().Be(bracketGroup.Matches[0]);
        }

        private void RegisterFirstTwoPlayers()
        {
            firstPlayerReference = bracketRound.RegisterPlayerReference(firstPlayerName);
            secondPlayerReference = bracketRound.RegisterPlayerReference(secondPlayerName);
            bracketGroup = bracketRound.Groups.First() as BracketGroup;
            match = bracketGroup.Matches.First();
        }

        private void ValidateBracketNodeConnections(BracketNode bracketNode, Match correctParentMatch, Match correctChildMatch1, Match correctChildMatch2)
        {
            ValidateBracketNodeMatch(bracketNode.Parent, correctParentMatch);
            ValidateBracketNodeMatch(bracketNode.Children[0], correctChildMatch1);
            ValidateBracketNodeMatch(bracketNode.Children[1], correctChildMatch2);
        }

        private void ValidateBracketNodeMatch(BracketNode bracketNode, Match correctMatch)
        {
            if (correctMatch == null)
            {
                bracketNode.Should().BeNull();
            }
            else
            {
                bracketNode.Match.Should().Be(correctMatch);
            }
        }
    }
}
