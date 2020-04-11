using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Domain.Groups
{
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
                    // LOGG Error: Ran out of nodes to construct bracket system with before matches. Something went horribly wrong.
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
}
