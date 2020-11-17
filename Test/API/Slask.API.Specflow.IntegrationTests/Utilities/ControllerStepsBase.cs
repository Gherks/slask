using FluentAssertions;
using Newtonsoft.Json;
using Slask.Common;
using Slask.Dto;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace Slask.API.Specflow.IntegrationTests.Utilities
{
    public class ControllerStepsBase : DtoValidationSteps, IClassFixture<InMemoryDatabaseWebApplicationFactory<Startup>>
    {
        const string _idReplacementToken = "IdReplacement";

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

        [Given(@"POST request is sent to ""(.*)""")]
        [When(@"POST request is sent to ""(.*)""")]
        public async Task GivenPOSTRequestIsSentTo(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                _response = await _client.PostAsync(uri, CreateHttpContentFromRow(row));
            }
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
                string finalizedUri = uri;

                if (uri.Contains(_idReplacementToken))
                {
                    finalizedUri = await ConvertNamesToIdsInUri(uri, row);
                }

                _response = await _client.GetAsync(finalizedUri);
            }
        }

        [Given(@"PUT request is sent to ""(.*)""")]
        [When(@"PUT request is sent to ""(.*)""")]
        public async Task GivenPUTRequestIsSentTo(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                string finalizedUri = uri;

                if (uri.Contains(_idReplacementToken))
                {
                    finalizedUri = await ConvertNamesToIdsInUri(uri, row);
                }

                _response = await _client.PutAsync(finalizedUri, CreateHttpContentFromRow(row));
            }
        }

        [When(@"DELETE request is sent to ""(.*)""")]
        public async Task WhenDELETERequestIsSentTo(string uri)
        {
            _response = await _client.DeleteAsync(uri);
        }

        [When(@"DELETE request is sent to ""(.*)""")]
        public async Task WhenDELETERequestIsSentTo(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                string finalizedUri = uri;

                if (uri.Contains(_idReplacementToken))
                {
                    finalizedUri = await ConvertNamesToIdsInUri(uri, row);
                }

                _response = await _client.DeleteAsync(finalizedUri);
            }
        }

        [When(@"HEAD request is sent to ""(.*)""")]
        public async Task WhenHEADRequestIsSentTo(string uri)
        {
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Head, uri);
            _response = await _client.SendAsync(message);
        }

        [When(@"HEAD request is sent to ""(.*)""")]
        public async Task WhenHEADRequestIsSentTo(string uri, Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                string finalizedUri = uri;

                if (uri.Contains(_idReplacementToken))
                {
                    finalizedUri = await ConvertNamesToIdsInUri(uri, row);
                }

                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Head, finalizedUri);
                _response = await _client.SendAsync(message);
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

        private async Task<string> ConvertNamesToIdsInUri(string uri, TableRow row)
        {
            int replacementIndex = 0;
            TournamentDto tournamentDto = null;
            RoundDto roundDto = null;

            while (true)
            {
                string idReplacementToken = _idReplacementToken + replacementIndex.ToString();
                string idStandIn;

                try
                {
                    idStandIn = row[idReplacementToken];
                }
                catch
                {
                    break;
                }

                string componentType = GetComponentTypeOfIdReplacement(uri, idReplacementToken);

                if (componentType == "users")
                {
                    string userUri = uri.Replace(idReplacementToken, idStandIn);
                    UserDto userDto = await FetchObject<UserDto>(userUri);

                    uri = uri.Replace(idReplacementToken, userDto.Id.ToString());
                }
                else if (componentType == "tournaments")
                {
                    string tournamentUri = SeverUriAtComponent(uri, "tournaments");
                    tournamentUri = tournamentUri.Replace(idReplacementToken, idStandIn);

                    tournamentDto = await FetchObject<TournamentDto>(tournamentUri);
                    uri = uri.Replace(idReplacementToken, tournamentDto.Id.ToString());
                }
                else if (componentType == "rounds")
                {
                    if (tournamentDto == null)
                    {
                        string tournamentUri = SeverUriAtComponent(uri, "tournaments");
                        tournamentDto = await FetchObject<TournamentDto>(tournamentUri);
                    }

                    roundDto = tournamentDto.Rounds.First(round => round.Name == idStandIn);
                    uri = uri.Replace(idReplacementToken, roundDto.Id.ToString());
                }

                replacementIndex++;
            }

            return uri;
        }

        private string GetComponentTypeOfIdReplacement(string uri, string idReplacementToken)
        {
            int componentTypeEnd = uri.IndexOf("/" + idReplacementToken) - 1;
            int componentTypeStart = uri.LastIndexOf("/", componentTypeEnd) + 1;
            return uri.Substring(componentTypeStart, componentTypeEnd - componentTypeStart + 1);
        }

        private string SeverUriAtComponent(string uri, string componentName)
        {
            string component = componentName + "/";
            int componentEnd = uri.IndexOf(component);
            int uriEnd = uri.IndexOf("/", componentEnd + component.Length);

            if (uriEnd == -1)
            {
                return uri;
            }

            return uri.Substring(0, uriEnd);
        }

        private async Task<TypeObject> FetchObject<TypeObject>(string uri)
        {
            HttpResponseMessage response = await _client.GetAsync(uri);
            string responseContent = await response.Content.ReadAsStringAsync();

            List<TypeObject> dtos = JsonResponseToObjectList<TypeObject>(responseContent);

            return dtos.First();
        }

        private HttpContent CreateHttpContentFromRow(TableRow row)
        {
            Dictionary<string, object> rowDictionary = new Dictionary<string, object>();

            foreach (KeyValuePair<string, string> cell in row)
            {
                if (cell.Key.Contains(_idReplacementToken))
                {
                    continue;
                }

                if (int.TryParse(cell.Value, out int number))
                {
                    rowDictionary[cell.Key] = number;
                }
                else
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
