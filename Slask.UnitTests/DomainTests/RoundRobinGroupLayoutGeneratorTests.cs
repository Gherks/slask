using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class RoundRobinGroupLayoutAssemblerTests
    {
        private readonly Tournament tournament;
        private readonly RoundRobinRound roundRobinRound;
        private readonly RoundRobinGroup roundRobinGroup;
        private readonly List<PlayerReference> playerReferences;

        public RoundRobinGroupLayoutAssemblerTests()
        {
            tournament = Tournament.Create("GSL 2019");
            roundRobinRound = tournament.AddRoundRobinRound("Round robin round", 3, 2) as RoundRobinRound;
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
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithTwoPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithThreePlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithFourPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithFivePlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithSixPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithSevenPlayers(matches);

            matches = RoundRobinGroupLayoutAssembler.ConstructMathes(++groupSizeCounter, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences.GetRange(0, groupSizeCounter), matches);
            RunTestsWithEightPlayers(matches);
        }

        [Fact]
        public void CannotFillMatchesWithPlayersWithoutValidPlayerReferenceList()
        {
            List<Match> matches = null;

            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(null, matches);

            matches.Should().BeNull();
        }

        [Fact]
        public void CannotFillMatchesWithPlayersWithoutValidMatchList()
        {
            List<Match> matches = null;

            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences, matches);

            matches.Should().BeNull();
        }

        [Fact]
        public void CannotFillMatchesWithPlayersWhenMatchesCannotFitAllPlayerReferences()
        {
            List<Match> matches = RoundRobinGroupLayoutAssembler.ConstructMathes(playerReferences.Count / 2, roundRobinGroup);
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences, matches);

            foreach (Match match in matches)
            {
                match.Player1.Should().BeNull();
                match.Player2.Should().BeNull();
            }
        }

        private void RunTestsWithTwoPlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Stork");
        }

        private void RunTestsWithThreePlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Taeja");

            matches[1].Player1.Name.Should().Be("Taeja");
            matches[1].Player2.Name.Should().Be("Stork");

            matches[2].Player1.Name.Should().Be("Stork");
            matches[2].Player2.Name.Should().Be("Maru");
        }

        private void RunTestsWithFourPlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Taeja");

            matches[1].Player1.Name.Should().Be("Stork");
            matches[1].Player2.Name.Should().Be("Rain");

            matches[2].Player1.Name.Should().Be("Maru");
            matches[2].Player2.Name.Should().Be("Rain");

            matches[3].Player1.Name.Should().Be("Taeja");
            matches[3].Player2.Name.Should().Be("Stork");

            matches[4].Player1.Name.Should().Be("Maru");
            matches[4].Player2.Name.Should().Be("Stork");

            matches[5].Player1.Name.Should().Be("Rain");
            matches[5].Player2.Name.Should().Be("Taeja");
        }

        private void RunTestsWithFivePlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Rain");

            matches[1].Player1.Name.Should().Be("Stork");
            matches[1].Player2.Name.Should().Be("Bomber");

            matches[2].Player1.Name.Should().Be("Rain");
            matches[2].Player2.Name.Should().Be("Bomber");

            matches[3].Player1.Name.Should().Be("Maru");
            matches[3].Player2.Name.Should().Be("Taeja");

            matches[4].Player1.Name.Should().Be("Bomber");
            matches[4].Player2.Name.Should().Be("Taeja");

            matches[5].Player1.Name.Should().Be("Rain");
            matches[5].Player2.Name.Should().Be("Stork");

            matches[6].Player1.Name.Should().Be("Taeja");
            matches[6].Player2.Name.Should().Be("Stork");

            matches[7].Player1.Name.Should().Be("Bomber");
            matches[7].Player2.Name.Should().Be("Maru");

            matches[8].Player1.Name.Should().Be("Stork");
            matches[8].Player2.Name.Should().Be("Maru");

            matches[9].Player1.Name.Should().Be("Taeja");
            matches[9].Player2.Name.Should().Be("Rain");
        }

        private void RunTestsWithSixPlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Rain");

            matches[1].Player1.Name.Should().Be("Stork");
            matches[1].Player2.Name.Should().Be("Bomber");

            matches[2].Player1.Name.Should().Be("Taeja");
            matches[2].Player2.Name.Should().Be("FanTaSy");

            matches[3].Player1.Name.Should().Be("Maru");
            matches[3].Player2.Name.Should().Be("Bomber");

            matches[4].Player1.Name.Should().Be("Rain");
            matches[4].Player2.Name.Should().Be("FanTaSy");

            matches[5].Player1.Name.Should().Be("Stork");
            matches[5].Player2.Name.Should().Be("Taeja");

            matches[6].Player1.Name.Should().Be("Maru");
            matches[6].Player2.Name.Should().Be("FanTaSy");

            matches[7].Player1.Name.Should().Be("Bomber");
            matches[7].Player2.Name.Should().Be("Taeja");

            matches[8].Player1.Name.Should().Be("Rain");
            matches[8].Player2.Name.Should().Be("Stork");

            matches[9].Player1.Name.Should().Be("Maru");
            matches[9].Player2.Name.Should().Be("Taeja");

            matches[10].Player1.Name.Should().Be("FanTaSy");
            matches[10].Player2.Name.Should().Be("Stork");

            matches[11].Player1.Name.Should().Be("Bomber");
            matches[11].Player2.Name.Should().Be("Rain");

            matches[12].Player1.Name.Should().Be("Maru");
            matches[12].Player2.Name.Should().Be("Stork");

            matches[13].Player1.Name.Should().Be("Taeja");
            matches[13].Player2.Name.Should().Be("Rain");

            matches[14].Player1.Name.Should().Be("FanTaSy");
            matches[14].Player2.Name.Should().Be("Bomber");
        }

        private void RunTestsWithSevenPlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Bomber");

            matches[1].Player1.Name.Should().Be("Stork");
            matches[1].Player2.Name.Should().Be("FanTaSy");

            matches[2].Player1.Name.Should().Be("Taeja");
            matches[2].Player2.Name.Should().Be("Stephano");

            matches[3].Player1.Name.Should().Be("Bomber");
            matches[3].Player2.Name.Should().Be("FanTaSy");

            matches[4].Player1.Name.Should().Be("Maru");
            matches[4].Player2.Name.Should().Be("Stephano");

            matches[5].Player1.Name.Should().Be("Stork");
            matches[5].Player2.Name.Should().Be("Rain");

            matches[6].Player1.Name.Should().Be("FanTaSy");
            matches[6].Player2.Name.Should().Be("Stephano");

            matches[7].Player1.Name.Should().Be("Bomber");
            matches[7].Player2.Name.Should().Be("Rain");

            matches[8].Player1.Name.Should().Be("Maru");
            matches[8].Player2.Name.Should().Be("Taeja");

            matches[9].Player1.Name.Should().Be("Stephano");
            matches[9].Player2.Name.Should().Be("Rain");

            matches[10].Player1.Name.Should().Be("FanTaSy");
            matches[10].Player2.Name.Should().Be("Taeja");

            matches[11].Player1.Name.Should().Be("Bomber");
            matches[11].Player2.Name.Should().Be("Stork");

            matches[12].Player1.Name.Should().Be("Rain");
            matches[12].Player2.Name.Should().Be("Taeja");

            matches[13].Player1.Name.Should().Be("Stephano");
            matches[13].Player2.Name.Should().Be("Stork");

            matches[14].Player1.Name.Should().Be("FanTaSy");
            matches[14].Player2.Name.Should().Be("Maru");

            matches[15].Player1.Name.Should().Be("Taeja");
            matches[15].Player2.Name.Should().Be("Stork");

            matches[16].Player1.Name.Should().Be("Rain");
            matches[16].Player2.Name.Should().Be("Maru");

            matches[17].Player1.Name.Should().Be("Stephano");
            matches[17].Player2.Name.Should().Be("Bomber");

            matches[18].Player1.Name.Should().Be("Stork");
            matches[18].Player2.Name.Should().Be("Maru");

            matches[19].Player1.Name.Should().Be("Taeja");
            matches[19].Player2.Name.Should().Be("Bomber");

            matches[20].Player1.Name.Should().Be("Rain");
            matches[20].Player2.Name.Should().Be("FanTaSy");
        }

        private void RunTestsWithEightPlayers(List<Match> matches)
        {
            matches[0].Player1.Name.Should().Be("Maru");
            matches[0].Player2.Name.Should().Be("Bomber");

            matches[1].Player1.Name.Should().Be("Stork");
            matches[1].Player2.Name.Should().Be("FanTaSy");

            matches[2].Player1.Name.Should().Be("Taeja");
            matches[2].Player2.Name.Should().Be("Stephano");

            matches[3].Player1.Name.Should().Be("Rain");
            matches[3].Player2.Name.Should().Be("Thorzain");

            matches[4].Player1.Name.Should().Be("Maru");
            matches[4].Player2.Name.Should().Be("FanTaSy");

            matches[5].Player1.Name.Should().Be("Bomber");
            matches[5].Player2.Name.Should().Be("Stephano");

            matches[6].Player1.Name.Should().Be("Stork");
            matches[6].Player2.Name.Should().Be("Thorzain");

            matches[7].Player1.Name.Should().Be("Taeja");
            matches[7].Player2.Name.Should().Be("Rain");

            matches[8].Player1.Name.Should().Be("Maru");
            matches[8].Player2.Name.Should().Be("Stephano");

            matches[9].Player1.Name.Should().Be("FanTaSy");
            matches[9].Player2.Name.Should().Be("Thorzain");

            matches[10].Player1.Name.Should().Be("Bomber");
            matches[10].Player2.Name.Should().Be("Rain");

            matches[11].Player1.Name.Should().Be("Stork");
            matches[11].Player2.Name.Should().Be("Taeja");

            matches[12].Player1.Name.Should().Be("Maru");
            matches[12].Player2.Name.Should().Be("Thorzain");

            matches[13].Player1.Name.Should().Be("Stephano");
            matches[13].Player2.Name.Should().Be("Rain");

            matches[14].Player1.Name.Should().Be("FanTaSy");
            matches[14].Player2.Name.Should().Be("Taeja");

            matches[15].Player1.Name.Should().Be("Bomber");
            matches[15].Player2.Name.Should().Be("Stork");

            matches[16].Player1.Name.Should().Be("Maru");
            matches[16].Player2.Name.Should().Be("Rain");

            matches[17].Player1.Name.Should().Be("Thorzain");
            matches[17].Player2.Name.Should().Be("Taeja");

            matches[18].Player1.Name.Should().Be("Stephano");
            matches[18].Player2.Name.Should().Be("Stork");

            matches[19].Player1.Name.Should().Be("FanTaSy");
            matches[19].Player2.Name.Should().Be("Bomber");

            matches[20].Player1.Name.Should().Be("Maru");
            matches[20].Player2.Name.Should().Be("Taeja");

            matches[21].Player1.Name.Should().Be("Rain");
            matches[21].Player2.Name.Should().Be("Stork");

            matches[22].Player1.Name.Should().Be("Thorzain");
            matches[22].Player2.Name.Should().Be("Bomber");

            matches[23].Player1.Name.Should().Be("Stephano");
            matches[23].Player2.Name.Should().Be("FanTaSy");

            matches[24].Player1.Name.Should().Be("Maru");
            matches[24].Player2.Name.Should().Be("Stork");

            matches[25].Player1.Name.Should().Be("Taeja");
            matches[25].Player2.Name.Should().Be("Bomber");

            matches[26].Player1.Name.Should().Be("Rain");
            matches[26].Player2.Name.Should().Be("FanTaSy");

            matches[27].Player1.Name.Should().Be("Thorzain");
            matches[27].Player2.Name.Should().Be("Stephano");
        }
    }
}
