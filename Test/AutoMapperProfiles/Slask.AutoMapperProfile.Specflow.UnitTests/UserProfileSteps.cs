using AutoMapper;
using FluentAssertions;
using Slask.Application.AutoMapperProfiles;
using Slask.Common;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Repositories;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.AutoMapperProfile.Specflow.UnitTests
{
    [Binding, Scope(Feature = "UserProfile")]
    public class UserProfileSteps : DtoValidationSteps
    {
        private readonly IMapper _mapper;

        public UserProfileSteps()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<UsersProfile>();
            });
         
            _mapper = mapperConfiguration.CreateMapper();
        }

        [When(@"automapper maps domain user ""(.*)"" to a user DTO")]
        public void WhenAutomapperMapsDomainUserToAUserDTO(string commaSeparatedUserNames)
        {
            List<string> userNames = commaSeparatedUserNames.ToStringList(",");

            using (UserRepository userRepository = CreateUserRepository())
            {
                foreach (string userName in userNames)
                {
                    User user = userRepository.GetUserByName(userName);
                    _userDtos.Add(_mapper.Map<UserDto>(user));
                }
            }
        }
    }
}
