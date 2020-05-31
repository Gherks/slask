using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Rounds.RoundUtilities;
using Slask.Domain.Utilities;
using Slask.Domain.Utilities.StandingsSolvers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class Tournament
    {
        private Tournament()
        {
            Rounds = new List<RoundBase>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            TournamentIssueReporter = new TournamentIssueReporter();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<RoundBase> Rounds { get; private set; }
        public List<Better> Betters { get; private set; }
        public List<Settings> Settings { get; private set; }

        // Ignored by SlaskContext
        public TournamentIssueReporter TournamentIssueReporter { get; private set; }

        public static Tournament Create(string name)
        {
            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public void RenameTo(string name)
        {
            Name = name.Trim();
        }

        public BracketRound AddBracketRound()
        {
            BracketRound round = BracketRound.Create(this);

            if (round == null)
            {
                return null;
            }

            bool roundWasIntegrated = IntegrateRoundToTournament(round);

            if (roundWasIntegrated)
            {
                return round;
            }

            return null;
        }

        public DualTournamentRound AddDualTournamentRound()
        {
            DualTournamentRound round = DualTournamentRound.Create(this);

            if (round == null)
            {
                return null;
            }

            bool roundWasIntegrated = IntegrateRoundToTournament(round);

            if (roundWasIntegrated)
            {
                return round;
            }

            return null;
        }

        public RoundRobinRound AddRoundRobinRound()
        {
            RoundRobinRound round = RoundRobinRound.Create(this);

            if (round == null)
            {
                return null;
            }

            bool roundWasIntegrated = IntegrateRoundToTournament(round);

            if (roundWasIntegrated)
            {
                return round;
            }

            return null;
        }

        public bool RemoveRound(RoundBase round)
        {
            bool soughtRoundIsValid = round != null;
            bool tournamentHasNotBegun = GetFirstRound().GetPlayState() == PlayState.NotBegun;

            if (soughtRoundIsValid && tournamentHasNotBegun)
            {
                return ManageRoundRemoval(round);
            }

            return false;
        }

        public Better AddBetter(User user)
        {
            bool betterAlreadyExists = GetBetterByName(user.Name) != null;

            if (betterAlreadyExists)
            {
                return null;
            }

            Betters.Add(Better.Create(user, this));

            return Betters.Last();
        }

        public void FindIssues()
        {
            TournamentIssueReporter.Clear();

            foreach (RoundBase round in Rounds)
            {
                RoundIssueFinder.FindIssues(round);
            }
        }

        public bool HasIssues()
        {
            return TournamentIssueReporter.Issues.Count > 0;
        }

        public RoundBase GetRoundByRoundId(Guid id)
        {
            return Rounds.FirstOrDefault(round => round.Id == id);
        }

        public RoundBase GetRoundByRoundName(string name)
        {
            return Rounds.FirstOrDefault(round => round.Name.ToLower() == name.ToLower());
        }

        public RoundBase GetFirstRound()
        {
            bool hasNoRounds = Rounds.Count == 0;

            if (hasNoRounds)
            {
                return null;
            }

            return Rounds.First();
        }

        public RoundBase GetLastRound()
        {
            bool hasNoRounds = Rounds.Count == 0;

            if (hasNoRounds)
            {
                return null;
            }

            return Rounds.Last();
        }

        public PlayerReference GetPlayerReferenceByPlayerId(Guid id)
        {
            return GetPlayerReferences().FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReferenceByPlayerName(string name)
        {
            return GetPlayerReferences().FirstOrDefault(playerReference => playerReference.Name == name);
        }

        public Better GetBetterById(Guid id)
        {
            return Betters.FirstOrDefault(better => better.Id == id);
        }

        public Better GetBetterByName(string name)
        {
            return Betters.FirstOrDefault(better => better.User.Name.ToLower() == name.ToLower());
        }

        public List<PlayerReference> GetPlayerReferences()
        {
            bool tournamentHasNoRounds = Rounds.Count == 0;

            if (tournamentHasNoRounds)
            {
                return new List<PlayerReference>();
            }

            return Rounds.First().PlayerReferences;
        }

        public PlayState GetPlayState()
        {
            bool hasNotBegun = Rounds.First().GetPlayState() == PlayState.NotBegun;

            if (hasNotBegun)
            {
                return PlayState.NotBegun;
            }

            bool lastRoundIsFinished = Rounds.Last().GetPlayState() == PlayState.Finished;

            return lastRoundIsFinished ? PlayState.Finished : PlayState.Ongoing;
        }

        public List<StandingsEntry<Better>> GetBetterStandings()
        {
            BetterStandingsSolver betterStandingsSolver = new BetterStandingsSolver();
            return betterStandingsSolver.FetchFrom(this);
        }

        private bool IntegrateRoundToTournament(RoundBase round)
        {
            if (CanAddNewRound())
            {
                Rounds.Add(round);
                round.Construct();
                FindIssues();

                return true;
            }

            return false;
        }

        private bool CanAddNewRound()
        {
            bool tournamentHasNoRounds = Rounds.Count == 0;

            if (tournamentHasNoRounds)
            {
                return true;
            }

            bool tournamentHasNotBegun = Rounds.First().GetPlayState() == PlayState.NotBegun;

            return tournamentHasNotBegun;
        }

        private bool ManageRoundRemoval(RoundBase round)
        {
            bool containsSoughtRound = Rounds.Contains(round);

            if (containsSoughtRound)
            {
                bool soughtRoundIsTheFirstOne = GetFirstRound().Id == round.Id;
                List<PlayerReference> playerReferences = new List<PlayerReference>();

                if (soughtRoundIsTheFirstOne)
                {
                    playerReferences = round.PlayerReferences;
                }

                bool removalSuccessful = Rounds.Remove(round);

                if (removalSuccessful)
                {
                    bool stillContainsRounds = Rounds.Count > 0;

                    if (stillContainsRounds)
                    {
                        RoundBase firstRound = GetFirstRound();

                        foreach (PlayerReference playerReference in playerReferences)
                        {
                            firstRound.RegisterPlayerReference(playerReference.Name);
                        }

                        firstRound.Construct();
                    }

                    FindIssues();
                }
            }

            return false;
        }
    }
}
