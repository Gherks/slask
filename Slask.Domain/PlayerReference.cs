using Slask.Domain.ObjectState;
using System;

namespace Slask.Domain
{
    public class PlayerReference : ObjectStateBase
    {
        private PlayerReference()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Guid TournamentId { get; private set; }
        public Tournament Tournament { get; private set; }

        public static PlayerReference Create(string name, Tournament tournament)
        {
            bool invalidNameGiven = name == null || name == "";
            bool noTournamentGiven = tournament == null;

            if (invalidNameGiven || noTournamentGiven)
            {
                return null;
            }

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(name);

            if (fetchedPlayerReference == null)
            {
                PlayerReference playerReference = new PlayerReference
                {
                    Name = name,
                    TournamentId = tournament.Id,
                    Tournament = tournament,
                    ObjectState = ObjectStateEnum.Added
                };

                return playerReference;
            }

            return fetchedPlayerReference;
        }

        public void RenameTo(string name)
        {
            name = name.Trim();

            if (name.Length > 0)
            {
                bool newNameIsntAlreadyInUse = Tournament.GetPlayerReferenceByPlayerName(name) == null;

                if (newNameIsntAlreadyInUse)
                {
                    Name = name;
                    MarkAsModified();
                }
            }
        }
    }
}
