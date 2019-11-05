using Slask.Common;
using Slask.Domain;
using Slask.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.TestCore
{
    /* 
     * Consists of one Dual Tournament round, with two groups that is played simultaneously. No winner is determined 
     * in this scenario.
     */
    public class BHAOpenSetup
    {
        public static Tournament Part01_CreateTournament(TournamentServiceContext serviceContext)
        {
            Tournament tournament = serviceContext.WhenCreatedTournament("BHA Open");

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

        public static Round Part03_AddDualTournamentRound(TournamentServiceContext serviceContext)
        {
            Tournament tournament = Part02_BettersAddedToTournament(serviceContext);

            Round round = serviceContext.WhenAddedDualTournamentRoundToTournament(tournament, "Dual Tournament Round", 3);

            serviceContext.SaveChanges();
            return round;
        }

        public static List<DualTournamentGroup> Part04_AddGroupsToDualTournamentRound(TournamentServiceContext serviceContext)
        {
            Round round = Part03_AddDualTournamentRound(serviceContext);

            List<DualTournamentGroup> groups = new List<DualTournamentGroup>();
            groups.Add((DualTournamentGroup)serviceContext.WhenAddedGroupToRound(round));
            groups.Add((DualTournamentGroup)serviceContext.WhenAddedGroupToRound(round));

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part05_AddedPlayersToDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            List<DualTournamentGroup> groups = Part04_AddGroupsToDualTournamentRound(serviceContext);

            serviceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Stålberto");
            serviceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Bönis");
            serviceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Guggelito");
            serviceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Danneboi");

            serviceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Bernard");
            serviceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Papa Puert");
            serviceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Klubbaxerino");
            serviceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Segmarken");

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part06_StartDateTimeSetToMatchesInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            List<DualTournamentGroup> groups = Part05_AddedPlayersToDualTournamentGroups(serviceContext);

            foreach (DualTournamentGroup group in groups)
            {
                for (int index = 0; index < group.Matches.Count; ++index)
                {
                    serviceContext.WhenSetStartDateTimeOnMatch(group.Matches[index], SystemTime.Now.AddDays(1).AddHours(1 + index));
                }
            }

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part07_BetsPlacedOnMatchesInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            List<DualTournamentGroup> groups = Part06_StartDateTimeSetToMatchesInDualTournamentGroups(serviceContext);

            foreach (DualTournamentGroup group in groups)
            {
                serviceContext.WhenBettersPlacesBetsOnAllMatchesInGroups(group);
            }

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part08_CompleteFirstMatchInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            List<DualTournamentGroup> groups = Part07_BetsPlacedOnMatchesInDualTournamentGroups(serviceContext);

            serviceContext.WhenPlayerScoreIncreased(groups.First().Matches.First().Player1, 2);

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part09_CompleteAllMatchesInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            List<DualTournamentGroup> groups = Part08_CompleteFirstMatchInDualTournamentGroups(serviceContext);

            Random random = new Random(133742069);

            foreach (DualTournamentGroup group in groups)
            {
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
            }

            serviceContext.SaveChanges();
            return groups;
        }
    }
}
