using System;
using System.Linq;

namespace Slask.Domain.Groups.GroupUtility
{
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
            if (SoughtBracketNodeFound(bracketNode, matchId))
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

        private bool SoughtBracketNodeFound(BracketNode bracketNode, Guid matchId)
        {
            if (bracketNode == null || bracketNode.Match == null)
            {
                return false;
            }

            return bracketNode.Match.Id == matchId;
        }
    }
}
