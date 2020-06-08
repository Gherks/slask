using Slask.SpecFlow.IntegrationTests.DomainTests;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    [Binding, Scope(Feature = "FetchTest")]
    public class FetchTestSteps : FetchTestStepDefinitions
    {

    }

    public class FetchTestStepDefinitions : BetterStepDefinitions
    {
    }
}
