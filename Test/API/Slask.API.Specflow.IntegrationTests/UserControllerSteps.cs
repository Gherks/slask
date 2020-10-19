using FluentAssertions;
using Newtonsoft.Json;
using Slask.Common;
using Slask.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "UserController")]
    public sealed class UserControllerSteps : APIControllerSteps
    {
        public UserControllerSteps(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Given(@"GET request is sent to fetch users")]
        [When(@"GET request is sent to fetch users")]
        public async Task GivenGETRequestIsSentToFetchUsers()
        {
            _response = await _client.GetAsync("api/users");
        }

        [Given(@"GET request is sent to fetch user named ""(.*)""")]
        [When(@"GET request is sent to fetch user named ""(.*)""")]
        public async Task GivenGETRequestIsSentToFetchUserNamed(string name)
        {
            _response = await _client.GetAsync("api/users/" + name);
        }

        [Given(@"GET request is sent to fetch user named ""(.*)"" by user id")]
        [When(@"GET request is sent to fetch user named ""(.*)"" by user id")]
        public async Task GivenGETRequestIsSentToFetchUserNamedByUserId(string name)
        {
            await GivenGETRequestIsSentToFetchUserNamed(name);

            List<UserDto> userDtos = await JsonResponseToObjectList<UserDto>();
            UserDto userDto = userDtos.FirstOrDefault(dto => dto.Name == name);

            if (userDto != null)
            {
                _response = await _client.GetAsync("api/users/" + userDto.Id);
            }
        }

        [Given(@"POST request is sent to create a user named ""(.*)""")]
        [When(@"POST request is sent to create a user named ""(.*)""")]
        public async Task GivenPOSTRequestIsSentToCreateAUserNamed(string name)
        {
            string jsonContent = JsonConvert.SerializeObject(new { username = name });
            HttpContent content = CreateHttpContent(jsonContent);

            _response = await _client.PostAsync("api/users", content);
        }

        [Then(@"response should contain users ""(.*)""")]
        public async Task ThenReponseShouldContainJSONContent(string commaSeparatedUsernames)
        {
            List<string> usernames = commaSeparatedUsernames.ToStringList(",");

            List<UserDto> userDtos = await JsonResponseToObjectList<UserDto>();

            userDtos.Should().HaveCount(usernames.Count);

            for (int index = 0; index < userDtos.Count; ++index)
            {
                UserDto userDto = userDtos[index];

                userDto.Id.Should().NotBeEmpty();
                userDto.Name.Should().Be(usernames[index]);
            }
        }
    }
}
