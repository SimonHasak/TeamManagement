using Data.DataContext;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{ 
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DatabaseContext _context;

        public PlayerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task AddPlayer(Player player)
        {
            _context.Players.Add(player);
            await this.SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTeamAssignment(TeamAssignment teamAssignment)
        {
            _context.Remove(teamAssignment);
            await this.SaveChanges();
        }

        public async Task RemovePlayer(Player player)
        {
            _context.Players.Remove(player);
            await this.SaveChanges();
        }

        public async Task<Player> GetPlayerWithTeams(int? id)
        {
            return await _context.Players
                .Include(t => t.TeamAssignments)
                    .ThenInclude(t => t.Team)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        public async Task<Player> GetPlayerWithTeamsAsNoTracking(int? id)
        {
            return await _context.Players
                .Include(s => s.TeamAssignments)
                    .ThenInclude(e => e.Team)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Player> GetPlayer(int? id)
        {
            return await _context.Players
                .Include(t => t.TeamAssignments)
                .SingleAsync(t => t.Id == id);
        }

        public async Task UpdatePlayer(Player player)
        {
            _context.Entry(player).State = EntityState.Modified;
            await SaveChanges();
        }

        public bool PlayerExists(int id)
        {
            return _context.Players.Any(p => p.Id == id);
        }
    }
}
