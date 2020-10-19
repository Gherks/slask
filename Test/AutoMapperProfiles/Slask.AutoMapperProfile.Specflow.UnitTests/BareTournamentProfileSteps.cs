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
    [Binding, Scope(Feature = "BareTournamentProfile")]
    public class BareTournamentProfileSteps : SpecflowCoreSteps
    {
        private const int _acceptableInaccuracy = 2000;

        private readonly IMapper _mapper;
        private readonly List<BareTournamentDto> _bareTournamentDtos;

        public BareTournamentProfileSteps()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<BareTournamentProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();

            _bareTournamentDtos = new List<BareTournamentDto>();
        }

        [When(@"automapper maps domain tournament ""(.*)"" to a bare tournament DTO")]
        public void WhenAutomapperMapsDomainTournamentToABareTournamentDTO(string tournamentName)
        {
            using (TournamentRepository userRepository = CreateTournamentRepository())
            {
                Tournament tournament = userRepository.GetTournamentByName(tournamentName);
                _bareTournamentDtos.Add(_mapper.Map<BareTournamentDto>(tournament));
            }
        }

        [Then(@"all automapped bare tournament DTOs should be valid")]
        public void ThenAllAutomappedBareTournamentDTOsShouldBeValid()
        {
            foreach (BareTournamentDto bareTournamentDto in _bareTournamentDtos)
            {
                bareTournamentDto.Id.Should().NotBeEmpty();
                bareTournamentDto.Name.Should().NotBeNullOrEmpty();
                bareTournamentDto.Created.Should().BeCloseTo(SystemTime.Now, _acceptableInaccuracy);
            }
        }
    }
}
