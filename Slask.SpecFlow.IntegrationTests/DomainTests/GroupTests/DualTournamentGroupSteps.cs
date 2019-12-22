using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using System;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "DualTournamentGroup")]
    public class DualTournamentGroupSteps : DualTournamentGroupStepDefinitions
    {

    }

    public class DualTournamentGroupStepDefinitions : GroupStepDefinitions
    {
        protected override void PlayAvailableMatches(GroupBase group)
        {
            int winningScore = (int)Math.Ceiling(group.Round.BestOf / 2.0);

            foreach (Domain.Match match in group.Matches)
            {
                bool matchShouldHaveStarted = match.StartDateTime < SystemTime.Now;
                bool matchIsNotFinished = match.GetPlayState() != PlayState.IsFinished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    match.Player1.IncreaseScore(winningScore);
                }
            }
        }
    }
}
