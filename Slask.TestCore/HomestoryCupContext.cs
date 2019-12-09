using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
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
    public static class HomestoryCupSetup
    {
        public static Tournament Part01CreateTournament(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            Tournament tournament = serviceContext.WhenCreatedTournament("Homestory Cup");

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

        public static RoundBase Part03AddRoundRobinRound(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            Tournament tournament = Part02BettersAddedToTournament(serviceContext);

            RoundBase round = TournamentServiceContext.WhenAddedRoundRobinRoundToTournament(tournament, "Round Robin Round", 3, 4);

            serviceContext.SaveChanges();
            return round;
        }

        public static RoundRobinGroup Part04AddedGroupToRoundRobinRound(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundBase round = Part03AddRoundRobinRound(serviceContext);

            RoundRobinGroup group = (RoundRobinGroup)TournamentServiceContext.WhenAddedGroupToRound(round);

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part05AddedPlayersToRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundRobinGroup group = Part04AddedGroupToRoundRobinRound(serviceContext);

            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Maru");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Stork");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Taeja");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Rain");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Bomber");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "FanTaSy");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Stephano");
            TournamentServiceContext.WhenAddedPlayerReferenceToGroup(group, "Thorzain");

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part06StartDateTimeSetToMatchesInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundRobinGroup group = Part05AddedPlayersToRoundRobinGroup(serviceContext);

            for (int index = 0; index < group.Matches.Count; ++index)
            {
                TournamentServiceContext.WhenSetStartDateTimeOnMatch(group.Matches[index], SystemTime.Now.AddHours(1 + index));
            }

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part07BetsPlacedOnMatchesInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundRobinGroup group = Part06StartDateTimeSetToMatchesInRoundRobinGroup(serviceContext);

            TournamentServiceContext.WhenBettersPlacesBetsOnAllMatchesInGroups(group);

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part08CompleteFirstMatchInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundRobinGroup group = Part07BetsPlacedOnMatchesInRoundRobinGroup(serviceContext);

            TournamentServiceContext.WhenPlayerScoreIncreased(group.Matches.First().Player1, 2);

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundRobinGroup Part09CompleteAllMatchesInRoundRobinGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundRobinGroup group = Part08CompleteFirstMatchInRoundRobinGroup(serviceContext);

            Random random = new Random(133742069);

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

            serviceContext.SaveChanges();
            return group;
        }

        public static RoundBase Part10AddBracketRound(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundRobinGroup group = Part09CompleteAllMatchesInRoundRobinGroup(serviceContext);
            Tournament tournament = group.Round.Tournament;

            RoundBase round = TournamentServiceContext.WhenAddedBracketRoundToTournament(tournament, "Bracket Round", 5);

            serviceContext.SaveChanges();
            return round;
        }

        public static BracketGroup Part11AddGroupToBracketRound(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            RoundBase round = Part10AddBracketRound(serviceContext);

            BracketGroup group = (BracketGroup)TournamentServiceContext.WhenAddedGroupToRound(round);

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part12AddWinningPlayersToBracketGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            BracketGroup group = Part11AddGroupToBracketRound(serviceContext);
            RoundBase previousRound = group.Round.GetPreviousRound();
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

        public static BracketGroup Part13StartDateTimeSetToMatchesInBracketGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            BracketGroup group = Part12AddWinningPlayersToBracketGroup(serviceContext);

            for (int index = 0; index < group.Matches.Count; ++index)
            {
                TournamentServiceContext.WhenSetStartDateTimeOnMatch(group.Matches[index], SystemTime.Now.AddDays(1).AddHours(1 + index));
            }

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part14BetsPlacedOnMatchesInBracketGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            BracketGroup group = Part13StartDateTimeSetToMatchesInBracketGroup(serviceContext);

            TournamentServiceContext.WhenBettersPlacesBetsOnAllMatchesInGroups(group);

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part15CompleteFirstMatchInBracketGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            BracketGroup group = Part14BetsPlacedOnMatchesInBracketGroup(serviceContext);

            TournamentServiceContext.WhenPlayerScoreIncreased(group.Matches.First().Player1, 3);

            serviceContext.SaveChanges();
            return group;
        }

        public static BracketGroup Part16CompleteAllMatchesInBracketGroup(TournamentServiceContext serviceContext)
        {
            if (serviceContext == null)
            {
                throw new ArgumentNullException(nameof(serviceContext));
            }

            BracketGroup group = Part15CompleteFirstMatchInBracketGroup(serviceContext);

            Random random = new Random(133742069);

            foreach (Match match in group.Matches)
            {
                if (match.GetPlayState() != PlayState.IsFinished)
                {
                    bool increasePlayer1Score = random.Next(2) == 0;

                    if (increasePlayer1Score)
                    {
                        TournamentServiceContext.WhenPlayerScoreIncreased(match.Player1, 3);
                    }
                    else
                    {
                        TournamentServiceContext.WhenPlayerScoreIncreased(match.Player2, 3);
                    }
                }
            }

            serviceContext.SaveChanges();
            return group;
        }
    }
}
