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

            WhenCreatedUsers();
            tournament.AddBetter(UserService.GetUserByName("Stålberto"));
            tournament.AddBetter(UserService.GetUserByName("Bönis"));
            tournament.AddBetter(UserService.GetUserByName("Guggelito"));
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Round HomestoryCup_03_AddRoundRobinRound()
        {
            Tournament tournament = HomestoryCup_02_BettersAddedToTournament();
            Round groupStage = tournament.AddRoundRobinRound("Round Robin Round", 3, 4);

            SlaskContext.SaveChanges();
            return groupStage;
        }

        public RoundRobinGroup HomestoryCup_04_AddGroupToRoundRobinRound()
        {
            Round groupStage = HomestoryCup_03_AddRoundRobinRound();

            RoundRobinGroup group = (RoundRobinGroup)groupStage.AddGroup();

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

            for(int index = 1; index < group.Matches.Count; ++index)
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

            Round round = tournament.AddBracketRound("Bracket", 5);

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
            List<Player> winningPlayers = group.Round.GetWinningPlayersOfPreviousRound();

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

        //public Tournament WhenCreatedACompleteTournamentWithStagesRoundRobinIntoBracket()
        //{
        //    #region Wall of Text

        //    WhenCreatedUsers();
        //    Tournament tournament = WhenTournamentCreated();
        //    WhenBettersAddedToTournament(tournament);

        //    //////////////////////////////////////////////////////////////////////////

        //    Round groupStage = tournament.AddRoundRobinRound("Round Robin Tournament", 3, 4);

        //    //////////////////////////////////////////////////////////////////////////

        //    RoundRobinGroup roundRobinGroup = groupStage.AddGroup<RoundRobinGroup>();

        //    //////////////////////////////////////////////////////////////////////////

        //    // Players added                IF FIRST ROUND PLAYERS CAN BE ADDED FREELY, OTHERWISE THEY NEED TO BE PICKED FROM A POOL OF PREVIOUS ADVANCES
        //    roundRobinGroup.AddPlayerReference("Maru");
        //    roundRobinGroup.AddPlayerReference("Stork");
        //    roundRobinGroup.AddPlayerReference("Taeja");
        //    roundRobinGroup.AddPlayerReference("Rain");
        //    roundRobinGroup.AddPlayerReference("Bomber");
        //    roundRobinGroup.AddPlayerReference("FanTaSy");
        //    roundRobinGroup.AddPlayerReference("Stephano");
        //    roundRobinGroup.AddPlayerReference("Thorzain");

        //    //////////////////////////////////////////////////////////////////////////

        //    // Valid dates in future set    DEFAULT TIME SET IS 1 WEEK AHEAD, EACH CONSEQUTIVE MATCH SEPARATED BY 1 HOUR
        //    for(int index = 0; index < roundRobinGroup.Matches.Count; ++index)
        //    {
        //        roundRobinGroup.Matches[index].ChangeStartDateTime(DateTimeHelper.Now.AddHours(index));
        //    }

        //    //////////////////////////////////////////////////////////////////////////

        //    // Admin flags roundRobinGroup as ready        LOCK/UNLOCKING FEATURE NEEDS TO BE ADDED TO GROUP, GROUP VALIDITY SHOULD BE DONE BEFORE BETTING IS ENABLED - THIS FLAG IS NEEDED SO AUTO-SEED IS POSSIBLE AND ADMIN CAN EDIT GROUP BEFORE BETTING COMMENCES

        //    roundRobinGroup.SetIsReady(true);

        //    //////////////////////////////////////////////////////////////////////////

        //    // Betting ensues               BETTING IS ENABLED FOR EACH MATCH UNTIL STARTDATETIME IS PAST

        //    Better betterStalberto = tournament.GetBetterByName("Stålberto");
        //    Better betterBonis = tournament.GetBetterByName("Bönis");
        //    Better betterGuggelito = tournament.GetBetterByName("Guggelito");

        //    Random random = new Random(1337);
        //    int matchCounter = 0;
        //    foreach (Domain.Match match in roundRobinGroup.Matches)
        //    {
        //        matchCounter++;

        //        if(matchCounter == 2)
        //        {
        //            continue;
        //        }

        //        betterStalberto.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);
        //        betterBonis.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);

        //        if (matchCounter == 3)
        //        {
        //            continue;
        //        }

        //        betterGuggelito.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);
        //    }

        //    //////////////////////////////////////////////////////////////////////////

        //    // First match is played        BETTING IS LOCKED FOR FIRST MATCH

        //    roundRobinGroup.Matches.First().Player1.AddScore(2);

        //    //////////////////////////////////////////////////////////////////////////

        //    // All other matches are played

        //    for (int index = 1; index < roundRobinGroup.Matches.Count; ++index)
        //    {
        //        roundRobinGroup.Matches[index].Player1.AddScore(2);
        //    }

        //    //////////////////////////////////////////////////////////////////////////

        //    // RoundRobinGroup played, winners move onto the bracket stage

        //    Round bracketRound = tournament.AddBracketRound("Bracket", 5);

        //    //////////////////////////////////////////////////////////////////////////

        //    BracketGroup bracketGroup = bracketRound.AddGroup<BracketGroup>();

        //    //////////////////////////////////////////////////////////////////////////

        //    List<Player> winningPlayers = bracketGroup.Round.GetWinningPlayersOfPreviousRound();

        //    foreach (Player player in winningPlayers)
        //    {
        //        bracketGroup.AddPlayerReference(player.Name);
        //    }

        //    //////////////////////////////////////////////////////////////////////////

        //    for (int index = 0; index < bracketGroup.Matches.Count; ++index)
        //    {
        //        bracketGroup.Matches[index].ChangeStartDateTime(DateTimeHelper.Now.AddDays(1).AddHours(index));
        //    }

        //    //////////////////////////////////////////////////////////////////////////

        //    bracketGroup.SetIsReady(true);

        //    //////////////////////////////////////////////////////////////////////////

        //    //Random random = new Random(1337);
        //    //int matchCounter = 0;
        //    foreach (Domain.Match match in bracketGroup.Matches)
        //    {
        //        matchCounter++;

        //        if (matchCounter == 2)
        //        {
        //            continue;
        //        }

        //        betterStalberto.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);
        //        betterBonis.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);

        //        if (matchCounter == 3)
        //        {
        //            continue;
        //        }

        //        betterGuggelito.PlaceMatchBet(match, random.Next(1) == 0 ? match.Player1 : match.Player2);
        //    }

        //    //////////////////////////////////////////////////////////////////////////

        //    bracketGroup.Matches.First().Player1.AddScore(3);

        //    //////////////////////////////////////////////////////////////////////////

        //    // All other matches are played

        //    for (int index = 1; index < bracketGroup.Matches.Count; ++index)
        //    {
        //        bracketGroup.Matches[index].Player1.AddScore(3);
        //    }

        //    #endregion

        //    //////////////////////////////////////////////////////////////////////////

        //    //// Play matches, add score

        //    bracketGroup.Matches[0].Player1.AddScore(3); // Maru advances, Taeja eliminated
        //    bracketGroup.Matches[1].Player1.AddScore(3); // Bomber advances, Stephano eliminated
        //    bracketGroup.Matches[2].Player1.AddScore(3); // Maru wins, Bomber loses

        //    //////////////////////////////////////////////////////////////////////////

        //    SlaskContext.SaveChanges();

        //    return tournament;
        //}

        public static new TournamentServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new TournamentServiceContext(slaskContextCreator.CreateContext());
        }
    }
}
