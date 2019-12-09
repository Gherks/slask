using Slask.Domain.Rounds;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Slask.Domain
{
    public class BracketGroup : GroupBase
    {
        private BracketGroup()
        {
        }

        [NotMapped]
        BracketNode FinalNode;

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

        protected override void OnParticipantAdded(PlayerReference playerReference)
        {
            UpdateMatchSetup();
        }

        protected override void OnParticipantRemoved(PlayerReference playerReference)
        {
            UpdateMatchSetup();
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
        }

        private void UpdateMatchSetup()
        {
            int matchAmount = ParticipatingPlayers.Count - 1;
            ChangeMatchAmountTo(matchAmount);

            if (Matches.Count > 0)
            {
                FinalNode = new BracketNode(null, Matches.Last());
                FinalNode.Construct(Matches);
            }

            for (int matchIndex = 0; matchIndex < Matches.Count; ++matchIndex)
            {
                PlayerReference playerReference1 = null;
                PlayerReference playerReference2 = null;

                try
                {
                    playerReference1 = ParticipatingPlayers[matchIndex * 2];
                    playerReference2 = ParticipatingPlayers[(matchIndex * 2) + 1];

                    Matches[matchIndex].AssignPlayerReferences(playerReference1, playerReference2);
                }
                catch
                {
                    if (playerReference1 != null || playerReference2 != null)
                    {
                        Matches[matchIndex].AssignPlayerReferences(playerReference1, playerReference2);
                    }
                    break;
                }
            }
        }
    }

    internal class BracketNode
    {
        public BracketNode(BracketNode parent, Match match)
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

        public bool IsLeaf() { return Children[0] == null && Children[1] == null; }

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
        public void Construct(List<Match> matches)
        {
            if (matches == null)
            {
                throw new ArgumentException(nameof(matches));
            }

            if (Parent != null)
            {
                // LOGG Error: Trying to construct bracket system within a bracket system. A bracket system should only be constructed upon a bracket node without a parent.
                return;
            }

            Queue<BracketNode> nodeQueue = new Queue<BracketNode>();
            nodeQueue.Enqueue(this);

            int matchIndex = matches.Count - 2;

            while (true)
            {
                BracketNode currentNode = nodeQueue.Dequeue();

                for (int childIndex = 0; childIndex < currentNode.Children.Length; ++childIndex)
                {
                    if (matchIndex >= 0)
                    {
                        BracketNode childNode = new BracketNode(currentNode, matches[matchIndex--]);
                        currentNode.Children[childIndex] = childNode;

                        nodeQueue.Enqueue(childNode);
                    }
                    else { return; }
                }

                if (nodeQueue.Count <= 0)
                {
                    // LOGG Error: Ran out of nodes to construct bracket system with before matches. Something whent horribly wrong.
                    return;
                }
            }
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
