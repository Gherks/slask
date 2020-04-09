using Slask.Domain.Bets;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups
{
    public partial class GroupBase
    {
        protected GroupBase()
        {
            ParticipatingPlayers = new List<PlayerReference>();
            Matches = new List<Match>();
        }

        public Guid Id { get; protected set; }
        public List<PlayerReference> ParticipatingPlayers { get; protected set; }
        public List<Match> Matches { get; protected set; }
        public Guid RoundId { get; protected set; }
        public RoundBase Round { get; protected set; }

        public PlayerReference GetPlayerReference(Guid id)
        {
            return ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReference(string name)
        {
            return ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == name);
        }

        public PlayState GetPlayState()
        {
            if (Matches.Count == 0)
            {
                return PlayState.NotBegun;
            }

            bool hasNoMatches = Matches.Count == 0;
            bool hasNotStarted = Matches.First().GetPlayState() == PlayState.NotBegun;

            if (hasNoMatches || hasNotStarted)
            {
                return PlayState.NotBegun;
            }

            bool lastMatchIsFinished = Matches.Last().GetWinningPlayer() != null;

            return lastMatchIsFinished ? PlayState.IsFinished : PlayState.IsPlaying;
        }

        public virtual PlayerReference AddNewPlayerReference(string name)
        {
            RoundBase firstRound = Round.Tournament.Rounds.First();
            bool groupDoesNotBelongToFirstRound = Round.Id != firstRound.Id;

            if (groupDoesNotBelongToFirstRound)
            {
                // LOG Error: Trying to add brand new player reference to group not within first round.
                return null;
            }

            PlayerReference playerReference = GetPlayerReferenceFromTournamentRegistryByName(name);

            if (playerReference == null)
            {
                playerReference = PlayerReference.Create(name, Round.Tournament);
                AddPlayerReference(playerReference);

                return playerReference;
            }

            // LOG Error: Player with given name already exists in tournament
            return null;
        }

        public bool AddPlayerReference(PlayerReference playerReference)
        {
            bool hasBegun = GetPlayState() != PlayState.NotBegun;

            if (hasBegun)
            {
                // LOG Error: Cannot add new player references to group that has already begun
                return false;
            }

            bool isParticipatingPlayer = ParticipatingPlayers.Where(participant => participant.Name.ToLower() == playerReference.Name.ToLower()).Any();

            if (isParticipatingPlayer)
            {
                // LOGG Error: Cannot add player reference to group twice
                return false;
            }

            ParticipatingPlayers.Add(playerReference);
            ConstructGroupLayout();
            return true;
        }

        public bool RemovePlayerReference(string name)
        {
            if (!ValidateRemoval())
            {
                return false;
            }

            PlayerReference foundPlayerReference = ParticipatingPlayers.FirstOrDefault(participant => participant.Name.ToLower() == name.ToLower());

            if (foundPlayerReference != null)
            {
                ParticipatingPlayers.Remove(foundPlayerReference);
                ConstructGroupLayout();
                return true;
            }

            // LOGG Error: Could not remove player from group; player with provided name does not exist: {name}
            return false;
        }

        public virtual bool RemovePlayerReference(PlayerReference playerReference)
        {
            if (!ValidateRemoval())
            {
                return false;
            }

            PlayerReference foundPlayerReference = ParticipatingPlayers.FirstOrDefault(participant => participant.Id == playerReference.Id);

            if (foundPlayerReference != null)
            {
                ParticipatingPlayers.Remove(foundPlayerReference);
                ConstructGroupLayout();
                return true;
            }

            // LOGG Error: Could not remove player from group; provided player reference with name "{name}" does not exist in group
            return false;
        }

        public virtual bool NewDateTimeIsValid(Match match, DateTime dateTime)
        {
            return true;
        }

        public virtual void OnMatchScoreIncreased(Match match)
        {
        }

        public virtual void OnMatchScoreDecreased(Match match)
        {
        }

        protected void ChangeMatchAmountTo(int amount)
        {
            Matches.Clear();

            while (Matches.Count < amount)
            {
                Matches.Add(Match.Create(this));
            }
        }

        protected virtual void ConstructGroupLayout()
        {
            throw new NotImplementedException();
        }

        private PlayerReference GetPlayerReferenceFromTournamentRegistryByName(string name)
        {
            return Round.Tournament.GetPlayerReferenceByPlayerName(name);
        }

        // CREATE TEST for these two new checks
        private bool ValidateRemoval()
        {
            if (!Round.IsFirstRound())
            {
                // LOGG Error: Cannot remove player reference from this group since this group does not belong to the first round.
                return false;
            }

            Match firstMatch = Round.GetFirstMatch();
            bool firstMatchHasBegun = firstMatch != null && firstMatch.GetPlayState() != PlayState.NotBegun;

            if (firstMatchHasBegun)
            {
                // LOGG Error: Cannot remove player reference from group within round that has already begun.
                return false;
            }

            return true;
        }

        internal void RemoveAllMatchBetsOnMatch(Match match)
        {
            foreach (Better better in Round.Tournament.Betters)
            {
                for (int betIndex = 0; betIndex < better.Bets.Count; ++betIndex)
                {
                    if (better.Bets[betIndex] is MatchBet matchBet)
                    {
                        if (matchBet.Match.Id == match.Id)
                        {
                            better.Bets.Remove(better.Bets[betIndex--]);
                        }
                    }
                }
            }
        }
    }
}
