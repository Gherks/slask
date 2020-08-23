using Slask.Domain.Bets;
using Slask.Domain.Groups;
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
            _created = DateTime.Now;

            Id = Guid.NewGuid();
            Rounds = new List<RoundBase>();
            PlayerReferences = new List<PlayerReference>();
            Betters = new List<Better>();
            TournamentIssueReporter = new TournamentIssueReporter();
        }

        private readonly DateTime _created;

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public DateTime Created { get { return _created; } private set { } }
        public List<RoundBase> Rounds { get; private set; }
        public List<PlayerReference> PlayerReferences { get; private set; }
        public List<Better> Betters { get; private set; }
        public TournamentIssueReporter TournamentIssueReporter { get; private set; }

        public static Tournament Create(string name)
        {
            return new Tournament
            {
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
            bool tournamentHasNotBegun = GetFirstRound().GetPlayState() == PlayStateEnum.NotBegun;

            if (soughtRoundIsValid && tournamentHasNotBegun)
            {
                return ManageRoundRemoval(round);
            }

            return false;
        }

        public PlayerReference RegisterPlayerReference(string name)
        {
            bool tournamentHasNotBegun = GetPlayState() == PlayStateEnum.NotBegun;
            bool nameIsNotRegistered = !PlayerReferences.Any(playerReference => playerReference.Name == name);
            bool nameIsNotEmpty = name.Length != 0;

            if (tournamentHasNotBegun && nameIsNotRegistered && nameIsNotEmpty)
            {
                PlayerReferences.Add(PlayerReference.Create(name, this));
                OnPlayerReferencesChanged();

                return PlayerReferences.Last();
            }

            return null;
        }

        public bool ExcludePlayerReference(string name)
        {
            bool tournamentHasNotBegun = GetPlayState() == PlayStateEnum.NotBegun;
            bool nameIsNotEmpty = name.Length != 0;

            if (tournamentHasNotBegun && nameIsNotEmpty)
            {
                PlayerReference playerReference = PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == name);
                bool playerReferenceExistInRound = playerReference != null;

                if (playerReferenceExistInRound)
                {
                    playerReference.MarkForDeletion();
                    PlayerReferences.Remove(playerReference);

                    OnPlayerReferencesChanged();
                    return true;
                }
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

        public bool RemoveBetter(Better better)
        {
            bool betterWasRemoved = Betters.Remove(better);

            if (betterWasRemoved)
            {
                better.MarkForDeletion();
            }

            return betterWasRemoved;
        }

        public void ResetObjectStatesOnAllEntities()
        {
            foreach (PlayerReference playerReference in PlayerReferences)
            {
                playerReference.ResetObjectState();
            }

            foreach (Better better in Betters)
            {
                better.ResetObjectState();

                foreach (BetBase bet in better.Bets)
                {
                    bet.ResetObjectState();
                }
            }

            foreach (RoundBase round in Rounds)
            {
                round.ResetObjectState();

                foreach (GroupBase group in round.Groups)
                {
                    group.ResetObjectState();

                    foreach (Match match in group.Matches)
                    {
                        match.ResetObjectState();

                        if (match.Player1 != null)
                        {
                            match.Player1.ResetObjectState();
                        }

                        if (match.Player2 != null)
                        {
                            match.Player2.ResetObjectState();
                        }
                    }
                }
            }
        }

        public void SortEntities()
        {
            Rounds = Rounds.OrderBy(round => round.SortOrder).ToList();

            foreach (RoundBase round in Rounds)
            {
                round.SortEntities();
            }
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

        public GroupBase GetGroupByGroupId(Guid id)
        {
            foreach (RoundBase round in Rounds)
            {
                GroupBase group = round.Groups.FirstOrDefault(group => group.Id == id);

                if (group != null)
                {
                    return group;
                }
            }

            return null;
        }

        public GroupBase GetGroupByGroupName(string name)
        {
            foreach (RoundBase round in Rounds)
            {
                GroupBase group = round.Groups.FirstOrDefault(group => group.Name.ToLower() == name.ToLower());

                if (group != null)
                {
                    return group;
                }
            }

            return null;
        }

        public Match GetMatchByMatchId(Guid id)
        {
            foreach (RoundBase round in Rounds)
            {
                foreach (GroupBase group in round.Groups)
                {
                    Match match = group.Matches.FirstOrDefault(match => match.Id == id);

                    if (match != null)
                    {
                        return match;
                    }
                }
            }

            return null;
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

        public PlayerReference GetPlayerReferenceById(Guid id)
        {
            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReferenceByName(string name)
        {
            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Name == name);
        }

        public List<PlayerReference> GetPlayerReferencesByIds(List<Guid> playerReferenceIds)
        {
            List<PlayerReference> playerReferences = new List<PlayerReference>();

            foreach (Guid playerReferenceId in playerReferenceIds)
            {
                PlayerReference playerReference = GetPlayerReferenceById(playerReferenceId);

                if (playerReference != null)
                {
                    playerReferences.Add(playerReference);
                }
            }

            return playerReferences;
        }

        public List<PlayerReference> GetPlayerReferencesByNames(List<string> playerReferenceNames)
        {
            List<PlayerReference> playerReferences = new List<PlayerReference>();

            foreach (string playerReferenceName in playerReferenceNames)
            {
                PlayerReference playerReference = GetPlayerReferenceByName(playerReferenceName);

                if (playerReference != null)
                {
                    playerReferences.Add(playerReference);
                }
            }

            return playerReferences;
        }

        public Better GetBetterById(Guid id)
        {
            return Betters.FirstOrDefault(better => better.Id == id);
        }

        public Better GetBetterByName(string name)
        {
            return Betters.FirstOrDefault(better => better.User.Name.ToLower() == name.ToLower());
        }

        public PlayStateEnum GetPlayState()
        {
            bool hasNotBegun = Rounds.First().GetPlayState() == PlayStateEnum.NotBegun;

            if (hasNotBegun)
            {
                return PlayStateEnum.NotBegun;
            }

            bool lastRoundIsFinished = Rounds.Last().GetPlayState() == PlayStateEnum.Finished;

            return lastRoundIsFinished ? PlayStateEnum.Finished : PlayStateEnum.Ongoing;
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

            bool tournamentHasNotBegun = Rounds.First().GetPlayState() == PlayStateEnum.NotBegun;

            return tournamentHasNotBegun;
        }

        private bool ManageRoundRemoval(RoundBase round)
        {
            bool containsSoughtRound = Rounds.Contains(round);

            if (containsSoughtRound)
            {
                bool removalSuccessful = Rounds.Remove(round);

                if (removalSuccessful)
                {
                    round.MarkForDeletion();

                    bool stillContainsRounds = Rounds.Count > 0;
                    if (stillContainsRounds)
                    {
                        GetFirstRound().Construct();
                    }

                    FindIssues();
                    return true;
                }
            }

            return false;
        }

        private void OnPlayerReferencesChanged()
        {
            RoundBase firstRound = GetFirstRound();
            firstRound.Construct();
            firstRound.FillGroupsWithPlayerReferences();

            FindIssues();
        }
    }
}
