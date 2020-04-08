using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain
{
    public class Match
    {
        private Match()
        {
            Players = new List<Player>();
            Players.Add(null);
            Players.Add(null);
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
                Group = group
            };

            if (group.Matches.Count == 0)
            {
                if (group.Round.IsFirstRound())
                {
                    match.StartDateTime = SystemTime.Now.AddDays(7);
                }
                else
                {
                    RoundBase previousRound = group.Round.GetPreviousRound();
                    Match lastMatchInPreviousRound = previousRound.Groups.Last().Matches.Last();
                    match.StartDateTime = lastMatchInPreviousRound.StartDateTime.AddHours(1);
                }
            }
            else
            {
                Match previousMatch = group.Matches.Last();
                match.StartDateTime = previousMatch.StartDateTime.AddHours(1);
            }

            return match;
        }

        public bool IsReady()
        {
            return Player1 != null && Player2 != null;
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
                return true;
            }

            if (Player2 == null)
            {
                CreatePlayerWithReference(1, playerReference);
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
                // LOG Error: New start date time must be a future date time
                return false;
            }

            if (!Group.NewDateTimeIsValidWithGroupRules(this, dateTime))
            {
                // LOG Error: New start date time does not work with group rules
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

        private bool CreatePlayerWithReference(int playerIndex, PlayerReference playerReference)
        {
            if (GetPlayState() == PlayState.NotBegun)
            {
                Players[playerIndex] = Player.Create(this, playerReference);
                return true;
            }

            return false;
        }

        private bool AnyPlayerHasWon()
        {
            if (Player1 != null && Player2 != null)
            {
                int matchPointBarrier = Group.Round.BestOf - (Group.Round.BestOf / 2);

                return Player1.Score >= matchPointBarrier || Player2.Score >= matchPointBarrier;
            }

            return false;
        }
    }
}
