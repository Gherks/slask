using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.ObjectState;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain
{
    public class Match : ObjectStateBase, SortableInterface
    {
        private Match()
        {
            Id = Guid.NewGuid();
            BestOf = 3;

            Player1 = Player.Create(this);
            Player1Id = Player1.Id;

            Player2 = Player.Create(this);
            Player2Id = Player2.Id;
        }

        public Guid Id { get; private set; }
        public int SortOrder { get; private set; }
        public int BestOf { get; protected set; }
        public DateTime StartDateTime { get; private set; }
        public Guid Player1Id { get; private set; }
        public Player Player1 { get; private set; }
        public Guid Player2Id { get; private set; }
        public Player Player2 { get; private set; }
        public Guid GroupId { get; private set; }
        public GroupBase Group { get; private set; }


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

        public override void MarkForDeletion()
        {
            base.MarkForDeletion();
            Player1.MarkForDeletion();
            Player2.MarkForDeletion();
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

        public bool AssignPlayerReferencesToPlayers(Guid player1ReferenceId, Guid player2ReferenceId)
        {
            bool matchHasNotBegun = GetPlayState() == PlayStateEnum.NotBegun;

            if (matchHasNotBegun)
            {
                bool anyPlayerReferenceIsInvalid = player1ReferenceId == Guid.Empty|| player2ReferenceId == Guid.Empty;
                bool bothPlayerReferencesDiffer = anyPlayerReferenceIsInvalid || player1ReferenceId != player2ReferenceId;

                if (bothPlayerReferencesDiffer)
                {
                    Player1.AssignPlayerReference(player1ReferenceId);
                    Player2.AssignPlayerReference(player2ReferenceId);

                    return true;
                }
            }

            return false;
        }

        public bool AssignPlayerReferenceToFirstAvailablePlayer(Guid playerReferenceId)
        {
            bool matchHasNotBegun = GetPlayState() == PlayStateEnum.NotBegun;
            bool playerReferenceIsValid = playerReferenceId != Guid.Empty;

            if (matchHasNotBegun && playerReferenceIsValid)
            {
                bool firstPlayerHasNoPlayerReference = Player1.PlayerReferenceId == Guid.Empty;
                if (firstPlayerHasNoPlayerReference)
                {
                    Player1.AssignPlayerReference(playerReferenceId);
                    return true;
                }

                bool secondPlayerHasNoPlayerReference = Player2.PlayerReferenceId == Guid.Empty;
                if (secondPlayerHasNoPlayerReference)
                {
                    Player2.AssignPlayerReference(playerReferenceId);
                    return true;
                }
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
            if (Player1 != null && Player1.GetName() == name)
            {
                return Player1;
            }

            if (Player2 != null && Player2.GetName() == name)
            {
                return Player2;
            }

            return null;
        }

        public bool IsReady()
        {
            return Player1.PlayerReferenceId != Guid.Empty && Player2.PlayerReferenceId != Guid.Empty;
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

        private bool AnyPlayerHasWon()
        {
            if (Player1 != null && Player2 != null)
            {
                int matchPointBarrier = BestOf - (BestOf / 2);

                return Player1.Score >= matchPointBarrier || Player2.Score >= matchPointBarrier;
            }

            return false;
        }
    }
}
