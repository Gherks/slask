using FluentAssertions;
using Newtonsoft.Json;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace Slask.API.Specflow.IntegrationTests
{
    public class APIControllerSteps : SpecflowCoreSteps, IClassFixture<InMemoryDatabaseWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        private string _contentType;
        private string _accept;
        private HttpResponseMessage _response;

        public APIControllerSteps(InMemoryDatabaseWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }


        [Given(@"Content-Type is set to ""(.*)"" and Accept is set to ""(.*)""")]
        [When(@"Content-Type is set to ""(.*)"" and Accept is set to ""(.*)""")]
        public void GivenContent_TypeIsSetToAndAcceptIsSetTo(string contentType, string accept)
        {
            _contentType = contentType;
            _accept = accept;
        }

        [Given(@"GET request is sent to ""(.*)""")]
        [When(@"GET request is sent to ""(.*)""")]
        public void GivenGetRequestIsSentToContainingBody(string address)
        {
            _response = _client.GetAsync(address).Result;
        }

        [Given(@"POST request is sent to ""(.*)"" containing body")]
        [When(@"POST request is sent to ""(.*)"" containing body")]
        public void GivenPostRequestIsSentToContainingBody(string address, Table table)
        {
            string jsonContent = "";

            foreach (TableRow row in table.Rows)
            {
                jsonContent += JsonConvert.SerializeObject(row);
            }

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));
            HttpContent body = new StringContent(jsonContent, Encoding.UTF8, _contentType);

            _response = _client.PostAsync(address, body).Result;
        }

        [Then(@"response should return with status code ""(.*)""")]
        public void ThenThePOSTResultShouldReturnStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be(statusCode);
        }

        [Then(@"response should contain JSON content")]
        public void ThenReponseShouldContainJSONContent(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
