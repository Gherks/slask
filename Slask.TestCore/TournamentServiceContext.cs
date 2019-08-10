using System;
using Slask.Common;
using Slask.Persistence.Services;
using Slask.Domain;
using Slask.Persistence;
using System.Linq;
using Moq;
using System.Collections.Generic;

namespace Slask.TestCore
{
    /* 
     * Can simulate two different tournament scenarios with several checkpoints where tests can be executed. Betters are
     * created and added to each tournament. Every better bets on matches as they become available.
     * 
     * Scenario 1: Homestory Cup
     * Consists of two rounds. First one is a Round Robin round where four players advances to the second round, the 
     * bracket round.
     * 
     * Scenario 2: BHA Open
     * Consists of one Dual Tournament round, with two groups that is played simultaneously. No winner is determined 
     * in this scenario. Use previous scenario if that is needed.
     */
    public class TournamentServiceContext : UserServiceContext
    {
        public TournamentService TournamentService { get; }

        protected TournamentServiceContext(SlaskContext slaskContext)
            : base(slaskContext)
        {
            TournamentService = new TournamentService(SlaskContext);
        }

        public Tournament HomestoryCup_01_CreateTournament()
        {
            Tournament tournament = TournamentService.CreateTournament("Homestory Cup");
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Tournament HomestoryCup_02_BettersAddedToTournament()
        {
            Tournament tournament = HomestoryCup_01_CreateTournament();
            AddBettersToTournament(tournament);

            SlaskContext.SaveChanges();
            return tournament;
        }

        public Round HomestoryCup_03_AddRoundRobinRound()
        {
            Tournament tournament = HomestoryCup_02_BettersAddedToTournament();
            Round round = tournament.AddRoundRobinRound("Round Robin Round", 3, 4);

            SlaskContext.SaveChanges();
            return round;
        }

        public RoundRobinGroup HomestoryCup_04_AddGroupToRoundRobinRound()
        {
            Round round = HomestoryCup_03_AddRoundRobinRound();

            RoundRobinGroup group = (RoundRobinGroup)round.AddGroup();

            SlaskContext.SaveChanges();
            return group;
        }

        public RoundRobinGroup HomestoryCup_05_AddedPlayersToRoundRobinGroup()
        {
            RoundRobinGroup group = HomestoryCup_04_AddGroupToRoundRobinRound();

            group.AddPlayerReference("Maru");
            group.AddPlayerReference("Stork");
            group.AddPlayerReference("Taeja");
            group.AddPlayerReference("Rain");
            group.AddPlayerReference("Bomber");
            group.AddPlayerReference("FanTaSy");
            group.AddPlayerReference("Stephano");
            group.AddPlayerReference("Thorzain");

            SlaskContext.SaveChanges();
            return group;
        }

        public RoundRobinGroup HomestoryCup_06_StartDateTimeSetToMatchesInRoundRobinGroup()
        {
            RoundRobinGroup group = HomestoryCup_05_AddedPlayersToRoundRobinGroup();

            for (int index = 0; index < group.Matches.Count; ++index)
            {
                group.Matches[index].SetStartDateTime(DateTimeHelper.Now.AddHours(index));
            }

            SlaskContext.SaveChanges();
            return group;
        }

        public RoundRobinGroup HomestoryCup_07_AdminFlagsRoundRobinGroupAsReady()
        {
            RoundRobinGroup group = HomestoryCup_06_StartDateTimeSetToMatchesInRoundRobinGroup();

            group.SetIsReady(true);

            SlaskContext.SaveChanges();
            return group;
        }

        public RoundRobinGroup HomestoryCup_08_BetsPlacedOnMatchesInRoundRobinGroup()
        {
            RoundRobinGroup group = HomestoryCup_07_AdminFlagsRoundRobinGroupAsReady();
            BettersPlacesBetsOnAllMatchesInGroups(group);

            SlaskContext.SaveChanges();
            return group;
        }

        public RoundRobinGroup HomestoryCup_09_CompleteFirstMatchInRoundRobinGroup()
        {
            RoundRobinGroup group = HomestoryCup_08_BetsPlacedOnMatchesInRoundRobinGroup();

            group.Matches.First().Player1.IncreaseScore(2);

            SlaskContext.SaveChanges();
            return group;
        }

        public RoundRobinGroup HomestoryCup_10_CompleteAllMatchesInRoundRobinGroup()
        {
            RoundRobinGroup group = HomestoryCup_09_CompleteFirstMatchInRoundRobinGroup();

            for (int index = 1; index < group.Matches.Count; ++index)
            {
                group.Matches[index].Player1.IncreaseScore(2);
            }

            SlaskContext.SaveChanges();
            return group;
        }

        public Round HomestoryCup_11_AddBracketRound()
        {
            RoundRobinGroup group = HomestoryCup_10_CompleteAllMatchesInRoundRobinGroup();
            Tournament tournament = group.Round.Tournament;

            Round round = tournament.AddBracketRound("Bracket Round", 5);

            SlaskContext.SaveChanges();
            return round;
        }

        public BracketGroup HomestoryCup_12_AddGroupToBracketRound()
        {
            Round round = HomestoryCup_11_AddBracketRound();

            BracketGroup group = (BracketGroup)round.AddGroup();

            SlaskContext.SaveChanges();
            return group;
        }

        public BracketGroup HomestoryCup_13_AddWinningPlayersToBracketGroup()
        {
            BracketGroup group = HomestoryCup_12_AddGroupToBracketRound();
            Round previousRound = group.Round.GetPreviousRound();
            List<Player> winningPlayers = previousRound.GetWinningPlayers();

            foreach (Player player in winningPlayers)
            {
                group.AddPlayerReference(player.Name);
            }

            SlaskContext.SaveChanges();
            return group;
        }

        public BracketGroup HomestoryCup_14_StartDateTimeSetToMatchesInBracketGroup()
        {
            BracketGroup group = HomestoryCup_13_AddWinningPlayersToBracketGroup();

            for (int index = 0; index < group.Matches.Count; ++index)
            {
                group.Matches[index].SetStartDateTime(DateTimeHelper.Now.AddDays(1).AddHours(index));
            }

            SlaskContext.SaveChanges();
            return group;
        }

        public BracketGroup HomestoryCup_15_AdminFlagsBracketGroupAsReady()
        {
            BracketGroup group = HomestoryCup_14_StartDateTimeSetToMatchesInBracketGroup();

            group.SetIsReady(true);

            SlaskContext.SaveChanges();
            return group;
        }

        public BracketGroup HomestoryCup_16_BetsPlacedOnMatchesInBracketGroup()
        {
            BracketGroup group = HomestoryCup_15_AdminFlagsBracketGroupAsReady();
            BettersPlacesBetsOnAllMatchesInGroups(group);

            SlaskContext.SaveChanges();
            return group;
        }

        public BracketGroup HomestoryCup_17_CompleteFirstMatchInBracketGroup()
        {
            BracketGroup group = HomestoryCup_16_BetsPlacedOnMatchesInBracketGroup();

            group.Matches.First().Player1.IncreaseScore(3);

            SlaskContext.SaveChanges();
            return group;
        }

        public BracketGroup HomestoryCup_18_CompleteAllMatchesInBracketGroup()
        {
            BracketGroup group = HomestoryCup_17_CompleteFirstMatchInBracketGroup();

            for (int index = 1; index < group.Matches.Count; ++index)
            {
                group.Matches[index].Player1.IncreaseScore(3);
            }

            SlaskContext.SaveChanges();
            return group;
        }
        public Tournament BHAOpen_01_CreateTournament()
        {
            Tournament tournament = TournamentService.CreateTournament("BHA Open");
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Tournament BHAOpen_02_BettersAddedToTournament()
        {
            Tournament tournament = BHAOpen_01_CreateTournament();
            AddBettersToTournament(tournament);

            SlaskContext.SaveChanges();
            return tournament;
        }

        public Round BHAOpen_03_AddDualTournamentRound()
        {
            Tournament tournament = BHAOpen_02_BettersAddedToTournament();
            Round round = tournament.AddDualTournamentRound("Dual Tournament Round", 3);

            SlaskContext.SaveChanges();
            return round;
        }

        public List<DualTournamentGroup> BHAOpen_04_AddGroupsToDualTournamentRound()
        {
            Round round = BHAOpen_03_AddDualTournamentRound();
            List<DualTournamentGroup> groups = new List<DualTournamentGroup>();
            groups.Add((DualTournamentGroup)round.AddGroup());
            groups.Add((DualTournamentGroup)round.AddGroup());

            SlaskContext.SaveChanges();
            return groups;
        }

        public List<DualTournamentGroup> BHAOpen_05_AddedPlayersToDualTournamentGroups()
        {
            List<DualTournamentGroup> groups = BHAOpen_04_AddGroupsToDualTournamentRound();

            groups[0].AddPlayerReference("Stålberto");
            groups[0].AddPlayerReference("Bönis");
            groups[0].AddPlayerReference("Guggelito");
            groups[0].AddPlayerReference("Danneboi");

            groups[1].AddPlayerReference("Bernard");
            groups[1].AddPlayerReference("Papa Puert");
            groups[1].AddPlayerReference("Klubbaxerino");
            groups[1].AddPlayerReference("Segmarken");
            
            SlaskContext.SaveChanges();
            return groups;
        }

        public List<DualTournamentGroup> BHAOpen_06_StartDateTimeSetToMatchesInDualTournamentGroups()
        {
            List<DualTournamentGroup> groups = BHAOpen_05_AddedPlayersToDualTournamentGroups();

            foreach (DualTournamentGroup group in groups)
            {
                for (int index = 0; index < group.Matches.Count; ++index)
                {
                    group.Matches[index].SetStartDateTime(DateTimeHelper.Now.AddHours(index));
                }
            }

            SlaskContext.SaveChanges();
            return groups;
        }

        public List<DualTournamentGroup> BHAOpen_07_AdminFlagsDualTournamentGroupsAsReady()
        {
            List<DualTournamentGroup> groups = BHAOpen_06_StartDateTimeSetToMatchesInDualTournamentGroups();

            foreach (DualTournamentGroup group in groups)
            {
                group.SetIsReady(true);
            }

            SlaskContext.SaveChanges();
            return groups;
        }

        public List<DualTournamentGroup> BHAOpen_08_BetsPlacedOnMatchesInDualTournamentGroups()
        {
            List<DualTournamentGroup> groups = BHAOpen_07_AdminFlagsDualTournamentGroupsAsReady();

            foreach (DualTournamentGroup group in groups)
            {
                BettersPlacesBetsOnAllMatchesInGroups(group);
            }

            SlaskContext.SaveChanges();
            return groups;
        }

        public List<DualTournamentGroup> BHAOpen_09_CompleteFirstMatchInDualTournamentGroups()
        {
            List<DualTournamentGroup> groups = BHAOpen_08_BetsPlacedOnMatchesInDualTournamentGroups();

            foreach (DualTournamentGroup group in groups)
            {
                group.Matches.First().Player1.IncreaseScore(2);
            }

            SlaskContext.SaveChanges();
            return groups;
        }

        public List<DualTournamentGroup> BHAOpen_10_CompleteAllMatchesInDualTournamentGroups()
        {
            List<DualTournamentGroup> groups = BHAOpen_09_CompleteFirstMatchInDualTournamentGroups();

            foreach (DualTournamentGroup group in groups)
            {
                for (int index = 1; index < group.Matches.Count; ++index)
                {
                    group.Matches[index].Player1.IncreaseScore(2);
                }
            }

            SlaskContext.SaveChanges();
            return groups;
        }

        public static new TournamentServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new TournamentServiceContext(slaskContextCreator.CreateContext());
        }

        private void AddBettersToTournament(Tournament tournament)
        {
            WhenCreatedUsers();
            tournament.AddBetter(UserService.GetUserByName("Stålberto"));
            tournament.AddBetter(UserService.GetUserByName("Bönis"));
            tournament.AddBetter(UserService.GetUserByName("Guggelito"));
        }

        private void BettersPlacesBetsOnAllMatchesInGroups(GroupBase group)
        {
            Tournament tournament = group.Round.Tournament;
            Better betterStalberto = tournament.GetBetterByName("Stålberto");
            Better betterBonis = tournament.GetBetterByName("Bönis");
            Better betterGuggelito = tournament.GetBetterByName("Guggelito");

            Random random = new Random(1337);
            int matchCounter = 0;
            foreach (Domain.Match match in group.Matches)
            {
                matchCounter++;

                if (matchCounter == 2)
                {
                    continue;
                }

                betterStalberto.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);
                betterBonis.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);

                if (matchCounter == 3)
                {
                    continue;
                }

                betterGuggelito.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);
            }
        }
    }
}
