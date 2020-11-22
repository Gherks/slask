using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.ObjectState;
using Slask.Domain.Rounds.RoundUtilities;
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
            PlayerReference1Id = Guid.Empty;
            Player1Score = 0;
            PlayerReference2Id = Guid.Empty;
            Player2Score = 0;
        }

        public Guid Id { get; private set; }
        public int SortOrder { get; private set; }
        public int BestOf { get; protected set; }
        public DateTime StartDateTime { get; private set; }
        public Guid PlayerReference1Id { get; private set; }
        public int Player1Score { get; private set; }
        public Guid PlayerReference2Id { get; private set; }
        public int Player2Score { get; private set; }
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

        public string GetPlayer1Name()
        {
            return GetPlayerName(0);
        }

        public string GetPlayer2Name()
        {
            return GetPlayerName(1);
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

        public bool AssignPlayerReferencesToPlayers(Guid playerReference1Id, Guid playerReference2Id)
        {
            bool matchHasNotBegun = GetPlayState() == PlayStateEnum.NotBegun;

            if (matchHasNotBegun)
            {
                bool anyPlayerReferenceIsInvalid = playerReference1Id == Guid.Empty || playerReference2Id == Guid.Empty;
                bool bothPlayerReferencesDiffer = anyPlayerReferenceIsInvalid || playerReference1Id != playerReference2Id;

                if (bothPlayerReferencesDiffer)
                {
                    PlayerReference1Id = playerReference1Id;
                    PlayerReference2Id = playerReference2Id;
                    MarkAsModified();

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
                if (PlayerReference1Id == Guid.Empty)
                {
                    PlayerReference1Id = playerReferenceId;
                    MarkAsModified();
                    return true;
                }

                if (PlayerReference2Id == Guid.Empty)
                {
                    PlayerReference2Id = playerReferenceId;
                    MarkAsModified();
                    return true;
                }
            }

            return false;
        }

        public bool HasPlayer(Guid id)
        {
            return PlayerReference1Id == id || PlayerReference2Id == id;
        }

        public Guid FindPlayer(string name)
        {
            PlayerReference playerReference = Group.Round.Tournament.GetPlayerReference(name);

            if (playerReference != null)
            {
                if (PlayerReference1Id == playerReference.Id)
                {
                    return PlayerReference1Id;
                }

                if (PlayerReference2Id == playerReference.Id)
                {
                    return PlayerReference2Id;
                }
            }

            return Guid.Empty;
        }

        public bool IsReady()
        {
            return PlayerReference1Id != Guid.Empty && PlayerReference2Id != Guid.Empty;
        }

        public PlayStateEnum GetPlayState()
        {
            if (StartDateTime > SystemTime.Now)
            {
                return PlayStateEnum.NotBegun;
            }

            return AnyPlayerHasWon() ? PlayStateEnum.Finished : PlayStateEnum.Ongoing;
        }

        public Guid GetWinningPlayerReference()
        {
            if (AnyPlayerHasWon())
            {
                return Player1Score > Player2Score ? PlayerReference1Id : PlayerReference2Id;
            }

            return Guid.Empty;
        }

        public Guid GetLosingPlayerReference()
        {
            if (AnyPlayerHasWon())
            {
                return Player1Score < Player2Score ? PlayerReference1Id : PlayerReference2Id;
            }

            return Guid.Empty;
        }

        public bool IncreaseScoreForPlayer(Guid playerReferenceId, int score)
        {
            if (CanChangeScore())
            {
                if (playerReferenceId == PlayerReference1Id)
                {
                    Player1Score += score;
                }
                else if (playerReferenceId == PlayerReference2Id)
                {
                    Player2Score += score;
                }
                else
                {
                    throw new InvalidOperationException("Invalid player reference id given. Id does not match any player in match.");
                }

                Group.OnMatchScoreIncreased(this);

                bool groupJustFinished = Group.GetPlayState() == PlayStateEnum.Finished;

                if (groupJustFinished)
                {
                    if (Group.Round.GetPlayState() == PlayStateEnum.Finished)
                    {
                        AdvancingPlayerTransfer advancingPlayerTransfer = new AdvancingPlayerTransfer();
                        advancingPlayerTransfer.TransferToNextRound(Group.Round);
                    }
                }

                MarkAsModified();

                return true;
            }

            return false;
        }

        public bool IncreaseScoreForPlayer1(int score)
        {
            return IncreaseScoreForPlayer(PlayerReference1Id, score);
        }

        public bool IncreaseScoreForPlayer2(int score)
        {
            return IncreaseScoreForPlayer(PlayerReference2Id, score);
        }

        private bool AnyPlayerHasWon()
        {
            if (PlayerReference1Id != Guid.Empty && PlayerReference2Id != Guid.Empty)
            {
                int matchPointBarrier = BestOf - (BestOf / 2);

                return Player1Score >= matchPointBarrier || Player2Score >= matchPointBarrier;
            }

            return false;
        }

        private bool CanChangeScore()
        {
            bool matchIsReady = IsReady();
            bool matchIsOngoing = GetPlayState() == PlayStateEnum.Ongoing;
            bool tournamentHasNoIssues = !Group.Round.Tournament.HasIssues();

            return matchIsReady && matchIsOngoing && tournamentHasNoIssues;
        }

        private string GetPlayerName(int playerIndex)
        {
            if (playerIndex == 0)
            {
                PlayerReference playerReference = GetPlayerReference(PlayerReference1Id);

                if (playerReference != null)
                {
                    return playerReference.Name;
                }

                return "";
            }
            else if (playerIndex == 1)
            {
                PlayerReference playerReference = GetPlayerReference(PlayerReference2Id);

                if (playerReference != null)
                {
                    return playerReference.Name;
                }

                return "";
            }

            throw new InvalidOperationException();
        }

        private PlayerReference GetPlayerReference(Guid playerReferenceId)
        {
            if (playerReferenceId != Guid.Empty)
            {
                foreach (PlayerReference playerReference in Group.Round.Tournament.PlayerReferences)
                {
                    if (playerReference.Id == playerReferenceId)
                    {
                        return playerReference;
                    }
                }
            }

            return null;
        }
    }
}
