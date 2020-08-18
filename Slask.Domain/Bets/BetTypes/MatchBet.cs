﻿using Slask.Domain.ObjectState;
using Slask.Domain.Utilities;
using System;

namespace Slask.Domain.Bets.BetTypes
{
    public class MatchBet : BetBase
    {
        private MatchBet()
        {
        }

        public Guid MatchId { get; private set; }
        public Match Match { get; private set; }
        public Guid PlayerId { get; private set; }
        public Player Player { get; private set; }

        public static MatchBet Create(Better better, Match match, Player player)
        {
            bool anyParameterIsInvalid = !ParametersAreValid(better, match, player);

            if (anyParameterIsInvalid)
            {
                return null;
            }

            bool matchBetIsInvalid = !MatchBetIsValid(match, player);

            if (matchBetIsInvalid)
            {
                return null;
            }

            return new MatchBet
            {
                BetterId = better.Id,
                Better = better,
                MatchId = match.Id,
                Match = match,
                PlayerId = player.Id,
                Player = player,
                ObjectState = ObjectStateEnum.Added
            };
        }

        public override bool IsWon()
        {
            bool anyParameterIsInvalid = !ParametersAreValid(Better, Match, Player);

            if (anyParameterIsInvalid)
            {
                return false;
            }

            bool notFinished = Match.GetPlayState() != PlayStateEnum.Finished;

            if (notFinished)
            {
                return false;
            }

            Player winningPlayer = Match.GetWinningPlayer();

            bool betIsWon = Player.Id == winningPlayer.Id;
            return betIsWon;
        }

        private static bool ParametersAreValid(Better better, Match match, Player player)
        {
            bool invalidBetterGiven = better == null;

            if (invalidBetterGiven)
            {
                // LOG Error: Cannot create match bet because given better was invalid
                return false;
            }

            bool invalidMatchGiven = match == null;

            if (invalidMatchGiven)
            {
                // LOG Error: Cannot create match bet because given match was invalid
                return false;
            }

            bool invalidPlayerGiven = player == null;

            if (invalidPlayerGiven)
            {
                // LOG Error: Cannot create match bet because given player was invalid
                return false;
            }

            return true;
        }

        private static bool MatchBetIsValid(Match match, Player player)
        {
            bool givenPlayerIsNotParticipantInGivenMatch = match.FindPlayer(player.Id) == null;

            if (givenPlayerIsNotParticipantInGivenMatch)
            {
                // LOG Error: Cannot create match bet because given player was not part of given match
                return false;
            }

            bool matchIsNotReady = !match.IsReady();

            if (matchIsNotReady)
            {
                // LOG Issue?: Cannot create match bet because given match is not ready
                return false;
            }

            bool matchHasBegun = match.GetPlayState() != PlayStateEnum.NotBegun;

            if (matchHasBegun)
            {
                // LOG Issue?: Cannot create match bet because given match has already begun
                return false;
            }

            return true;
        }
    }
}
