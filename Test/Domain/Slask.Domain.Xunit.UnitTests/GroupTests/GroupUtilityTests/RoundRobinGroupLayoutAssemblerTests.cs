using FluentAssertions;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Collections.Generic;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests.GroupTests.GroupUtilityTests
{
    public class RoundRobinGroupLayoutAssemblerTests
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound roundRobinRound;
        private readonly RoundRobinGroup roundRobinGroup;
        private readonly List<PlayerReference> playerReferences;

        private readonly Guid maruId;
        private readonly Guid storkId;
        private readonly Guid taejaId;
        private readonly Guid rainId;
        private readonly Guid bomberId;
        private readonly Guid fantasyId;
        private readonly Guid stephanoId;
        private readonly Guid thorzainId;

        public RoundRobinGroupLayoutAssemblerTests()
        {
            tournament = Tournament.Create("GSL 2019");
            roundRobinRound = tournament.AddRoundRobinRound() as RoundRobinRound;
            roundRobinGroup = RoundRobinGroup.Create(roundRobinRound);

            playerReferences = new List<PlayerReference>();
            playerReferences.Add(PlayerReference.Create("Maru", tournament));
            playerReferences.Add(PlayerReference.Create("Stork", tournament));
            playerReferences.Add(PlayerReference.Create("Taeja", tournament));
            playerReferences.Add(PlayerReference.Create("Rain", tournament));
            playerReferences.Add(PlayerReference.Create("Bomber", tournament));
            playerReferences.Add(PlayerReference.Create("FanTaSy", tournament));
            playerReferences.Add(PlayerReference.Create("Stephano", tournament));
            playerReferences.Add(PlayerReference.Create("Thorzain", tournament));

            maruId = playerReferences[0].Id;
            storkId = playerReferences[1].Id;
            taejaId = playerReferences[2].Id;
            rainId = playerReferences[3].Id;
            bomberId = playerReferences[4].Id;
            fantasyId = playerReferences[5].Id;
            stephanoId = playerReferences[6].Id;
            thorzainId = playerReferences[7].Id;
        }

        [Fact]
        public void ConstructsAppropriateCountOfMatchesDependingOnGroupSize()
        {
            int groupSizeCounter = 0;
            List<Match> matches;

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(0);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(1);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(3);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(6);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(10);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(15);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(21);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            matches.Should().HaveCount(28);
        }

        [Fact]
        public void CannotConstructMatchesWithoutValidGroupProvided()
        {
            RoundRobinGroupLayoutAssembler.ConstructMathes(4, null).Should().BeEmpty();
        }

        [Fact]
        public void CanFillConstructedMatchesWithPlayers()
        {
            int groupSizeCounter = 1;
            List<Match> matches;

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithTwoPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithThreePlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithFourPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithFivePlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithSixPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithSevenPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithEightPlayers(matches);
        }

        [Fact]
        public void CannotAssignPlayersToMatchesWithoutValidPlayerReferenceList()
        {
            List<Match> matches = null;

            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(null, matches);

            matches.Should().BeNull();
        }

        [Fact]
        public void CannotAssignPlayersToMatchesWithoutValidMatchList()
        {
            List<Match> matches = null;

            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences, matches);

            matches.Should().BeNull();
        }

        [Fact]
        public void CanAssignPlayersToMatchesWhenPlayerReferencesWontFillAllMatches()
        {
            int halfPlayerReferenceCount = playerReferences.Count / 2;

            List<Match> matches = RoundRobinGroupLayoutAssembler.ConstructMathes(playerReferences.Count, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences.GetRange(0, halfPlayerReferenceCount), matches);

            int index = 0;
            int validMatchesCount = 6;

            for (; index < validMatchesCount; ++index)
            {
                matches[index].PlayerReference1Id.Should().NotBeEmpty();
                matches[index].PlayerReference2Id.Should().NotBeEmpty();
            }

            for (; index < matches.Count; ++index)
            {
                matches[index].PlayerReference1Id.Should().BeEmpty();
                matches[index].PlayerReference2Id.Should().BeEmpty();
            }
        }

        [Fact]
        public void CannotAssignPlayersToMatchesWhenMatchesCannotFitAllPlayerReferences()
        {
            int halfPlayerReferenceCount = playerReferences.Count / 2;

            List<Match> matches = RoundRobinGroupLayoutAssembler.ConstructMathes(halfPlayerReferenceCount, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.AssignPlayersToMatches(playerReferences, matches);

            foreach (Match match in matches)
            {
                match.PlayerReference1Id.Should().BeEmpty();
                match.PlayerReference2Id.Should().BeEmpty();
            }
        }

        private void RunTestsWithTwoPlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(storkId);
        }

        private void RunTestsWithThreePlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(taejaId);

            matches[1].PlayerReference1Id.Should().Be(taejaId);
            matches[1].PlayerReference2Id.Should().Be(storkId);

            matches[2].PlayerReference1Id.Should().Be(storkId);
            matches[2].PlayerReference2Id.Should().Be(maruId);
        }

        private void RunTestsWithFourPlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(taejaId);

            matches[1].PlayerReference1Id.Should().Be(storkId);
            matches[1].PlayerReference2Id.Should().Be(rainId);

            matches[2].PlayerReference1Id.Should().Be(maruId);
            matches[2].PlayerReference2Id.Should().Be(rainId);

            matches[3].PlayerReference1Id.Should().Be(taejaId);
            matches[3].PlayerReference2Id.Should().Be(storkId);

            matches[4].PlayerReference1Id.Should().Be(maruId);
            matches[4].PlayerReference2Id.Should().Be(storkId);

            matches[5].PlayerReference1Id.Should().Be(rainId);
            matches[5].PlayerReference2Id.Should().Be(taejaId);
        }

        private void RunTestsWithFivePlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(rainId);

            matches[1].PlayerReference1Id.Should().Be(storkId);
            matches[1].PlayerReference2Id.Should().Be(bomberId);

            matches[2].PlayerReference1Id.Should().Be(rainId);
            matches[2].PlayerReference2Id.Should().Be(bomberId);

            matches[3].PlayerReference1Id.Should().Be(maruId);
            matches[3].PlayerReference2Id.Should().Be(taejaId);

            matches[4].PlayerReference1Id.Should().Be(bomberId);
            matches[4].PlayerReference2Id.Should().Be(taejaId);

            matches[5].PlayerReference1Id.Should().Be(rainId);
            matches[5].PlayerReference2Id.Should().Be(storkId);

            matches[6].PlayerReference1Id.Should().Be(taejaId);
            matches[6].PlayerReference2Id.Should().Be(storkId);

            matches[7].PlayerReference1Id.Should().Be(bomberId);
            matches[7].PlayerReference2Id.Should().Be(maruId);

            matches[8].PlayerReference1Id.Should().Be(storkId);
            matches[8].PlayerReference2Id.Should().Be(maruId);

            matches[9].PlayerReference1Id.Should().Be(taejaId);
            matches[9].PlayerReference2Id.Should().Be(rainId);
        }

        private void RunTestsWithSixPlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(rainId);

            matches[1].PlayerReference1Id.Should().Be(storkId);
            matches[1].PlayerReference2Id.Should().Be(bomberId);

            matches[2].PlayerReference1Id.Should().Be(taejaId);
            matches[2].PlayerReference2Id.Should().Be(fantasyId);

            matches[3].PlayerReference1Id.Should().Be(maruId);
            matches[3].PlayerReference2Id.Should().Be(bomberId);

            matches[4].PlayerReference1Id.Should().Be(rainId);
            matches[4].PlayerReference2Id.Should().Be(fantasyId);

            matches[5].PlayerReference1Id.Should().Be(storkId);
            matches[5].PlayerReference2Id.Should().Be(taejaId);

            matches[6].PlayerReference1Id.Should().Be(maruId);
            matches[6].PlayerReference2Id.Should().Be(fantasyId);

            matches[7].PlayerReference1Id.Should().Be(bomberId);
            matches[7].PlayerReference2Id.Should().Be(taejaId);

            matches[8].PlayerReference1Id.Should().Be(rainId);
            matches[8].PlayerReference2Id.Should().Be(storkId);

            matches[9].PlayerReference1Id.Should().Be(maruId);
            matches[9].PlayerReference2Id.Should().Be(taejaId);

            matches[10].PlayerReference1Id.Should().Be(fantasyId);
            matches[10].PlayerReference2Id.Should().Be(storkId);

            matches[11].PlayerReference1Id.Should().Be(bomberId);
            matches[11].PlayerReference2Id.Should().Be(rainId);

            matches[12].PlayerReference1Id.Should().Be(maruId);
            matches[12].PlayerReference2Id.Should().Be(storkId);

            matches[13].PlayerReference1Id.Should().Be(taejaId);
            matches[13].PlayerReference2Id.Should().Be(rainId);

            matches[14].PlayerReference1Id.Should().Be(fantasyId);
            matches[14].PlayerReference2Id.Should().Be(bomberId);
        }

        private void RunTestsWithSevenPlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(bomberId);

            matches[1].PlayerReference1Id.Should().Be(storkId);
            matches[1].PlayerReference2Id.Should().Be(fantasyId);

            matches[2].PlayerReference1Id.Should().Be(taejaId);
            matches[2].PlayerReference2Id.Should().Be(stephanoId);

            matches[3].PlayerReference1Id.Should().Be(bomberId);
            matches[3].PlayerReference2Id.Should().Be(fantasyId);

            matches[4].PlayerReference1Id.Should().Be(maruId);
            matches[4].PlayerReference2Id.Should().Be(stephanoId);

            matches[5].PlayerReference1Id.Should().Be(storkId);
            matches[5].PlayerReference2Id.Should().Be(rainId);

            matches[6].PlayerReference1Id.Should().Be(fantasyId);
            matches[6].PlayerReference2Id.Should().Be(stephanoId);

            matches[7].PlayerReference1Id.Should().Be(bomberId);
            matches[7].PlayerReference2Id.Should().Be(rainId);

            matches[8].PlayerReference1Id.Should().Be(maruId);
            matches[8].PlayerReference2Id.Should().Be(taejaId);

            matches[9].PlayerReference1Id.Should().Be(stephanoId);
            matches[9].PlayerReference2Id.Should().Be(rainId);

            matches[10].PlayerReference1Id.Should().Be(fantasyId);
            matches[10].PlayerReference2Id.Should().Be(taejaId);

            matches[11].PlayerReference1Id.Should().Be(bomberId);
            matches[11].PlayerReference2Id.Should().Be(storkId);

            matches[12].PlayerReference1Id.Should().Be(rainId);
            matches[12].PlayerReference2Id.Should().Be(taejaId);

            matches[13].PlayerReference1Id.Should().Be(stephanoId);
            matches[13].PlayerReference2Id.Should().Be(storkId);

            matches[14].PlayerReference1Id.Should().Be(fantasyId);
            matches[14].PlayerReference2Id.Should().Be(maruId);

            matches[15].PlayerReference1Id.Should().Be(taejaId);
            matches[15].PlayerReference2Id.Should().Be(storkId);

            matches[16].PlayerReference1Id.Should().Be(rainId);
            matches[16].PlayerReference2Id.Should().Be(maruId);

            matches[17].PlayerReference1Id.Should().Be(stephanoId);
            matches[17].PlayerReference2Id.Should().Be(bomberId);

            matches[18].PlayerReference1Id.Should().Be(storkId);
            matches[18].PlayerReference2Id.Should().Be(maruId);

            matches[19].PlayerReference1Id.Should().Be(taejaId);
            matches[19].PlayerReference2Id.Should().Be(bomberId);

            matches[20].PlayerReference1Id.Should().Be(rainId);
            matches[20].PlayerReference2Id.Should().Be(fantasyId);
        }

        private void RunTestsWithEightPlayers(List<Match> matches)
        {
            matches[0].PlayerReference1Id.Should().Be(maruId);
            matches[0].PlayerReference2Id.Should().Be(bomberId);

            matches[1].PlayerReference1Id.Should().Be(storkId);
            matches[1].PlayerReference2Id.Should().Be(fantasyId);

            matches[2].PlayerReference1Id.Should().Be(taejaId);
            matches[2].PlayerReference2Id.Should().Be(stephanoId);

            matches[3].PlayerReference1Id.Should().Be(rainId);
            matches[3].PlayerReference2Id.Should().Be(thorzainId);

            matches[4].PlayerReference1Id.Should().Be(maruId);
            matches[4].PlayerReference2Id.Should().Be(fantasyId);

            matches[5].PlayerReference1Id.Should().Be(bomberId);
            matches[5].PlayerReference2Id.Should().Be(stephanoId);

            matches[6].PlayerReference1Id.Should().Be(storkId);
            matches[6].PlayerReference2Id.Should().Be(thorzainId);

            matches[7].PlayerReference1Id.Should().Be(taejaId);
            matches[7].PlayerReference2Id.Should().Be(rainId);

            matches[8].PlayerReference1Id.Should().Be(maruId);
            matches[8].PlayerReference2Id.Should().Be(stephanoId);

            matches[9].PlayerReference1Id.Should().Be(fantasyId);
            matches[9].PlayerReference2Id.Should().Be(thorzainId);

            matches[10].PlayerReference1Id.Should().Be(bomberId);
            matches[10].PlayerReference2Id.Should().Be(rainId);

            matches[11].PlayerReference1Id.Should().Be(storkId);
            matches[11].PlayerReference2Id.Should().Be(taejaId);

            matches[12].PlayerReference1Id.Should().Be(maruId);
            matches[12].PlayerReference2Id.Should().Be(thorzainId);

            matches[13].PlayerReference1Id.Should().Be(stephanoId);
            matches[13].PlayerReference2Id.Should().Be(rainId);

            matches[14].PlayerReference1Id.Should().Be(fantasyId);
            matches[14].PlayerReference2Id.Should().Be(taejaId);

            matches[15].PlayerReference1Id.Should().Be(bomberId);
            matches[15].PlayerReference2Id.Should().Be(storkId);

            matches[16].PlayerReference1Id.Should().Be(maruId);
            matches[16].PlayerReference2Id.Should().Be(rainId);

            matches[17].PlayerReference1Id.Should().Be(thorzainId);
            matches[17].PlayerReference2Id.Should().Be(taejaId);

            matches[18].PlayerReference1Id.Should().Be(stephanoId);
            matches[18].PlayerReference2Id.Should().Be(storkId);

            matches[19].PlayerReference1Id.Should().Be(fantasyId);
            matches[19].PlayerReference2Id.Should().Be(bomberId);

            matches[20].PlayerReference1Id.Should().Be(maruId);
            matches[20].PlayerReference2Id.Should().Be(taejaId);

            matches[21].PlayerReference1Id.Should().Be(rainId);
            matches[21].PlayerReference2Id.Should().Be(storkId);

            matches[22].PlayerReference1Id.Should().Be(thorzainId);
            matches[22].PlayerReference2Id.Should().Be(bomberId);

            matches[23].PlayerReference1Id.Should().Be(stephanoId);
            matches[23].PlayerReference2Id.Should().Be(fantasyId);

            matches[24].PlayerReference1Id.Should().Be(maruId);
            matches[24].PlayerReference2Id.Should().Be(storkId);

            matches[25].PlayerReference1Id.Should().Be(taejaId);
            matches[25].PlayerReference2Id.Should().Be(bomberId);

            matches[26].PlayerReference1Id.Should().Be(rainId);
            matches[26].PlayerReference2Id.Should().Be(fantasyId);

            matches[27].PlayerReference1Id.Should().Be(thorzainId);
            matches[27].PlayerReference2Id.Should().Be(stephanoId);
        }
    }
}
