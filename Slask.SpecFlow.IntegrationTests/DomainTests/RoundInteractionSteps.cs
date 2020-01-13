﻿using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "RoundInteraction", Tag = "BracketRoundInteractionTag")]
    public class BracketRoundInteractionSteps : BracketGroupStepDefinitions
    {
        [Then(@"fetched advancing players in created round (.*) should be exactly ""(.*)""")]
        public void ThenFetchedAdvancingPlayersInCreatedRoundShouldBeExactly(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            RoundInteractionStepUtility.FetchingAdvancingPlayersInRoundYieldsGivenPlayerNames(round, playerNames);
        }

        [Then(@"fetched advancing players in created round (.*) should yield null")]
        public void ThenFetchedAdvancingPlayersInCreatedRoundShouldBeEmpty(int roundIndex)
        {
            RoundBase round = createdRounds[roundIndex];

            RoundInteractionStepUtility.FetchingAdvancingPlayersInRoundYieldsNull(round);
        }
    }

    [Binding, Scope(Feature = "RoundInteraction", Tag = "DualTournamentRoundInteractionTag")]
    public class DualTournamentRoundInteractionSteps : DualTournamentGroupStepDefinitions
    {
        [Then(@"fetched advancing players in created round (.*) should be exactly ""(.*)""")]
        public void ThenFetchedAdvancingPlayersInCreatedRoundShouldBeExactly(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            RoundInteractionStepUtility.FetchingAdvancingPlayersInRoundYieldsGivenPlayerNames(round, playerNames);
        }

        [Then(@"fetched advancing players in created round (.*) should yield null")]
        public void ThenFetchedAdvancingPlayersInCreatedRoundShouldBeEmpty(int roundIndex)
        {
            RoundBase round = createdRounds[roundIndex];

            RoundInteractionStepUtility.FetchingAdvancingPlayersInRoundYieldsNull(round);
        }
    }

    [Binding, Scope(Feature = "RoundInteraction", Tag = "RoundRobinRoundInteractionTag")]
    public class RoundRobinRoundInteractionSteps : RoundRobinGroupStepDefinitions
    {
        [Then(@"fetched advancing players in created round (.*) should be exactly ""(.*)""")]
        public void ThenFetchedAdvancingPlayersInCreatedRoundShouldBeExactly(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            RoundInteractionStepUtility.FetchingAdvancingPlayersInRoundYieldsGivenPlayerNames(round, playerNames);
        }

        [Then(@"fetched advancing players in created round (.*) should yield null")]
        public void ThenFetchedAdvancingPlayersInCreatedRoundShouldBeEmpty(int roundIndex)
        {
            RoundBase round = createdRounds[roundIndex];

            RoundInteractionStepUtility.FetchingAdvancingPlayersInRoundYieldsNull(round);
        }
    }

    public static class RoundInteractionStepUtility
    {
        public static void FetchingAdvancingPlayersInRoundYieldsGivenPlayerNames(RoundBase round, List<string> playerNames)
        {
            List<PlayerReference> fetchedPlayerReferences = round.GetAdvancingPlayers();

            fetchedPlayerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                fetchedPlayerReferences.SingleOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        public static void FetchingAdvancingPlayersInRoundYieldsNull(RoundBase round)
        {
            round.GetAdvancingPlayers().Should().BeNull();
        }
    }
}
