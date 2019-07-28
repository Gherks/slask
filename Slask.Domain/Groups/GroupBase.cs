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

        public Guid Id { get; private set; }
        public bool IsReady { get; private set; }
        public List<PlayerReference> ParticipatingPlayers { get; private set; }
        public List<Match> Matches { get; private set; }
        public Guid RoundId { get; private set; }
        public Round Round { get; private set; }

        public static GroupBase Create(Round round)
        {
            if(round == null)
            {
                return null;
            }

            return new GroupBase
            {
                Id = Guid.NewGuid(),
                IsReady = false,
                Round = round
            };
        }

        public void AddPlayerReference(string name)
        {
            PlayerReference playerReference = ParticipatingPlayers.Where(reference => reference.Name == name).FirstOrDefault();

            if (playerReference == null)
            {
                playerReference = GetPlayerReferenceFromTournamentRegistryByName(name);

                if(playerReference == null)
                {
                    playerReference = RegisterPlayerToTournament(name);
                }

                ParticipatingPlayers.Add(playerReference);
                UpdateMatchLayout();
            }
        }

        public PlayerReference GetPlayerReference(Guid id)
        {
            return ParticipatingPlayers.Where(playerReference => playerReference.Id == id).FirstOrDefault();
        }

        public PlayerReference GetPlayerReference(string name)
        {
            return ParticipatingPlayers.Where(playerReference => playerReference.Name == name).FirstOrDefault();
        }

        private PlayerReference GetPlayerReferenceFromTournamentRegistryByName(string name)
        {
            return Round.Tournament.GetPlayerReferenceByPlayerName(name);
        }

        private PlayerReference RegisterPlayerToTournament(string name)
        {
            Tournament tournament = Round.Tournament;

            PlayerReference playerReference = PlayerReference.Create(name, tournament);
            tournament.PlayerReferences.Add(playerReference);

            return playerReference;
        }

        public void SetIsReady(bool isReady)
        {
            throw new NotImplementedException();
        }

        private PlayerReference GetPlayerReferenceWithName(string name)
        {
            return ParticipatingPlayers.Where(reference => reference.Name == name).FirstOrDefault();
        }

        public virtual void MatchScoreChanged()
        {
        }

        protected virtual void UpdateMatchLayout()
        {
            BalanceMatchesByPlayerPairings();
        }

        protected void BalanceMatchesByPlayerPairings()
        {
            int playerPairingAmount = (int)Math.Ceiling(ParticipatingPlayers.Count / 2.0);

            while (Matches.Count < playerPairingAmount)
            {
                Matches.Add(Match.Create());
            }

            if (Matches.Count > playerPairingAmount)
            {
                int removeAmount = Matches.Count - playerPairingAmount;
                Matches.RemoveRange(Matches.Count - removeAmount, removeAmount);
            }
        }
    }
}
