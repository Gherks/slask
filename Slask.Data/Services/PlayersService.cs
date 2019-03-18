using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slask.Domain;

namespace Slask.Data.Services
{
    public class PlayersService
    {
        private readonly SlaskContext slaskContext;

        public PlayersService(SlaskContext slaskContext)
        {
            this.slaskContext = slaskContext;
        }

        public async Task<IList<Player>> GetAll()
        {
            var players = await slaskContext.Players.Select(a => new Player()
            { 
                Id = a.Id,
                Name = a.Name
            }).ToListAsync();

            return players;
        }
    }
}
