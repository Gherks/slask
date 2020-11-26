using Slask.API.Specflow.IntegrationTests.Utilities;
using Slask.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public override async Task GivenPUTRequestIsSentTo(string uri, Table table)
        {
            const string userIdentifier = "UserIdentifier";
            const string idReplacementTag = "-Id";

            foreach (TableRow row in table.Rows)
            {
                bool containsUserIdentifierKey = row.Keys.Contains(userIdentifier);

                if (containsUserIdentifierKey)
                {
                    string identifier = row[userIdentifier];
                    string identifierTag = identifier.Substring(identifier.Length - idReplacementTag.Length);

                    bool shouldReplaceUsernameWithUserId = identifierTag == idReplacementTag;

                    if (shouldReplaceUsernameWithUserId)
                    {
                        Guid userId = await ReplaceUserIdentifierNameWithId(identifier, row);

                        row[userIdentifier] = userId.ToString();
                    }
                }
            }

            await base.GivenPUTRequestIsSentTo(uri, table);
        }

        private async Task<Guid> ReplaceUserIdentifierNameWithId(string identifier, TableRow row)
        {
            string username = identifier.Substring(0, identifier.Length - 3);

            await GivenGETRequestIsSentTo("api/users/" + username);
            string responseContent = await _response.Content.ReadAsStringAsync();

            List<UserDto> userDtos = JsonResponseToObjectList<UserDto>(responseContent);

            return userDtos.First().Id;
        }
    }
}
