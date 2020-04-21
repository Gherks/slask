using Slask.Domain.Groups.Bases;
using Slask.Domain.Groups.GroupUtility;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain.Groups
{
    public class BracketGroup : GroupBase
    {
        private BracketGroup()
        {
        }

        [NotMapped]
        public BracketNodeSystem BracketNodeSystem;

        public static BracketGroup Create(BracketRound round)
        {
            if (round == null)
            {
                return null;
            }

            return new BracketGroup
            {
                Id = Guid.NewGuid(),
                RoundId = round.Id,
                Round = round
            };
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
                // LOGG Error: Match does not exist in this group
                return;
            }

            BracketNode finalNode = BracketNodeSystem.FinalNode;

            bool matchIsFinished = match.GetPlayState() == PlayState.IsFinished;
            bool matchIsNotFinal = match.Id != finalNode.Match.Id;

            if (matchIsFinished && matchIsNotFinal)
            {
                BracketNode bracketNode = finalNode.GetBracketNodeByMatchId(match.Id);
                BracketNode parentNode = bracketNode.Parent;

                parentNode.Match.AddPlayer(match.GetWinningPlayer().PlayerReference);
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

        public override bool FillMatchesWithPlayerReferences(List<PlayerReference> playerReferences)
        {
            for (int matchIndex = 0; matchIndex < Matches.Count; ++matchIndex)
            {
                PlayerReference playerReference1 = null;
                PlayerReference playerReference2 = null;

                try
                {
                    playerReference1 = playerReferences[matchIndex * 2];
                    playerReference2 = playerReferences[(matchIndex * 2) + 1];

                    Matches[matchIndex].SetPlayers(playerReference1, playerReference2);
                }
                catch
                {
                    if (playerReference1 != null || playerReference2 != null)
                    {
                        Matches[matchIndex].SetPlayers(playerReference1, playerReference2);
                    }
                    break;
                }
            }

            return true;
        }
    }
}
