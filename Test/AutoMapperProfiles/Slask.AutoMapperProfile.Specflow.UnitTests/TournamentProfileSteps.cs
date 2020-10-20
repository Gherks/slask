using AutoMapper;
using Slask.Application.AutoMapperProfiles;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Repositories;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using TechTalk.SpecFlow;

namespace Slask.AutoMapperProfile.Specflow.UnitTests
{
    [Binding, Scope(Feature = "TournamentProfile")]
    public sealed class TournamentProfileSteps : DtoValidationSteps
    {
        private readonly IMapper _mapper;

        public TournamentProfileSteps()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<TournamentProfile>();
            });

            _mapper = mapperConfiguration.CreateMapper();
        }

        [When(@"automapper maps domain tournament ""(.*)"" to a tournament DTO")]
        public void WhenAutomapperMapssDomainTournamentToATournamentDTO(string tournamentName)
        {
            using (TournamentRepository userRepository = CreateTournamentRepository())
            {
                Tournament tournament = userRepository.GetTournamentByName(tournamentName);
                _tournamentDtos.Add(_mapper.Map<TournamentDto>(tournament));
            }
        }
    }
}
