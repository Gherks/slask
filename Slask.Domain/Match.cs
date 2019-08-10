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
        public DateTime StartDateTime { get; private set; }
        private List<Player> Players { get; set; }
        public Guid GroupId { get; private set; }
        public GroupBase Group { get; private set; }

        // Ignored by SlaskContext
        public Player Player1 { get { return Players[0]; } private set { } }
        public Player Player2 { get { return Players[1]; } private set { } }

        public static Match Create(GroupBase group)
        {
            if(group == null)
            {
                // LOGG
                return null;
            }

            Match match = new Match()
            {
                Id = Guid.NewGuid(),
                StartDateTime = DateTimeHelper.Now.AddYears(1),
                GroupId = group.Id,
                Group = group
            };

            return match;
        }

        //public bool AssignPlayerReferenceFromGroup(string playerReferenceName)
        //{
        //    PlayerReference playerReference = Group.GetPlayerReference(playerReferenceName);

        //    if (ValidateNewPlayerReference(playerReference))
        //    {
        //        return false;
        //    }

        //    if (Player1.PlayerReference == null)
        //    {
        //        Player1.PlayerReference = playerReference;
        //        return true;
        //    }

        //    Player2.PlayerReference = playerReference;
        //    return true;
        //}

        public void AssignPlayerReferences(PlayerReference player1Reference, PlayerReference player2Reference)
        {
            if(player1Reference == null ||player2Reference == null)
            {
                return;
            }

            Player1.PlayerReference = player1Reference;
            Player2.PlayerReference = player2Reference;
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
            if (DateTimeProvider.Current.Now > dateTime)
            {
                // LOGG
                return false;
            }

            StartDateTime = dateTime;
            return true;
        }

        public MatchState GetState()
        {
            if (StartDateTime > DateTimeHelper.Now)
            {
                return MatchState.HasNotBegun;
            }

            return AnyPlayerHasWon() ? MatchState.IsFinished : MatchState.IsBeingPlayed;
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

        private bool AnyPlayerHasWon()
        {
            return Player1.Score >= Group.Round.BestOf || Player2.Score >= Group.Round.BestOf;
        }
    }
}
