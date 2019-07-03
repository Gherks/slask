using System;

namespace Slask.Domain
{
    public class PlayerNameReference
    {
        private PlayerNameReference()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public void RenameTo(string name)
        {
            if (name.Length > 0)
            {
                bool newNameIsntAlreadyInUse = Tournament.GetPlayerNameReferenceByPlayerName(name) == null;

                if (newNameIsntAlreadyInUse)
                {
                    Name = name;
                }
            }
        }

        public static PlayerNameReference Create(string name, Tournament tournament)
        {
            PlayerNameReference fetchedPlayerNameReference = tournament.GetPlayerNameReferenceByPlayerName(name);

            if(fetchedPlayerNameReference == null)
            {
                PlayerNameReference playerNameReference = new PlayerNameReference
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    TournamentId = tournament.Id,
                    Tournament = tournament
                };

                tournament.PlayerNameReferences.Add(playerNameReference);
                return playerNameReference;
            }

            return fetchedPlayerNameReference;
        }
    }
}
