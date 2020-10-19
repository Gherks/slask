using FluentAssertions;
using Newtonsoft.Json;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Slask.API.Specflow.IntegrationTests
{
    public class ControllerStepsBase : SpecflowCoreSteps, IClassFixture<InMemoryDatabaseWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient _client;
        protected HttpResponseMessage _response;

        private string _contentType;
        private string _accept;

        public ControllerStepsBase(InMemoryDatabaseWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Given(@"Content-Type is set to ""(.*)"" and Accept is set to ""(.*)""")]
        [When(@"Content-Type is set to ""(.*)"" and Accept is set to ""(.*)""")]
        public void GivenContentTypeIsSetToAndAcceptIsSetTo(string contentType, string accept)
        {
            _contentType = contentType;
            _accept = accept;
        }

        [Then(@"response should return with status code ""(.*)""")]
        public void ThenResponseShouldReturnWithStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be(statusCode);
        }

        protected StringContent CreateHttpContent(string content)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            return new StringContent(content, Encoding.UTF8, _contentType);
        }

        protected async Task<List<ObjectType>> JsonResponseToObjectList<ObjectType>()
        {
            string responseContent = await _response.Content.ReadAsStringAsync();

            try
            {
                ObjectType userDto = JsonConvert.DeserializeObject<ObjectType>(responseContent);
                return new List<ObjectType>() { userDto };
            }
            catch
            {
                return JsonConvert.DeserializeObject<List<ObjectType>>(responseContent);
            }
        }
    }
}
