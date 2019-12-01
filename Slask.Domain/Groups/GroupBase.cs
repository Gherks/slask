using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
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
            if(Matches.Count == 0)
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

        public List<PlayerReference> GetAdvancingPlayers()
        {
            if (GetPlayState() == PlayState.IsFinished)
            {
                List<PlayerStandingEntry> playerStandings = CalculatePlayerStandings();

                playerStandings = playerStandings.OrderByDescending(player => player.Wins).ToList();

                return FilterAdvancingPlayers(ref playerStandings);
            }

            return new List<PlayerReference>();
        }

        public virtual PlayerReference AddPlayerReference(string name)
        {
            if (GetPlayState() != PlayState.NotBegun)
            {
                return null;
            }

            bool isParticipatingPlayer = ParticipatingPlayers.Where(participant => participant.Name.ToLower() == name.ToLower()).Any();

            if (!isParticipatingPlayer)
            {
                PlayerReference playerReference = GetPlayerReferenceFromTournamentRegistryByName(name);

                if (playerReference == null)
                {
                    playerReference = PlayerReference.Create(name, Round.Tournament);
                }

                ParticipatingPlayers.Add(playerReference);
                OnParticipantAdded(playerReference);

                return playerReference;
            }
            else
            {
                // LOGG 
                return null;
            }
        }

        public virtual bool RemovePlayerReference(string name)
        {
            if (GetPlayState() != PlayState.NotBegun)
            {
                return false;
            }

            PlayerReference playerReference = ParticipatingPlayers.Where(participant => participant.Name.ToLower() == name.ToLower()).FirstOrDefault();

            return RemovePlayerReference(playerReference);
        }

        public virtual bool RemovePlayerReference(PlayerReference playerReference)
        {
            if (GetPlayState() != PlayState.NotBegun)
            {
                return false;
            }

            if (playerReference != null)
            {
                ParticipatingPlayers.Remove(playerReference);
                OnParticipantRemoved(playerReference);
                return true;
            }
            else
            {
                // LOGG 
                return false;
            }
        }

        public virtual void Clear()
        {
            ParticipatingPlayers.Clear();
            Matches.Clear();
        }

        public virtual void MatchScoreIncreased(Match match)
        {
        }

        public virtual void MatchScoreDecreased(Match match)
        {
        }

        protected void ChangeMatchAmountTo(int amount)
        {
            while (Matches.Count < amount)
            {
                Matches.Add(Match.Create(this));
            }

            while (Matches.Count > amount)
            {
                Matches.RemoveAt(0);
            }
        }

        protected virtual void OnParticipantAdded(PlayerReference playerReference)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnParticipantRemoved(PlayerReference playerReference)
        {
            throw new NotImplementedException();
        }

        private PlayerReference GetPlayerReferenceFromTournamentRegistryByName(string name)
        {
            return Round.Tournament.GetPlayerReferenceByPlayerName(name);
        }

        private List<PlayerStandingEntry> CalculatePlayerStandings()
        {
            List<PlayerStandingEntry> playerStandings = new List<PlayerStandingEntry>();

            foreach (Match match in Matches)
            {
                PlayerReference winner = match.GetWinningPlayer().PlayerReference;
                PlayerStandingEntry playerStandingEntry = playerStandings.Find(player => player.PlayerReference.Name == winner.Name);

                if (playerStandingEntry == null)
                {
                    playerStandings.Add(PlayerStandingEntry.Create(winner));
                }
                else
                {
                    playerStandingEntry.AddWin();
                }
            }

            return playerStandings;
        }

        private List<PlayerReference> FilterAdvancingPlayers(ref List<PlayerStandingEntry> playerStandings)
        {
            List<PlayerReference> advancingPlayers = new List<PlayerReference>();

            for (int index = 0; index < Round.AdvancingPerGroupAmount; ++index)
            {
                advancingPlayers.Add(playerStandings[index].PlayerReference);
            }

            return advancingPlayers;
        }
    }
}
