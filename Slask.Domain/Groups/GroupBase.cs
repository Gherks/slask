using Slask.Domain.Bets;
using Slask.Domain.Rounds;
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

        public virtual PlayerReference AddNewPlayerReference(string name)
        {
            RoundBase firstRound = Round.Tournament.Rounds.First();

            bool groupDoesNotBelongToFirstRound = Round.Id != firstRound.Id;
            bool hasNotBegun = GetPlayState() != PlayState.NotBegun;

            if (groupDoesNotBelongToFirstRound || hasNotBegun)
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
                ConstructGroupLayout();

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

            PlayerReference foundPlayerReference = ParticipatingPlayers.FirstOrDefault(participant => participant.Name.ToLower() == name.ToLower());

            if (foundPlayerReference != null)
            {
                ParticipatingPlayers.Remove(foundPlayerReference);
                ConstructGroupLayout();
                return true;
            }

            // LOGG 
            return false;
        }

        public virtual bool RemovePlayerReference(PlayerReference playerReference)
        {
            if (GetPlayState() != PlayState.NotBegun)
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

            // LOGG 
            return false;
        }

        public virtual bool NewDateTimeIsValidWithGroupRules(Match match, DateTime dateTime)
        {
            return true;
        }

        public virtual void MatchScoreIncreased(Match match)
        {
        }

        public virtual void MatchScoreDecreased(Match match)
        {
        }

        public virtual bool SwitchPlayerReferences(Player player1, Player player2)
        {
            if (player1 == null || player2 == null)
            {
                // LOG Error: One of given players was null when trying to switch places
                return false;
            }

            bool bothPlayersHasPlayerReferences = player1.PlayerReference != null && player2.PlayerReference != null;
            bool noMatchHasStartedInGroup = player1.Match.Group.GetPlayState() == PlayState.NotBegun;

            if (bothPlayersHasPlayerReferences && noMatchHasStartedInGroup)
            {
                bool bothPlayersResidesInSameMatch = player1.Match.Id == player2.Match.Id;

                if (bothPlayersResidesInSameMatch)
                {
                    MakeSwitchOnPlayerReferencesInSameMatch(player1.Match);
                    return true;
                }

                bool bothPlayersResidesInSameGroup = player1.Match.Group.Id == player2.Match.Group.Id;

                if (bothPlayersResidesInSameGroup)
                {
                    MakeSwitchOnPlayerReferencesInDifferentMatch(player1, player2);
                    return true;
                }
            }

            return false;
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

        private void MakeSwitchOnPlayerReferencesInSameMatch(Match match)
        {
            PlayerReference firstPlayerReference = match.Player1.PlayerReference;
            PlayerReference secondPlayerReference = match.Player2.PlayerReference;

            match.SetPlayers(secondPlayerReference, firstPlayerReference);

            RemoveBettersMatchBetsOnMatches(new List<Match> { match });
        }

        private void MakeSwitchOnPlayerReferencesInDifferentMatch(Player player1, Player player2)
        {
            PlayerReference firstPlayerReference = player1.PlayerReference;
            PlayerReference secondPlayerReference = player2.PlayerReference;

            Match player1Match = player1.Match;
            Match player2Match = player2.Match;

            if (player1Match.Player1.Id == player1.Id)
            {
                player1Match.SetPlayers(secondPlayerReference, player1Match.Player2.PlayerReference);
            }
            else
            {
                player1Match.SetPlayers(player1Match.Player1.PlayerReference, secondPlayerReference);
            }

            if (player2Match.Player1.Id == player2.Id)
            {
                player2Match.SetPlayers(firstPlayerReference, player2Match.Player2.PlayerReference);
            }
            else
            {
                player2Match.SetPlayers(player2Match.Player1.PlayerReference, firstPlayerReference);
            }

            RemoveBettersMatchBetsOnMatches(new List<Match> { player1Match, player2Match });
        }

        private void RemoveBettersMatchBetsOnMatches(List<Match> matches)
        {
            foreach (Better better in Round.Tournament.Betters)
            {
                for (int betIndex = 0; betIndex < better.Bets.Count; ++betIndex)
                {
                    if (better.Bets[betIndex] is MatchBet matchBet)
                    {
                        foreach (Match match in matches)
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
}
