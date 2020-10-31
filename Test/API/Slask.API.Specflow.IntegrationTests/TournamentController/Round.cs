using Slask.API.Specflow.IntegrationTests.Utilities;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests.TournamentController
{
    [Binding, Scope(Feature = "TournamentControllerRound")]
    public class Round : ControllerStepsBase
    {
        public Round(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }
    }
}
