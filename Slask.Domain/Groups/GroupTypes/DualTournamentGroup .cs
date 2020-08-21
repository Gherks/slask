using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups.GroupTypes
{
    public class DualTournamentGroup : GroupBase
    {
        private DualTournamentGroup()
        {
            ContestType = ContestTypeEnum.DualTournament;
        }

        private const int _matchCapacity = 5;

        public static DualTournamentGroup Create(DualTournamentRound round)
        {
            if (round == null)
            {
                return null;
            }

            DualTournamentGroup group = new DualTournamentGroup
            {
                RoundId = round.Id,
                Round = round
            };

            group.AssignDefaultName();

            return group;
        }

        public override bool NewDateTimeIsValid(Match match, DateTime dateTime)
        {
            for (int matchIndex = 0; matchIndex < Matches.Count; ++matchIndex)
            {
                if (Matches[matchIndex].Id == match.Id)
                {
                    if (matchIndex > 0)
                    {
                        if (Matches[matchIndex - 1].StartDateTime > dateTime)
                        {
                            return false;
                        }
                    }

                    if (matchIndex < Matches.Count - 1)
                    {
                        if (Matches[matchIndex + 1].StartDateTime < dateTime)
                        {
                            return false;
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public override void OnMatchScoreIncreased(Match match)
        {
            bool matchExistInThisGroup = Matches.Where(currentMatch => currentMatch.Id == match.Id).Any();

            if (!matchExistInThisGroup)
            {
                // LOG Error: Match does not exist in this group
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

        public override bool ConstructGroupLayout(int playersPerGroupCount)
        {
            ChangeMatchCountTo(_matchCapacity);
            MarkAsModified();

            return true;
        }

        public override void FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences)
        {
            PlayerReference participant1 = playerReferences.Count > 0 ? playerReferences[0] : null;
            PlayerReference participant2 = playerReferences.Count > 1 ? playerReferences[1] : null;
            PlayerReference participant3 = playerReferences.Count > 2 ? playerReferences[2] : null;
            PlayerReference participant4 = playerReferences.Count > 3 ? playerReferences[3] : null;

            Matches[0].SetPlayers(participant1, participant2);
            Matches[1].SetPlayers(participant3, participant4);
        }

        private bool FirstMatchPairHasPlayed(Match match)
        {
            bool matchIsOneOfFirstPairMatches = match.Id == GetMatch1().Id || match.Id == GetMatch2().Id;

            if (matchIsOneOfFirstPairMatches)
            {
                return GetMatch1().GetPlayState() == PlayStateEnum.Finished && GetMatch2().GetPlayState() == PlayStateEnum.Finished;
            }

            return false;
        }

        private bool SecondMatchPairHasPlayed(Match match)
        {
            bool matchIsOneOfSecondPairMatches = match.Id == GetWinnersMatch().Id || match.Id == GetLosersMatch().Id;

            if (matchIsOneOfSecondPairMatches)
            {
                return GetWinnersMatch().GetPlayState() == PlayStateEnum.Finished && GetLosersMatch().GetPlayState() == PlayStateEnum.Finished;
            }

            return false;
        }

        private void AssignPlayersToWinnersMatch()
        {
            PlayerReference Match1Winner = GetMatch1().GetWinningPlayer().PlayerReference;
            PlayerReference Match2Winner = GetMatch2().GetWinningPlayer().PlayerReference;

            GetWinnersMatch().SetPlayers(Match1Winner, Match2Winner);
        }

        private void AssignPlayersToLosersMatch()
        {
            PlayerReference Match1Loser = GetMatch1().GetLosingPlayer().PlayerReference;
            PlayerReference Match2Loser = GetMatch2().GetLosingPlayer().PlayerReference;

            GetLosersMatch().SetPlayers(Match1Loser, Match2Loser);
        }

        private void AssignPlayersToTiebreakerMatch()
        {
            PlayerReference WinnersMatchLoser = GetWinnersMatch().GetLosingPlayer().PlayerReference;
            PlayerReference LosersMatchWinner = GetLosersMatch().GetWinningPlayer().PlayerReference;

            GetTiebreakerMatch().SetPlayers(WinnersMatchLoser, LosersMatchWinner);
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