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
    public class UserProfileSteps : SpecflowCoreSteps
    {
        private readonly IMapper _mapper;
        private readonly List<UserDto> _userDtos;

        public UserProfileSteps()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<UsersProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();

            _userDtos = new List<UserDto>();
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

        [Then(@"all automapped user DTOs should be valid")]
        public void ThenAllAutomappedUserDTOsShouldBeValid()
        {
            foreach (UserDto userDto in _userDtos)
            {
                userDto.Id.Should().NotBeEmpty();
                userDto.Name.Should().NotBeNullOrEmpty();
            }
        }
    }
}
