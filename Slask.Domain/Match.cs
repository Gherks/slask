using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slask.Domain
{
    public class Match
    {
        private Match()
        {
            Players = new List<Player>();
        }

        public bool ContainsPlayer(string playerName)
        {
            if(playerName == "")
            {
                return false;
            }

            return Player1.Name == playerName || Player2.Name == playerName;
        }

        public bool ContainsPlayer(Guid playerId)
        {
            if (playerId == Guid.Empty)
            {
                return false;
            }

            return Player1.Id == playerId || Player2.Id == playerId;
        }

        public void ChangeStartDateTime(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Guid Id { get; private set; }
        private List<Player> Players { get; set; }
        public DateTime StartDateTime { get; private set; }
        public Guid GroupId { get; private set; }
        public Group Group { get; private set; }

        [NotMapped]
        public Player Player1
        {
            get { return Players.Count >= 1 ? Players[0] : null; }
            private set { }
        }
        [NotMapped]
        public Player Player2
        {
            get { return Players.Count >= 2 ? Players[1] : null; }
            private set { }
        }
        public enum State
        {
            HasNotBegun,
            IsBeingPlayed,
            IsFinished
        }

        public static Match Create(string player1Name, string player2Name, DateTime startDateTime)
        {
            bool anyNameIsEmpty = player1Name == "" || player2Name == "";
            bool namesAreIdentical = player1Name == player2Name;
            bool dateTimeIsInThePast = startDateTime < DateTime.Now;

            if (anyNameIsEmpty || namesAreIdentical || dateTimeIsInThePast)
            {
                // LOG ISSUE HERE
                return null;
            }

            Match match = new Match()
            {
                Id = Guid.NewGuid(),
                StartDateTime = startDateTime
            };

            Player player1 = Player.Create(player1Name, match);
            Player player2 = Player.Create(player2Name, match);

            match.Players.Add(player1);
            match.Players.Add(player2);

            return match;
        }

        public State GetState()
        {
            throw new NotImplementedException();
        }

        public Player GetWinningPlayer()
        {
            throw new NotImplementedException();
        }

        public Player GetLosingPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
