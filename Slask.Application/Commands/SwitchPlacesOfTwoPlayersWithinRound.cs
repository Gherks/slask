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
        public Guid Player1Id { get; }
        public Guid Match2Id { get; }
        public Guid Player2Id { get; }

        public SwitchPlacesOfTwoPlayersWithinRound(Guid tournamentId, Guid match1Id, Guid player1Id, Guid match2Id, Guid player2Id)
        {
            TournamentId = tournamentId;
            Match1Id = match1Id;
            Player1Id = player1Id;
            Match2Id = match2Id;
            Player2Id = player2Id;
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
                return Result.Failure($"Could not switch places on two players ({ command.Player1Id }, { command.Player2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Tournament ({ command.TournamentId }) not found.");
            }

            Match match1 = tournament.GetMatchById(command.Match1Id);

            if (match1 == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.Player1Id }, { command.Player2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Match ({ command.Match1Id }) not found.");
            }

            Match match2 = tournament.GetMatchById(command.Match2Id);

            if (match2 == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.Player1Id }, { command.Player2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Match ({ command.Match2Id }) not found.");
            }

            Player player1 = match1.FindPlayer(command.Player1Id);

            if (player1 == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.Player1Id }, { command.Player2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Player ({ command.Player1Id }) not found.");
            }

            Player player2 = match2.FindPlayer(command.Player2Id);

            if (player2 == null)
            {
                return Result.Failure($"Could not switch places on two players ({ command.Player1Id }, { command.Player2Id }) in matches ({ command.Match1Id }, { command.Match2Id }). Player ({ command.Player2Id }) not found.");
            }

            bool switchMade = _tournamentRepository.SwitchPlayersInMatches(player1, player2);

            if (!switchMade)
            {
                return Result.Failure($"Could not switch places on two players ({ command.Player1Id }, { command.Player2Id }) in matches ({ command.Match1Id }, { command.Match2Id }).");
            }

            _tournamentRepository.Save();
            return Result.Success();
        }
    }
}
