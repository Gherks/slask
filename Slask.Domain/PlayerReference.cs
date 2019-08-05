using System;

namespace Slask.Domain
{
    public class PlayerReference
    {
        private PlayerReference()
        {
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static PlayerReference Create(string name, Tournament tournament)
        {
            if (tournament == null)
            {
                return null;
            }

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(name);

            if (fetchedPlayerReference == null)
            {
                PlayerReference playerReference = new PlayerReference
                {
                    Id = Guid.NewGuid(),
                    Name = name,
                    TournamentId = tournament.Id,
                    Tournament = tournament
                };

                tournament.PlayerReferences.Add(playerReference);
                return playerReference;
            }

            return fetchedPlayerReference;
        }

        public void RenameTo(string name)
        {
            if (name.Length > 0)
            {
                bool newNameIsntAlreadyInUse = Tournament.GetPlayerReferenceByPlayerName(name) == null;

                if (newNameIsntAlreadyInUse)
                {
                    Name = name;
                }
            }
        }
    }
}
