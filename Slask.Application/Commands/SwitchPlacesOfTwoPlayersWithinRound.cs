using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using System;

namespace Slask.Application.Commands
{
    public sealed class SwitchPlacesOfTwoPlayersWithinRound : CommandInterface
    {
        public Guid TournamentId { get; }
        public Guid Match1Id { get; }
        public Guid PlayerReference1Id { get; }
        public Guid Match2Id { get; }
        public Guid PlayerReference2Id { get; }

        public SwitchPlacesOfTwoPlayersWithinRound(Guid tournamentId, Guid match1Id, Guid playerReference1Id, Guid match2Id, Guid playerReference2Id)
        {
            TournamentId = tournamentId;
            Match1Id = match1Id;
            PlayerReference1Id = playerReference1Id;
            Match2Id = match2Id;
            PlayerReference2Id = playerReference2Id;
        }
    }

    public sealed class SwitchPlacesOfTwoPlayersWithinRoundHandler : CommandHandlerInterface<SwitchPlacesOfTwoPlayersWithinRound>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public SwitchPlacesOfTwoPlayersWithinRoundHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(SwitchPlacesOfTwoPlayersWithinRound command)
        {
            Tournament tournament = _tournamentRepository.GetTournament(command.TournamentId);

            if (tournament == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.PlayerReference1Id }, { command.PlayerReference2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Tournament ({ command.TournamentId }) not found.");
            }

            Match match1 = tournament.GetMatchById(command.Match1Id);

            if (match1 == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.PlayerReference1Id }, { command.PlayerReference2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Match ({ command.Match1Id }) not found.");
            }

            Match match2 = tournament.GetMatchById(command.Match2Id);

            if (match2 == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.PlayerReference1Id }, { command.PlayerReference2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Match ({ command.Match2Id }) not found.");
            }

            if (!match1.HasPlayer(command.PlayerReference1Id))
            {
                return Result.Failure($"Could not switch places on two players ({ command.PlayerReference1Id }, { command.PlayerReference2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Player ({ command.PlayerReference1Id }) not found.");
            }

            if (!match2.HasPlayer(command.PlayerReference2Id))
            {
                return Result.Failure($"Could not switch places on two players ({ command.PlayerReference1Id }, { command.PlayerReference2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Player ({ command.PlayerReference2Id }) not found.");
            }

            bool switchMade = _tournamentRepository.SwitchPlayersInMatches(match1, command.PlayerReference1Id, match2, command.PlayerReference2Id);

            if (!switchMade)
            {
                return Result.Failure($"Could not switch places on two players ({ command.PlayerReference1Id }, { command.PlayerReference2Id }) in matches ({ command.Match1Id }, { command.Match2Id }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
