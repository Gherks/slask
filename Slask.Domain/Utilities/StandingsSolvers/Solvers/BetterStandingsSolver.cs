using Slask.Domain.Bets;
using System.Collections.Generic;

namespace Slask.Domain.Utilities.StandingsSolvers
{
    public class BetterStandingsSolver : StandingsSolverBase<Tournament, Better>
    {
        protected override List<StandingsEntry<Better>> CreateStandingsList(Tournament tournament)
        {
            List<StandingsEntry<Better>> betterStandings = new List<StandingsEntry<Better>>();

            foreach (Better better in tournament.Betters)
            {
                betterStandings.Add(StandingsEntry<Better>.Create(better));
            }

            return betterStandings;
        }

        protected override void AggregatePointsForStandingEntries(Tournament tournament, List<StandingsEntry<Better>> playerStandings)
        {
            foreach (StandingsEntry<Better> entry in playerStandings)
            {
                foreach (BetInterface bet in entry.Object.Bets)
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
