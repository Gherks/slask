using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.ObjectState;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;

namespace Slask.Domain
{
    public class Match : ObjectStateBase, SortableInterface
    {
        private Match()
        {
            Id = Guid.NewGuid();
            BestOf = 3;

            Players = new List<Player>();
            Players.Add(null);
            Players.Add(null);
        }

        public Guid Id { get; private set; }
        public int SortOrder { get; private set; }
        public int BestOf { get; protected set; }
        public DateTime StartDateTime { get; private set; }
        private List<Player> Players { get; set; }
        public Guid GroupId { get; private set; }
        public GroupBase Group { get; private set; }

        // Ignored by SlaskContext
        public Player Player1 { get { return Players[0]; } private set { } }
        public Player Player2 { get { return Players[1]; } private set { } }


        public static Match Create(GroupBase group)
        {
            bool groupIsInvalid = group == null;

            if (groupIsInvalid)
            {
                // LOG Error: Cannot create match with invalid group
                return null;
            }

            Match match = new Match()
            {
                StartDateTime = DateTime.MaxValue,
                GroupId = group.Id,
                Group = group
            };

            return match;
        }

        public bool SetBestOf(int bestOf)
        {
            bool matchHasNotBegun = GetPlayState() == PlayStateEnum.NotBegun;
            bool newBestOfIsUneven = bestOf % 2 != 0;
            bool bestOfIsGreaterThanZero = bestOf > 0;

            if (matchHasNotBegun && newBestOfIsUneven && bestOfIsGreaterThanZero)
            {
                BestOf = bestOf;
                MarkAsModified();

                return true;
            }

            return false;
        }

        public bool SetStartDateTime(DateTime dateTime)
        {
            bool newDateTimeIsValid = MatchStartDateTimeValidator.Validate(this, dateTime);

            if (newDateTimeIsValid)
            {
                StartDateTime = dateTime;
                MarkAsModified();

                return true;
            }

            return false;
        }

        public bool SetPlayers(PlayerReference player1Reference, PlayerReference player2Reference)
        {
            if (player1Reference == null || player2Reference == null || player1Reference.Id != player2Reference.Id)
            {
                CreatePlayerWithReference(0, player1Reference);
                CreatePlayerWithReference(1, player2Reference);
                MarkAsModified();

                return true;
            }

            return false;
        }

        public bool AddPlayer(PlayerReference playerReference)
        {
            if (playerReference == null)
            {
                throw new ArgumentNullException(nameof(playerReference));
            }

            if (Player1 == null)
            {
                CreatePlayerWithReference(0, playerReference);
                MarkAsModified();

                return true;
            }

            if (Player2 == null)
            {
                CreatePlayerWithReference(1, playerReference);
                MarkAsModified();

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

        public bool IsReady()
        {
            return Player1 != null && Player2 != null;
        }

        public PlayStateEnum GetPlayState()
        {
            if (StartDateTime > SystemTime.Now)
            {
                return PlayStateEnum.NotBegun;
            }

            return AnyPlayerHasWon() ? PlayStateEnum.Finished : PlayStateEnum.Ongoing;
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

        private bool CreatePlayerWithReference(int playerIndex, PlayerReference playerReference)
        {
            if (GetPlayState() == PlayStateEnum.NotBegun)
            {
                Players[playerIndex] = Player.Create(this, playerReference);
                MarkAsModified();

                return true;
            }

            return false;
        }

        private bool AnyPlayerHasWon()
        {
            if (Player1 != null && Player2 != null)
            {
                int matchPointBarrier = BestOf - (BestOf / 2);

                return Player1.Score >= matchPointBarrier || Player2.Score >= matchPointBarrier;
            }

            return false;
        }

        public void UpdateSortOrder()
        {
            if (ObjectState == ObjectStateEnum.Deleted)
            {
                return;
            }

            for (int index = 0; index < Group.Matches.Count; ++index)
            {
                if (Group.Matches[index].Id == Id)
                {
                    if (SortOrder != index)
                    {
                        MarkAsModified();
                    }

                    SortOrder = index;
                    return;
                }
            }
        }
    }
}
