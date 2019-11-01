using Slask.Common;
using Slask.Domain;
using Slask.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.TestCore
{
    /* 
     * Consists of two rounds. First one is a Round Robin round where four players advances to the second round, the 
     * bracket round.
     */
    public class HomestoryCupSetup
    {
        public static Tournament Part01_CreateTournament(TournamentServiceContext serviceContext)
        {
            Tournament tournament = serviceContext.WhenCreatedTournament("Homestory Cup");

            serviceContext.SaveChanges();
            return tournament;
        }

        public static Tournament Part02_BettersAddedToTournament(TournamentServiceContext serviceContext)
        {
            Tournament tournament = Part01_CreateTournament(serviceContext);

            serviceContext.WhenCreatedUsers();
            serviceContext.WhenAddedBettersToTournament(tournament);

            serviceContext.SaveChanges();
            return tournament;
        }

        public static Round Part03_AddRoundRobinRound(TournamentServiceContext serviceContext)
        {
            Tournament tournament = Part02_BettersAddedToTournament(serviceContext);

            Round round = serviceContext.WhenAddedRoundRobinRoundToTournament(tournament, "Round Robin Round", 3, 4);

            serviceContext.SaveChanges();
            return round;
        }

        public static RoundRobinGroup Part04_AddedGroupToRoundRobinRound(TournamentServiceContext serviceContext)
        {
            Round round = Part03_AddRoundRobinRound(serviceContext);

            RoundRobinGroup group = (RoundRobinGroup)serviceContext.WhenAddedGroupToRound(round);

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part05_AddedPlayersToRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            RoundRobinGroup group = Part04_AddedGroupToRoundRobinRound(serviceContext);

            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Maru");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Stork");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Taeja");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Rain");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Bomber");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "FanTaSy");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Stephano");
            serviceContext.WhenAddedPlayerReferenceToGroup(group, "Thorzain");

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part06_StartDateTimeSetToMatchesInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            RoundRobinGroup group = Part05_AddedPlayersToRoundRobinGroup(serviceContext);

            for (int index = 0; index < group.Matches.Count; ++index)
            {
                serviceContext.WhenSetStartDateTimeOnMatch(group.Matches[index], SystemTime.Now.AddHours(1 + index));
            }

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part07_BetsPlacedOnMatchesInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            RoundRobinGroup group = Part06_StartDateTimeSetToMatchesInRoundRobinGroup(serviceContext);

            serviceContext.WhenBettersPlacesBetsOnAllMatchesInGroups(group);

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part08_CompleteFirstMatchInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            RoundRobinGroup group = Part07_BetsPlacedOnMatchesInRoundRobinGroup(serviceContext);

            serviceContext.WhenPlayerScoreIncreased(group.Matches.First().Player1, 2);

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part09_CompleteAllMatchesInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            RoundRobinGroup group = Part08_CompleteFirstMatchInRoundRobinGroup(serviceContext);

            Random random = new Random(133742069);

            foreach (Match match in group.Matches)
            {
                if (match.GetPlayState() != PlayState.IsFinished)
                {
                    bool increasePlayer1Score = random.Next(2) == 0;

                    if (increasePlayer1Score)
                    {
                        serviceContext.WhenPlayerScoreIncreased(match.Player1, 2);
                    }
                    else
                    {
                        serviceContext.WhenPlayerScoreIncreased(match.Player2, 2);
                    }
                }
            }

            serviceContext.SaveChanges();
            return group;
        }

        public static Round Part10_AddBracketRound(TournamentServiceContext serviceContext)
        {
            RoundRobinGroup group = Part09_CompleteAllMatchesInRoundRobinGroup(serviceContext);
            Tournament tournament = group.Round.Tournament;

            Round round = serviceContext.WhenAddedBracketRoundToTournament(tournament, "Bracket Round", 5);

            serviceContext.SaveChanges();
            return round;
        }

        public static BracketGroup Part11_AddGroupToBracketRound(TournamentServiceContext serviceContext)
        {
            Round round = Part10_AddBracketRound(serviceContext);

            BracketGroup group = (BracketGroup)serviceContext.WhenAddedGroupToRound(round);

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part12_AddWinningPlayersToBracketGroup(TournamentServiceContext serviceContext)
        {
            BracketGroup group = Part11_AddGroupToBracketRound(serviceContext);
            Round previousRound = group.Round.GetPreviousRound();
            List<PlayerReference> winningPlayers = previousRound.GetAdvancingPlayers();

            foreach (PlayerReference player in winningPlayers)
            {
                group.AddPlayerReference(player.Name);
            }

            // 6 Taeja
            // 4 FanTaSy
            // 4 Thorzain
            // 4 Rain
            // 3 Maru
            // 3 Bomber
            // 2 Stephano
            // 2 Stork

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part13_StartDateTimeSetToMatchesInBracketGroup(TournamentServiceContext serviceContext)
        {
            BracketGroup group = Part12_AddWinningPlayersToBracketGroup(serviceContext);

            for (int index = 0; index < group.Matches.Count; ++index)
            {
                serviceContext.WhenSetStartDateTimeOnMatch(group.Matches[index], SystemTime.Now.AddDays(1).AddHours(1 + index));
            }

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part14_BetsPlacedOnMatchesInBracketGroup(TournamentServiceContext serviceContext)
        {
            BracketGroup group = Part13_StartDateTimeSetToMatchesInBracketGroup(serviceContext);

            serviceContext.WhenBettersPlacesBetsOnAllMatchesInGroups(group);

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part15_CompleteFirstMatchInBracketGroup(TournamentServiceContext serviceContext)
        {
            BracketGroup group = Part14_BetsPlacedOnMatchesInBracketGroup(serviceContext);

            serviceContext.WhenPlayerScoreIncreased(group.Matches.First().Player1, 3);

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part16_CompleteAllMatchesInBracketGroup(TournamentServiceContext serviceContext)
        {
            BracketGroup group = Part15_CompleteFirstMatchInBracketGroup(serviceContext);

            Random random = new Random(133742069);

            foreach (Match match in group.Matches)
            {
                if (match.GetPlayState() != PlayState.IsFinished)
                {
                    bool increasePlayer1Score = random.Next(2) == 0;

                    if (increasePlayer1Score)
                    {
                        serviceContext.WhenPlayerScoreIncreased(match.Player1, 3);
                    }
                    else
                    {
                        serviceContext.WhenPlayerScoreIncreased(match.Player2, 3);
                    }
                }
            }

            serviceContext.SaveChanges();
            return group;
        }
    }
}
