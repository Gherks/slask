using Slask.Domain.Rounds;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain
{
    public class DualTournamentGroup : GroupBase
    {
        private DualTournamentGroup()
        {
        }

        [NotMapped]
        private const int ParticipatingPlayerCapacity = 4;

        public static DualTournamentGroup Create(DualTournamentRound round)
        {
            if (round == null)
            {
                return null;
            }

            DualTournamentGroup group = new DualTournamentGroup
            {
                Id = Guid.NewGuid(),
                RoundId = round.Id,
                Round = round
            };

            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));
            group.Matches.Add(Match.Create(group));

            return group;
        }

        public override void Clear()
        {
            ParticipatingPlayers.Clear();
            Matches.Clear();

            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
            Matches.Add(Match.Create(this));
        }

        public override PlayerReference AddPlayerReference(string name)
        {
            if (ParticipatingPlayers.Count < ParticipatingPlayerCapacity)
            {
                return base.AddPlayerReference(name);
            }

            return null;
        }

        protected override void OnParticipantAdded(PlayerReference playerReference)
        {
            UpdatePlayerSetup();
        }

        protected override void OnParticipantRemoved(PlayerReference playerReference)
        {
            UpdatePlayerSetup();
        }

        public override void MatchScoreIncreased(Match match)
        {
            bool matchExistInThisGroup = Matches.Where(currentMatch => currentMatch.Id == match.Id).Any();

            if (!matchExistInThisGroup)
            {
                // Match does not exist in this group
                // LOGG
                return;
            }

            if (FirstMatchPairHasPlayed(match))
            {
                AssignPlayersToWinnersMatch();
                AssignPlayersToLosersMatch();
            }

            if (SecondMatchPairHasPlayed(match))
            {
                AssignPlayersToTiebreakerMatch();
            }
        }

        private void UpdatePlayerSetup()
        {
            PlayerReference participant1 = ParticipatingPlayers.Count > 0 ? ParticipatingPlayers[0] : null;
            PlayerReference participant2 = ParticipatingPlayers.Count > 1 ? ParticipatingPlayers[1] : null;
            PlayerReference participant3 = ParticipatingPlayers.Count > 2 ? ParticipatingPlayers[2] : null;
            PlayerReference participant4 = ParticipatingPlayers.Count > 3 ? ParticipatingPlayers[3] : null;

            Matches[0].AssignPlayerReferences(participant1, participant2);
            Matches[1].AssignPlayerReferences(participant3, participant4);
        }

        private bool FirstMatchPairHasPlayed(Match match)
        {
            bool matchIsOneOfFirstPairMatches = match.Id == GetMatch1().Id || match.Id == GetMatch2().Id;

            if (matchIsOneOfFirstPairMatches)
            {
                return GetMatch1().GetPlayState() == PlayState.IsFinished && GetMatch2().GetPlayState() == PlayState.IsFinished;
            }

            return false;
        }

        private bool SecondMatchPairHasPlayed(Match match)
        {
            bool matchIsOneOfSecondPairMatches = match.Id == GetWinnersMatch().Id || match.Id == GetLosersMatch().Id;

            if (matchIsOneOfSecondPairMatches)
            {
                return GetWinnersMatch().GetPlayState() == PlayState.IsFinished && GetLosersMatch().GetPlayState() == PlayState.IsFinished;
            }

            return false;
        }

        private void AssignPlayersToWinnersMatch()
        {
            PlayerReference Match1Winner = GetMatch1().GetWinningPlayer().PlayerReference;
            PlayerReference Match2Winner = GetMatch2().GetWinningPlayer().PlayerReference;

            GetWinnersMatch().AssignPlayerReferences(Match1Winner, Match2Winner);
        }

        private void AssignPlayersToLosersMatch()
        {
            PlayerReference Match1Loser = GetMatch1().GetLosingPlayer().PlayerReference;
            PlayerReference Match2Loser = GetMatch2().GetLosingPlayer().PlayerReference;

            GetLosersMatch().AssignPlayerReferences(Match1Loser, Match2Loser);
        }

        private void AssignPlayersToTiebreakerMatch()
        {
            PlayerReference WinnersMatchLoser = GetWinnersMatch().GetLosingPlayer().PlayerReference;
            PlayerReference LosersMatchWinner = GetLosersMatch().GetWinningPlayer().PlayerReference;

            GetTiebreakerMatch().AssignPlayerReferences(WinnersMatchLoser, LosersMatchWinner);
        }

        private Match GetMatch1()
        {
            return Matches[0];
        }

        private Match GetMatch2()
        {
            return Matches[1];
        }

        private Match GetWinnersMatch()
        {
            return Matches[2];
        }

        private Match GetLosersMatch()
        {
            return Matches[3];
        }

        private Match GetTiebreakerMatch()
        {
            return Matches[4];
        }
    }
}