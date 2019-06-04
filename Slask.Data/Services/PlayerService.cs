using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slask.Domain;

namespace Slask.Data.Services
{
    public class PlayerService
    {
        private SlaskContext _slaskContext;

        public PlayerService(SlaskContext slaskContext)
        {
            _slaskContext = slaskContext;
        }

        public async Task<IList<Player>> GetAll()
        {
            var players = await _slaskContext.Players.Select(currentPlayer => new Player()
            { 
                Id = currentPlayer.Id,
                Name = currentPlayer.Name
            }).ToListAsync();

            return players;
        }

        public Player AddPlayer(string name)
        {
            Player player = _slaskContext.Players.Add(new Player
            {
                Name = name
            }).Entity;

            return player;
        }
    }
}
