using Slask.Domain.Groups.Bases;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;

/*
 * Excerpt from Wikipedia (https://en.wikipedia.org/wiki/Round-robin_tournament)
 * If N is the number of competitors, a pure round robin tournament requires N / 2 * (N - 1) games. If N is even, then in each
 * of (N - 1) rounds, N / 2 games can be run concurrently, provided there exist sufficient resources (e.g. courts for a tennis 
 * tournament). If N is odd, there will be N rounds, each with (N - 1) / 2 games, and one competitor having no game in that round. 
 */
namespace Slask.Domain.Groups
{
    public class RoundRobinGroup : GroupBase
    {
        private RoundRobinGroup()
        {
        }

        public static RoundRobinGroup Create(RoundRobinRound round)
        {
            if (round == null)
            {
                return null;
            }

            return new RoundRobinGroup()
            {
                Id = Guid.NewGuid(),
                RoundId = round.Id,
                Round = round
            };
        }

        // CREATE TESTS
        public override bool NewDateTimeIsValid(Match match, DateTime dateTime)
        {
            bool matchBelongsToFirstRound = match.Group.Round.IsFirstRound();

            if (matchBelongsToFirstRound)
            {
                return true;
            }

            Match lastMatchOfPreviousRound = match.Group.Round.GetPreviousRound().GetLastMatch();

            if (dateTime < lastMatchOfPreviousRound.StartDateTime)
            {
                return false;
            }

            return true;
        }

        public override bool ConstructGroupLayout(int playersPerGroupCount)
        {
            Matches = RoundRobinGroupLayoutAssembler.ConstructMathes(playersPerGroupCount, this);
            return true;
        }

        public override bool FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences)
        {
            RoundRobinGroupLayoutAssembler.FillMatchesWithPlayers(playerReferences, Matches);
            return true;
        }
    }
}
