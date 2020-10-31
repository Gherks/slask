using FluentAssertions;
using Slask.API.Specflow.IntegrationTests.Utilities;
using Slask.Common;
using Slask.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Slask.API.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "UserController")]
    public sealed class UserControllerSteps : ControllerStepsBase
    {
        public UserControllerSteps(InMemoryDatabaseWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Then(@"response should contain users ""(.*)""")]
        public async Task ThenReponseShouldContainJSONContent(string commaSeparatedUsernames)
        {
            List<string> usernames = commaSeparatedUsernames.ToStringList(",");

            string responseContent = await _response.Content.ReadAsStringAsync();

            List<UserDto> userDtos = JsonResponseToObjectList<UserDto>(responseContent);

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
