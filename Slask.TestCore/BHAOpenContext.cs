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
    public static class BHAOpenSetup
    {
        public static Tournament Part01CreateTournament(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            Tournament tournament = serviceContext.WhenCreatedTournament("BHA Open");

            serviceContext.SaveChanges();
            return tournament;
        }

        public static Tournament Part02BettersAddedToTournament(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            Tournament tournament = Part01CreateTournament(serviceContext);

            serviceContext.WhenCreatedUsers();
            serviceContext.WhenAddedBettersToTournament(tournament);

            serviceContext.SaveChanges();
            return tournament;
        }

        public static Round Part03AddDualTournamentRound(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            Tournament tournament = Part02BettersAddedToTournament(serviceContext);

            Round round = TournamentServiceContext.WhenAddedDualTournamentRoundToTournament(tournament, "Dual Tournament Round", 3);

            serviceContext.SaveChanges();
            return round;
        }

        public static List<DualTournamentGroup> Part04AddGroupsToDualTournamentRound(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            Round round = Part03AddDualTournamentRound(serviceContext);

            List<DualTournamentGroup> groups = new List<DualTournamentGroup>();
            groups.Add((DualTournamentGroup)TournamentServiceContext.WhenAddedGroupToRound(round));
            groups.Add((DualTournamentGroup)TournamentServiceContext.WhenAddedGroupToRound(round));

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part05AddedPlayersToDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            List<DualTournamentGroup> groups = Part04AddGroupsToDualTournamentRound(serviceContext);

            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Stålberto");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Bönis");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Guggelito");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[0], "Danneboi");

            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Bernard");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Papa Puert");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Klubbaxerino");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(groups[1], "Segmarken");

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part06StartDateTimeSetToMatchesInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            List<DualTournamentGroup> groups = Part05AddedPlayersToDualTournamentGroups(serviceContext);

            foreach (DualTournamentGroup group in groups)
            {
                for (int index = 0; index < group.Matches.Count; ++index)
                {
                    TournamentServiceContext.WhenSetStartDateTimeOnMatch(group.Matches[index], SystemTime.Now.AddDays(1).AddHours(1 + index));
                }
            }

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part07BetsPlacedOnMatchesInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            List<DualTournamentGroup> groups = Part06StartDateTimeSetToMatchesInDualTournamentGroups(serviceContext);

            foreach (DualTournamentGroup group in groups)
            {
                TournamentServiceContext.WhenBettersPlacesBetsOnAllMatchesInGroups(group);
            }

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part08CompleteFirstMatchInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            List<DualTournamentGroup> groups = Part07BetsPlacedOnMatchesInDualTournamentGroups(serviceContext);

            TournamentServiceContext.WhenPlayerScoreIncreased(groups.First().Matches.First().Player1, 2);

            serviceContext.SaveChanges();
            return groups;
        }

        public static List<DualTournamentGroup> Part09CompleteAllMatchesInDualTournamentGroups(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            List<DualTournamentGroup> groups = Part08CompleteFirstMatchInDualTournamentGroups(serviceContext);

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
                            TournamentServiceContext.WhenPlayerScoreIncreased(match.Player1, 2);
                        }
                        else
                        {
                            TournamentServiceContext.WhenPlayerScoreIncreased(match.Player2, 2);
                        }
                    }
                }
            }

            serviceContext.SaveChanges();
            return groups;
        }
    }
}
