using Slask.Common;
using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Match
    {
        private Match()
        {
            Players = new List<Player>();
            Players.Add(Player.Create(this));
            Players.Add(Player.Create(this));
        }

        public Guid Id { get; private set; }
        public DateTime StartDateTime { get; private set; }
        private List<Player> Players { get; set; }
        public Guid GroupId { get; private set; }
        public GroupBase Group { get; private set; }

        // Ignored by SlaskContext
        public Player Player1 { get { return Players[0]; } private set { } }
        public Player Player2 { get { return Players[1]; } private set { } }

        public static Match Create(GroupBase group)
        {
            if (group == null)
            {
                // LOGG
                return null;
            }

            Match match = new Match()
            {
                Id = Guid.NewGuid(),
                GroupId = group.Id,
                Group = group,

            };

            if (group.Matches.Count == 0)
            {
                match.StartDateTime = SystemTime.Now.AddDays(7);
            }
            else
            {
                Match previousMatch = group.Matches[group.Matches.Count - 1];
                match.StartDateTime = previousMatch.StartDateTime.AddHours(1);
            }

            return match;
        }

        public bool IsReady()
        {
            return Player1.PlayerReference != null && Player2.PlayerReference != null;
        }

        // Needs to restricted to GroupBase?
        public bool AddPlayerReference(PlayerReference playerReference)
        {
            if (playerReference == null)
            {
                throw new ArgumentNullException(nameof(playerReference));
            }

            if (Player1.PlayerReference == null)
            {
                Player1.SetPlayerReference(playerReference);
                return true;
            }

            if (Player2.PlayerReference == null)
            {
                Player2.SetPlayerReference(playerReference);
                return true;
            }

            return false;
        }

        // Needs to restricted to GroupBase?
        public bool AssignPlayerReferences(PlayerReference player1Reference, PlayerReference player2Reference)
        {
            if (player1Reference == null || player2Reference == null || player1Reference.Id != player2Reference.Id)
            {
                Player1.SetPlayerReference(player1Reference);
                Player2.SetPlayerReference(player2Reference);
                return true;
            }

            return false;
        }

        public Player FindPlayer(Guid id)
        {
            if (Player1 != null && Player1.Id == id)
            {
                return Player1;
            }

            if (Player2 != null && Player2.Id == id)
            {
                return Player2;
            }

            return null;
        }

        public Player FindPlayer(string name)
        {
            if (Player1 != null && Player1.Name == name)
            {
                return Player1;
            }

            if (Player2 != null && Player2.Name == name)
            {
                return Player2;
            }

            return null;
        }

        public bool SetStartDateTime(DateTime dateTime)
        {
            if (SystemTime.Now > dateTime)
            {
                // LOGG
                return false;
            }

            StartDateTime = dateTime;
            return true;
        }

        public PlayState GetPlayState()
        {
            if (StartDateTime > SystemTime.Now)
            {
                return PlayState.NotBegun;
            }

            return AnyPlayerHasWon() ? PlayState.IsFinished : PlayState.IsPlaying;
        }

        public Player GetWinningPlayer()
        {
            if (AnyPlayerHasWon())
            {
                return Player1.Score > Player2.Score ? Player1 : Player2;
            }

            return null;
        }

        public Player GetLosingPlayer()
        {
            if (AnyPlayerHasWon())
            {
                return Player1.Score < Player2.Score ? Player1 : Player2;
            }

            return null;
        }

        private bool AnyPlayerHasWon()
        {
            int matchPointBarrier = Group.Round.BestOf - (Group.Round.BestOf / 2);

            return Player1.Score >= matchPointBarrier || Player2.Score >= matchPointBarrier;
        }
    }
}
