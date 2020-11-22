using Slask.API.Specflow.IntegrationTests.Utilities;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests.TournamentController
{
    [Binding, Scope(Feature = "TournamentControllerBetter")]
    public class Better : ControllerStepsBase
    {
        public Better(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }
    }
}
