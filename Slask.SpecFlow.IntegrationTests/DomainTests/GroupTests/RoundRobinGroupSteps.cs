using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using System;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "RoundRobinGroup")]
    public class RoundRobinGroupSteps : RoundRobinGroupStepDefinitions
    {

    }

    public class RoundRobinGroupStepDefinitions : GroupStepDefinitions
    {
        private readonly Random randomizer;

        public RoundRobinGroupStepDefinitions()
        {
            randomizer = new Random(133742069);
        }

        protected override void PlayAvailableMatches(GroupBase group)
        {
            int winningScore = (int)Math.Ceiling(group.Round.BestOf / 2.0);

            foreach (Domain.Match match in group.Matches)
            {
                bool matchShouldHaveStarted = match.StartDateTime < SystemTime.Now;
                bool matchIsNotFinished = match.GetPlayState() != PlayState.IsFinished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    bool increasePlayer1Score = randomizer.Next(2) == 0;

                    if(increasePlayer1Score)
                    {
                        match.Player1.IncreaseScore(winningScore);
                    }
                    else
                    {
                        match.Player2.IncreaseScore(winningScore);
                    }
                }
            }
        }
    }
}
