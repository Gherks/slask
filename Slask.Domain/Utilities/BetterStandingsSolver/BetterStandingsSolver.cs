using Slask.Domain.Bets;
using Slask.Domain.Groups;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Utilities
{
    public static class BetterStandingsSolver
    {
        public static List<BetterStandingsEntry> FetchFrom(Tournament tournament)
        {
            List<BetterStandingsEntry> betterStandings = CreateStandingsList(tournament);

            AggregatePointsForEntries(betterStandings);

            return betterStandings.OrderByDescending(player => player.Points).ToList();
        }

        private static List<BetterStandingsEntry> CreateStandingsList(Tournament tournament)
        {
            List<BetterStandingsEntry> betterStandings = new List<BetterStandingsEntry>();

            foreach (Better better in tournament.Betters)
            {
                betterStandings.Add(BetterStandingsEntry.Create(better));
            }

            return betterStandings;
        }

        private static void AggregatePointsForEntries(List<BetterStandingsEntry> betterStandings)
        {
            foreach (BetterStandingsEntry entry in betterStandings)
            {
                foreach (BetInterface bet in entry.Better.Bets)
                {
                    if (bet.IsWon())
                    {
                        entry.AddPoint();
                    }
                }
            }
        }
    }
}
