using Slask.Domain;
using Slask.Domain.Rounds;

namespace Slask.Persistence.Services
{
    public class RoundService
    {
        private readonly SlaskContext _slaskContext;

        public RoundService(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public PlayerReference RegisterPlayerReferenceToRound(RoundBase round, string name)
        {
            PlayerReference playerReference = round.RegisterPlayerReference(name);

            if (playerReference != null)
            {
                _slaskContext.Add(playerReference);
            }

            return playerReference;
        }

        public void Save()
        {
            _slaskContext.SaveChanges();
        }
    }
}
