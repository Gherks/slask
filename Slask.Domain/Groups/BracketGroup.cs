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

        protected override void ConstructGroupLayout()
        {
            int matchAmount = ParticipatingPlayers.Count - 1;
            ChangeMatchAmountTo(matchAmount);

            if (Matches.Count > 0)
            {
                BracketNodeSystem = new BracketNodeSystem();
                BracketNodeSystem.Construct(Matches);
            }

            FillMatchesWithPlayerReferences();
        }

        private void FillMatchesWithPlayerReferences()
        {
            for (int matchIndex = 0; matchIndex < Matches.Count; ++matchIndex)
            {
                PlayerReference playerReference1 = null;
                PlayerReference playerReference2 = null;

                try
                {
                    playerReference1 = ParticipatingPlayers[matchIndex * 2];
                    playerReference2 = ParticipatingPlayers[(matchIndex * 2) + 1];

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
        }
    }

    public class BracketNodeSystem
    {
        internal BracketNodeSystem()
        {
            bracketNodesByTier = new List<List<BracketNode>>();
        }

        public BracketNode FinalNode { get; private set; }

        public int TierCount
        {
            get { return bracketNodesByTier.Count; }
            private set { }
        }

        private readonly List<List<BracketNode>> bracketNodesByTier;

        public List<BracketNode> GetBracketNodesInTier(int bracketTier)
        {
            if (bracketTier >= bracketNodesByTier.Count)
            {
                return null;
            }

            return bracketNodesByTier[bracketTier];
        }

        // Creates one bracket node for each match in given list, minus the final. Loops over list backwards so that 
        // the first bracket node is the final match in bracket.
        //
        // Example for a bracket containing three matches in total (1 Final, 2 Semifinals):
        // The bracket node representing the final is created by allocating a bracket node via the new-keyword. The other
        // two (semifinals) is allocated with this method.
        // To start the algorithm the final bracket node is added to a queue and is the one to have the second and third 
        // match added to its list of children. Every bracket node that is added to a parent bracket node is added to the
        // queue to be processed later. When a bracket node has reached its capacity of children, the next bracket node
        // is dequeued and is the one to get children added to it.
        // 
        // Just before a bracket node is allocated and added to a parent bracket node a check is made to determine if
        // there is any more matches in match list to draw from. When it detects that the first match has been processed
        // the algorithm is aborted.
        internal void Construct(List<Match> matches)
        {
            if (matches == null)
            {
                throw new ArgumentException(nameof(matches));
            }

            bracketNodesByTier.Clear();
            FinalNode = CreateBracketNode(null, matches.Last());

            Queue<BracketNode> nodeQueue = new Queue<BracketNode>();
            nodeQueue.Enqueue(FinalNode);

            int matchIndex = matches.Count - 2;

            while (true)
            {
                BracketNode currentNode = nodeQueue.Dequeue();

                for (int childIndex = 0; childIndex < currentNode.Children.Length; ++childIndex)
                {
                    if (matchIndex >= 0)
                    {
                        BracketNode childNode = CreateBracketNode(currentNode, matches[matchIndex--]);
                        currentNode.Children[childIndex] = childNode;

                        nodeQueue.Enqueue(childNode);
                    }
                    else
                    {
                        return;
                    }
                }

                if (nodeQueue.Count <= 0)
                {
                    // LOGG Error: Ran out of nodes to construct bracket system with before matches. Something whent horribly wrong.
                    return;
                }
            }
        }

        private BracketNode CreateBracketNode(BracketNode parent, Match match)
        {
            BracketNode bracketNode = new BracketNode(parent, match);
            AddBracketNodeToTierList(bracketNode);

            return bracketNode;
        }

        private void AddBracketNodeToTierList(BracketNode bracketNode)
        {
            if (bracketNode == null)
            {
                throw new ArgumentException(nameof(bracketNode));
            }

            while (bracketNode.BracketTier >= bracketNodesByTier.Count)
            {
                bracketNodesByTier.Add(new List<BracketNode>());
            }

            bracketNodesByTier[bracketNode.BracketTier].Add(bracketNode);
        }
    }

    // Represents all matches in bracket with a node tree strucutre. All nodes contains a parent node and two 
    // children nodes. The parent node represents the match the winning player will advance to. The two children
    // nodes represents the matches where the winners of the previous bracket round came from (Example: RO32 -> RO16).
    public class BracketNode
    {
        internal BracketNode(BracketNode parent, Match match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            Children = new BracketNode[2];
            Parent = parent;
            Match = match;
            BracketTier = (Parent == null) ? 0 : Parent.BracketTier + 1;
        }

        public BracketNode[] Children { get; private set; }
        public BracketNode Parent { get; private set; }
        public Match Match { get; private set; }
        public int BracketTier { get; private set; }

        public bool IsFinal() { return Parent == null; }

        public bool IsLeaf() { return !Children.Any(bracketNode => bracketNode != null); }

        // Makes it easier to attain the root node from any node in the tree.
        public BracketNode GetFinalBracketNode()
        {
            BracketNode bracketNode = this;

            while (bracketNode.Parent != null)
            {
                bracketNode = bracketNode.Parent;
            }

            return bracketNode;
        }

        public BracketNode GetBracketNodeByMatchId(Guid matchId)
        {
            BracketNode bracketNode = GetFinalBracketNode();

            return GetBracketNodeByMatchIdRecursive(bracketNode, matchId);
        }

        private BracketNode GetBracketNodeByMatchIdRecursive(BracketNode bracketNode, Guid matchId)
        {
            if (bracketNode == null || bracketNode.Match == null)
            {
                return null;
            }

            if (bracketNode.Match.Id == matchId)
            {
                return bracketNode;
            }

            foreach (BracketNode childNode in bracketNode.Children)
            {
                if (childNode != null)
                {
                    BracketNode foundNode = GetBracketNodeByMatchIdRecursive(childNode, matchId);

                    if (foundNode != null)
                    {
                        return foundNode;
                    }
                }
            }

            return null;
        }
    }
}
