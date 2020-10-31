using FluentAssertions;
using Newtonsoft.Json;
using Slask.Common;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Slask.API.Specflow.IntegrationTests.Utilities
{
    public class ControllerStepsBase : DtoValidationSteps, IClassFixture<InMemoryDatabaseWebApplicationFactory<Startup>>
    {
        const string _idReplacementToken = "IdReplacement";
        const string _dtoTypeToken = "DtoType";

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

        [Given(@"GET request is sent to ""(.*)""")]
        [When(@"GET request is sent to ""(.*)""")]
        public async Task GivenGETRequestIsSentTo(string uri)
        {
            _response = await _client.GetAsync(uri);
        }

        [Given(@"GET request is sent to ""(.*)""")]
        [When(@"GET request is sent to ""(.*)""")]
        public async Task GivenGETRequestIsSentTo(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                dynamic dto = await GetReplacementObject(uri, row[_idReplacementToken], row[_dtoTypeToken]);

                uri = uri.Replace(_idReplacementToken, dto.Id.ToString());

                _response = await _client.GetAsync(uri);
            }
        }

        [Given(@"POST request is sent to ""(.*)""")]
        [When(@"POST request is sent to ""(.*)""")]
        public async Task GivenPOSTRequestIsSentToWithContent(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                _response = await _client.PostAsync(uri, GetHttpContentFromRow(row));
            }
        }

        [When(@"PUT request is sent to ""(.*)""")]
        public async Task GivenPUTRequestIsSentToWithContent(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                dynamic dto = await GetReplacementObject(uri, row[_idReplacementToken], row[_dtoTypeToken]);

                uri = uri.Replace(_idReplacementToken, dto.Id.ToString());

                _response = await _client.PutAsync(uri, GetHttpContentFromRow(row));
            }
        }

        [When(@"DELETE request is sent to ""(.*)""")]
        public async Task WhenDELETERequestIsSentToWithContent(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                dynamic dto = await GetReplacementObject(uri, row[_idReplacementToken], row[_dtoTypeToken]);

                uri = uri.Replace(_idReplacementToken, dto.Id.ToString());

                _response = await _client.DeleteAsync(uri);
            }
        }

        [When(@"OPTIONS request is sent to ""(.*)""")]
        public async Task WhenOPTIONSRequestIsSentTo(string uri)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Options, uri);
            _response = await _client.SendAsync(message);
        }

        [Given(@"response return with status code ""(.*)""")]
        [When(@"response return with status code ""(.*)""")]
        [Then(@"response return with status code ""(.*)""")]
        public void ThenResponseReturnWithStatusCode(int statusCode)
        {
            _response.StatusCode.Should().Be(statusCode);
        }

        [Then(@"response header contain endpoints ""(.*)""")]
        public void ThenResponseHeaderContainEndpoints(string commaSeparatedEndpoints)
        {
            List<string> endpoints = commaSeparatedEndpoints.ToStringList(",");
            List<string> allowed = _response.Content.Headers.Allow.ToList();

            allowed.Should().HaveCount(endpoints.Count);

            foreach (string endpoint in endpoints)
            {
                allowed.Contains(endpoint);
            }
        }

        public List<ObjectType> JsonResponseToObjectList<ObjectType>(string responseContent)
        {
            try
            {
                ObjectType deserializedObject = JsonConvert.DeserializeObject<ObjectType>(responseContent);
                return new List<ObjectType>() { deserializedObject };
            }
            catch
            {
                return JsonConvert.DeserializeObject<List<ObjectType>>(responseContent);
            }
        }

        private async Task<dynamic> GetReplacementObject(string baseUri, string replacementValue, string dtoType)
        {
            string replacementUri = baseUri.Replace(_idReplacementToken, replacementValue);

            _response = await _client.GetAsync(replacementUri);
            string responseContent = await _response.Content.ReadAsStringAsync();

            dynamic list = ExtractGenericContentsFromResponseContent(dtoType, responseContent);

            if (list.Count != 1)
            {
                throw new InvalidOperationException();
            }

            return list[0];
        }

        private dynamic ExtractGenericContentsFromResponseContent(string dtoType, string responseContent)
        {
            Type type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).First(x => x.Name == dtoType);

            MethodInfo method = typeof(ControllerStepsBase).GetMethod(nameof(JsonResponseToObjectList));
            MethodInfo generic = method.MakeGenericMethod(type);
            return generic.Invoke(this, new object[] { responseContent });
        }

        private HttpContent GetHttpContentFromRow(TableRow row)
        {
            Dictionary<string, object> rowDictionary = new Dictionary<string, object>();

            foreach (KeyValuePair<string, string> cell in row)
            {
                if (cell.Key != _idReplacementToken && cell.Key != _dtoTypeToken)
                {
                    rowDictionary[cell.Key] = cell.Value;
                }
            }

            return CreateHttpContent(JsonConvert.SerializeObject(rowDictionary));
        }

        private StringContent CreateHttpContent(string content)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));

            return new StringContent(content, Encoding.UTF8, _contentType);
        }
    }
}
