using AutoMapper;
using Slask.Application.AutoMapperProfiles;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Repositories;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using TechTalk.SpecFlow;

namespace Slask.AutoMapperProfile.Specflow.UnitTests
{
    [Binding, Scope(Feature = "BareTournamentProfile")]
    public class BareTournamentProfileSteps : DtoValidationSteps
    {
        private readonly IMapper _mapper;

        public BareTournamentProfileSteps()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<BareTournamentProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();
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
    }
}
