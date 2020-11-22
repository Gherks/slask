﻿using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Dto.UpdateDtos;
using System;

namespace Slask.Application.Commands
{
    public sealed class UpdateRoundSettings : CommandInterface
    {
        public string TournamentIdentifier { get; }
        public string RoundIdentifier { get; }
        public UpdateRoundSettingsDto UpdateRoundSettingsDto { get; }

        public UpdateRoundSettings(string tournamentIdentifier, string roundIdentifier, UpdateRoundSettingsDto updateRoundSettingsDto)
        {
            TournamentIdentifier = tournamentIdentifier;
            RoundIdentifier = roundIdentifier;
            UpdateRoundSettingsDto = updateRoundSettingsDto;
        }
    }

    public sealed class UpdateRoundSettingsHandler : CommandHandlerInterface<UpdateRoundSettings>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public UpdateRoundSettingsHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result Handle(UpdateRoundSettings command)
        {
            Tournament tournament = GetTournamentByIdentifier(command.TournamentIdentifier);

            if (tournament == null)
            {
                return Result.Failure($"Could not update settings to ({ BuildSettingsString(command.UpdateRoundSettingsDto) }) in round ({ command.RoundIdentifier }). Tournament ({ command.TournamentIdentifier }) not found.");
            }

            RoundBase round = GetRoundByIdentifier(tournament, command.RoundIdentifier);

            if (round == null)
            {
                return Result.Failure($"Could not update settings to ({ BuildSettingsString(command.UpdateRoundSettingsDto) }) in round ({ command.RoundIdentifier }). Round not found.");
            }

            Result result = UpdateTournamentRound(round, command.UpdateRoundSettingsDto);

            if (result.IsSuccess)
            {
                _tournamentRepository.ReconfigureRounds(round.Tournament);
                _tournamentRepository.Save();
            }

            return result;
        }

        private Tournament GetTournamentByIdentifier(string tournamentIdentifier)
        {
            if (Guid.TryParse(tournamentIdentifier, out Guid tournamentId))
            {
                return _tournamentRepository.GetTournament(tournamentId);
            }

            return _tournamentRepository.GetTournament(tournamentIdentifier);
        }

        private RoundBase GetRoundByIdentifier(Tournament tournament, string roundIdentifier)
        {
            if (Guid.TryParse(roundIdentifier, out Guid roundId))
            {
                return tournament.GetRoundById(roundId);
            }

            return tournament.GetRoundByName(roundIdentifier);
        }

        private Result UpdateTournamentRound(RoundBase round, UpdateRoundSettingsDto updateRoundSettingsDto)
        {
            bool changeSuccessful = _tournamentRepository.RenameRoundInTournament(round, updateRoundSettingsDto.Name);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not rename ({ BuildSettingsString(updateRoundSettingsDto) }) round ({ round.Name }).");
            }

            changeSuccessful = _tournamentRepository.SetPlayersPerGroupCountInRound(round, updateRoundSettingsDto.PlayersPerGroupCount);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change players per group count ({ BuildSettingsString(updateRoundSettingsDto) }) setting in round ({ round.Name }).");
            }

            changeSuccessful = _tournamentRepository.SetAdvancingPerGroupCountInRound(round, updateRoundSettingsDto.AdvancingPerGroupCount);

            if (!changeSuccessful)
            {
                return Result.Failure($"Could not change advancing per group count ({ BuildSettingsString(updateRoundSettingsDto) }) setting in round ({ round.Name }).");
            }

            return Result.Success();
        }

        private string BuildSettingsString(UpdateRoundSettingsDto updateRoundSettingsDto)
        {
            string nameSetting = $"Name: { updateRoundSettingsDto.Name }";
            string playersPerGroupCountSetting = $"PlayersPerGroupCount: { updateRoundSettingsDto.PlayersPerGroupCount }";
            string advancingPerGroupCountSetting = $"AdvancingPerGroupCount: { updateRoundSettingsDto.AdvancingPerGroupCount }";

            return $"{ nameSetting }, { playersPerGroupCountSetting }, { advancingPerGroupCountSetting }";
        }
    }
}
