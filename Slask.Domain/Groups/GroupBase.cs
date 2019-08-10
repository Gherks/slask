using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class GroupBase
    {
        protected GroupBase()
        {
            ParticipatingPlayers = new List<PlayerReference>();
            Matches = new List<Match>();
        }

        public Guid Id { get; protected set; }
        public bool IsReady { get; protected set; }
        public List<PlayerReference> ParticipatingPlayers { get; protected set; }
        public List<Match> Matches { get; protected set; }
        public Guid RoundId { get; protected set; }
        public Round Round { get; protected set; }

        public PlayerReference GetPlayerReference(Guid id)
        {
            return ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Id == id);
        }

        public PlayerReference GetPlayerReference(string name)
        {
            return ParticipatingPlayers.FirstOrDefault(playerReference => playerReference.Name == name);
        }

        private PlayerReference GetPlayerReferenceFromTournamentRegistryByName(string name)
        {
            return Round.Tournament.GetPlayerReferenceByPlayerName(name);
        }

        public void SetIsReady(bool isReady)
        {
            throw new NotImplementedException();
        }

        private PlayerReference GetPlayerReferenceWithName(string name)
        {
            return ParticipatingPlayers.FirstOrDefault(reference => reference.Name == name);
        }

        public virtual void AddPlayerReference(string name)
        {
            PlayerReference playerReference = ParticipatingPlayers.FirstOrDefault(reference => reference.Name == name);

            if (playerReference == null)
            {
                playerReference = GetPlayerReferenceFromTournamentRegistryByName(name);

                if (playerReference == null)
                {
                    playerReference = PlayerReference.Create(name, Round.Tournament);
                }

                ParticipatingPlayers.Add(playerReference);
                UpdateMatchLayout();
            }
        }

        public virtual void Clear()
        {
        }

        public virtual void MatchScoreChanged(Match match)
        {
        }

        protected virtual void UpdateMatchLayout()
        {
        }
    }
}
