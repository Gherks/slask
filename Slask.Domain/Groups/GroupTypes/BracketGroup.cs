using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups.GroupTypes
{
    public class BracketGroup : GroupBase
    {
        private BracketGroup()
        {
            ContestType = ContestTypeEnum.Bracket;
        }

        public BracketNodeSystem BracketNodeSystem { get; private set; }

        public static BracketGroup Create(BracketRound round)
        {
            if (round == null)
            {
                return null;
            }

            BracketGroup group = new BracketGroup
            {
                RoundId = round.Id,
                Round = round
            };

            group.AssignDefaultName();

            return group;
        }

        public override bool NewDateTimeIsValid(Match match, DateTime dateTime)
        {
            int matchTier = -1;

            for (int tierIndex = 0; tierIndex < BracketNodeSystem.TierCount; ++tierIndex)
            {
                List<BracketNode> bracketNodeTier = BracketNodeSystem.GetBracketNodesInTier(tierIndex);

                foreach (BracketNode bracketNode in bracketNodeTier)
                {
                    if (bracketNode.Match.Id == match.Id)
                    {
                        matchTier = tierIndex;
                        break;
                    }
                }

                if (matchTier != -1)
                {
                    break;
                }
            }

            if (matchTier > 0)
            {
                List<BracketNode> bracketNodeTier = BracketNodeSystem.GetBracketNodesInTier(matchTier - 1);

                foreach (BracketNode bracketNode in bracketNodeTier)
                {
                    if (bracketNode.Match.StartDateTime < dateTime)
                    {
                        return false;
                    }
                }
            }

            if (matchTier < BracketNodeSystem.TierCount - 1)
            {
                List<BracketNode> bracketNodeTier = BracketNodeSystem.GetBracketNodesInTier(matchTier + 1);

                foreach (BracketNode bracketNode in bracketNodeTier)
                {
                    if (bracketNode.Match.StartDateTime > dateTime)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public override void OnMatchScoreIncreased(Match match)
        {
            bool matchExistInThisGroup = Matches.Where(currentMatch => currentMatch.Id == match.Id).Any();

            if (!matchExistInThisGroup)
            {
                // LOG Error: Match does not exist in this group
                return;
            }

            if (BracketNodeSystem == null)
            {
                BracketNodeSystem = new BracketNodeSystem();
                BracketNodeSystem.Construct(Matches);
            }

            BracketNode finalNode = BracketNodeSystem.FinalNode;

            bool matchIsFinished = match.GetPlayState() == PlayStateEnum.Finished;
            bool matchIsNotFinal = match.Id != finalNode.Match.Id;

            if (matchIsFinished && matchIsNotFinal)
            {
                BracketNode bracketNode = finalNode.GetBracketNodeByMatchId(match.Id);
                BracketNode parentNode = bracketNode.Parent;

                parentNode.Match.AssignPlayerReferenceToFirstAvailablePlayer(match.GetWinningPlayerReference());
            }
        }

        public override bool ConstructGroupLayout(int playersPerGroupCount)
        {
            int matchCount = playersPerGroupCount - 1;
            ChangeMatchCountTo(matchCount);

            if (Matches.Count > 0)
            {
                BracketNodeSystem = new BracketNodeSystem();
                BracketNodeSystem.Construct(Matches);
            }

            return true;
        }

        public override void FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences)
        {
            for (int matchIndex = 0; matchIndex < Matches.Count; ++matchIndex)
            {
                int firstIndex = matchIndex * 2;
                int secondIndex = matchIndex * 2 + 1;

                Guid playerReference1Id = playerReferences.Count > firstIndex ? playerReferences[firstIndex].Id : Guid.Empty;
                Guid playerReference2Id = playerReferences.Count > secondIndex ? playerReferences[secondIndex].Id : Guid.Empty;

                Matches[matchIndex].AssignPlayerReferencesToPlayers(playerReference1Id, playerReference2Id);
            }
        }
    }
}
