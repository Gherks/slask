using Slask.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain
{
    public enum MatchState
    {
        HasNotBegun,
        IsBeingPlayed,
        IsFinished
    }

    public class Match
    {
        private Match()
        {
            Players = new List<Player>();
            Players.Add(Player.Create(this));
            Players.Add(Player.Create(this));
        }

        public Guid Id { get; private set; }
        private List<Player> Players { get; set; }
        public DateTime StartDateTime { get; private set; }
        public Guid GroupId { get; private set; }
        public GroupBase Group { get; private set; }

        [NotMapped]
        public Player Player1
        {
            get { return Players[0]; }
            private set {}
        }

        [NotMapped]
        public Player Player2
        {
            get { return Players[1]; }
            private set {}
        }

        public static Match Create()
        {
            return new Match()
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTimeHelper.Now.AddYears(1)
            };
        }

        public bool AssignPlayerReferenceFromGroup(string playerReferenceName)
        {
            PlayerReference playerReference =  Group.GetPlayerReference(playerReferenceName);

            if(ValidateNewPlayerReference(playerReference))
            {
                return false;
            }

            if (Player1.PlayerReference == null)
            {
                Player1.PlayerReference = playerReference;
                return true;
            }

            Player2.PlayerReference = playerReference;
            return true;
        }

        public Player FindPlayer(Guid id)
        {
            return Players.Where(player => player.Id == id).FirstOrDefault();
        }

        public Player FindPlayer(string name)
        {
            return Players.Where(player => player.Name == name).FirstOrDefault();
        }

        public bool ChangeStartDateTime(DateTime dateTime)
        {
            if(DateTimeProvider.Current.Now > dateTime)
            {
                // LOGG
                return false;
            }

            StartDateTime = dateTime;
            return true;
        }

        public MatchState GetState()
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

        private bool ValidateNewPlayerReference(PlayerReference playerReference)
        {
            if (playerReference == null)
            {
                return false;
            }

            bool matchIsFilled = Player1.PlayerReference != null && Player2.PlayerReference != null;

            if (matchIsFilled)
            {
                return false;
            }

            bool alreadyAddedToMatch = FindPlayer(playerReference.Name) != null; 

            if (alreadyAddedToMatch)
            {
                return false;
            }

            return true;
        }
    }
}
