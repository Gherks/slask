using Slask.Common;
using Slask.Domain.Rounds;
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
            PlayerReferences = new List<PlayerReference>();
            Betters = new List<Better>();
            Settings = new List<Settings>();
            MiscBetCatalogue = new List<MiscBetCatalogue>();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public List<RoundBase> Rounds { get; private set; }
        public List<PlayerReference> PlayerReferences { get; private set; }
        public List<Better> Betters { get; private set; }
        public List<Settings> Settings { get; private set; }
        public List<MiscBetCatalogue> MiscBetCatalogue { get; private set; }

        public static Tournament Create(string name)
        {
            return new Tournament
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }

        public void ChangeName(string name)
        {
            Name = name;
        }

        public RoundBase AddBracketRound(string name, int bestOf)
        {
            RoundBase round = BracketRound.Create(name, bestOf, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            return Rounds.Last();
        }

        public RoundBase AddDualTournamentRound(string name, int bestOf)
        {
            RoundBase round = DualTournamentRound.Create(name, bestOf, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            return Rounds.Last();
        }

        public RoundBase AddRoundRobinRound(string name, int bestOf, int advanceAmount)
        {
            RoundBase round = RoundRobinRound.Create(name, bestOf, advanceAmount, this);

            if (round == null)
            {
                return null;
            }

            Rounds.Add(round);
            return Rounds.Last();
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

        public RoundBase GetRoundByRoundId(Guid id)
        {
            return Rounds.FirstOrDefault(round => round.Id == id);
        }

        public RoundBase GetRoundByRoundName(string name)
        {
            return Rounds.FirstOrDefault(round => round.Name.ToLower() == name.ToLower());
        }

        public PlayerReference GetPlayerReferenceByPlayerId(Guid id)
        {
            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReferenceByPlayerName(string name)
        {
            return PlayerReferences.FirstOrDefault(playerReference => playerReference.Name.ToLower() == name.ToLower());
        }

        public Better GetBetterById(Guid id)
        {
            return Betters.FirstOrDefault(better => better.Id == id);
        }

        public Better GetBetterByName(string name)
        {
            return Betters.FirstOrDefault(better => better.User.Name.ToLower() == name.ToLower());
        }

        public bool RemoveDanglingPlayerReference(PlayerReference targetedPlayerReference)
        {
            if (targetedPlayerReference == null)
            {
                throw new ArgumentNullException(nameof(targetedPlayerReference));
            }

            bool playerReferenceNotInPlayerReferencePool = PlayerReferences.Any(playerReference => playerReference.Id == targetedPlayerReference.Id);

            if (playerReferenceNotInPlayerReferencePool)
            {
                bool shouldBeRemovedFromPlayerReferencePool = !PlayerReferenceInUseByAnyGroup(targetedPlayerReference);

                if (shouldBeRemovedFromPlayerReferencePool)
                {
                    PlayerReferences.Remove(targetedPlayerReference);
                }

                return shouldBeRemovedFromPlayerReferencePool;
            }
            else
            {
                // LOG Error: Player reference has already been removed. Tournament player reference pool was out of sync with the players residing in groups.
            }

            return false;
        }

        private bool PlayerReferenceInUseByAnyGroup(PlayerReference targetedPlayerReference)
        {
            foreach (RoundBase round in Rounds)
            {
                foreach (GroupBase group in round.Groups)
                {
                    bool groupHasPlayerReference = group.ParticipatingPlayers.Any(playerReference => playerReference.Id == targetedPlayerReference.Id);

                    if (groupHasPlayerReference)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
