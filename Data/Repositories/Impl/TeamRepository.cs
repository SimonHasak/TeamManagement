using Data.DataContext;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Data.Repositories.Impl
{
    public class TeamRepository : ITeamRepository
    {
        private readonly DatabaseContext _context;
        public TeamRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task AddTeam(Team team)
        {
            _context.Teams.Add(team);
            await this.SaveChanges();
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        public async Task<Team> GetTeam(int? id)
        {
            return await _context.Teams
                .Include(p => p.PlayerAssignments)
                .SingleAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetTeamWithPlayers(int? id)
        {
            return await _context.Teams
                .Include(p => p.PlayerAssignments)
                    .ThenInclude(p => p.Player)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Team> GetTeamWithPlayersAsNoTracking(int? id)
        {
            return await _context.Teams
                .Include(p => p.PlayerAssignments)
                    .ThenInclude(p => p.Player)
                .AsNoTracking() 
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task RemoveTeam(Team team)
        {
            _context.Teams.Remove(team);
            await this.SaveChanges();
        }

        public async Task RemoveTeamAssignment(TeamAssignment teamAssignment)
        {
            _context.Remove(teamAssignment);
            await this.SaveChanges();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public bool TeamExists(int id)
        {
            return _context.Teams.Any(p => p.Id == id);
        }
    }
}
