using Slask.Domain;
using System;
using System.Collections.Generic;

namespace Slask.Persistence.Services
{
    public interface TournamentServiceInterface
    {
        Tournament CreateTournament(string name);
        List<Better> GetBettersByTournamentId(Guid id);
        List<Better> GetBettersByTournamentName(string name);
        List<PlayerReference> GetPlayerReferencesByTournamentId(Guid id);
        List<PlayerReference> GetPlayerReferencesByTournamentName(string name);
        Tournament GetTournamentById(Guid id);
        Tournament GetTournamentByName(string name);
        bool RenameTournament(Guid id, string name);
        void Save();
        void SaveAsync();
    }
}