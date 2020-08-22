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
            Guid playerReference1Id = playerReferences.Count > 0 ? playerReferences[0].Id : Guid.Empty;
            Guid playerReference2Id = playerReferences.Count > 1 ? playerReferences[1].Id : Guid.Empty;
            Guid playerReference3Id = playerReferences.Count > 2 ? playerReferences[2].Id : Guid.Empty;
            Guid playerReference4Id = playerReferences.Count > 3 ? playerReferences[3].Id : Guid.Empty;

            Matches[0].AssignPlayerReferencesToPlayers(playerReference1Id, playerReference2Id);
            Matches[1].AssignPlayerReferencesToPlayers(playerReference3Id, playerReference4Id);
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
            Guid match1Winner = GetMatch1().GetWinningPlayer().PlayerReferenceId;
            Guid match2Winner = GetMatch2().GetWinningPlayer().PlayerReferenceId;

            GetWinnersMatch().AssignPlayerReferencesToPlayers(match1Winner, match2Winner);
        }

        private void AssignPlayersToLosersMatch()
        {
            Guid match1Loser = GetMatch1().GetLosingPlayer().PlayerReferenceId;
            Guid match2Loser = GetMatch2().GetLosingPlayer().PlayerReferenceId;

            GetLosersMatch().AssignPlayerReferencesToPlayers(match1Loser, match2Loser);
        }

        private void AssignPlayersToTiebreakerMatch()
        {
            Guid WinnersMatchLoser = GetWinnersMatch().GetLosingPlayer().PlayerReferenceId;
            Guid LosersMatchWinner = GetLosersMatch().GetWinningPlayer().PlayerReferenceId;

            GetTiebreakerMatch().AssignPlayerReferencesToPlayers(WinnersMatchLoser, LosersMatchWinner);
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