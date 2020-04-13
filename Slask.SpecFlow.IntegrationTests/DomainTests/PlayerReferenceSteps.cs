using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "PlayerReference")]
    public class PlayerReferenceSteps : PlayerReferenceStepDefinitions
    {

    }

    public class PlayerReferenceStepDefinitions : RoundRobinGroupStepDefinitions
    {
        [Given(@"a player named ""(.*)"" has been added to created group (.*)")]
        [When(@"a player named ""(.*)"" has been added to created group (.*)")]
        public void GivenAPlayerNamedHasBeenAddedToCreatedGroup(string playerName, int groupIndex)
        {
            //GroupBase group = createdGroups[groupIndex];

            //group.AddNewPlayerReference(playerName);
        }

        [When(@"renaming created player reference (.*) to ""(.*)""")]
        public void WhenRenamingCreatedPlayerReferenceTo(int createdPlayerReferenceIndex, string newName)
        {
            PlayerReference playerReference = createdPlayerReferences[createdPlayerReferenceIndex];

            playerReference.RenameTo(newName);
        }

        [Then(@"fetching player references from created tournament (.*) should yeild (.*) player references named: ""(.*)""")]
        public void ThenFetchingPlayerReferencesFromTournamentShouldYeildPlayerReferencesNamed(int createdTournamentIndex, int playerReferencesInTournamentCount, string commaSeparatedPlayerNames)
        {
            Tournament tournament = createdTournaments[createdTournamentIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();

            playerReferences.Should().HaveCount(playerReferencesInTournamentCount);

            foreach (string playerName in playerNames)
            {
                playerReferences.SingleOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }
    }
}
