using Slask.Domain;
using Slask.Persistence.Services;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using TechTalk.SpecFlow;

namespace Slask.Persistence.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "ControlPlaythroughChanges")]
    public class ControlPlaythroughChangesSteps : PersistenceSteps
    {

        [Given(@"round named ""(.*)"" is removed from tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" is removed from tournament named ""(.*)""")]
        public void GivenRoundNamedIsRemovedFromTournamentNamed(string roundName, string tournamentName)
        {
            using (TournamentService tournamentService = CreateTournamentService())
            {
                Tournament tournament = tournamentService.GetTournamentByName(tournamentName);
                tournamentService.RemoveRoundFromTournament(tournament, roundName);
                tournamentService.Save();
            }
        }
    }
}
