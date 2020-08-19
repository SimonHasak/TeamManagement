using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Player>> GetPlayers();
        Task<IEnumerable<Team>> GetTeams();
        Task AddTeam(Team team);
        Task SaveChanges();
        Task RemoveTeamAssignment(TeamAssignment teamAssignment);
        Task RemoveTeam(Team team);
        Task<Team> GetTeamWithPlayers(int? id);
        Task<Team> GetTeamWithPlayersAsNoTracking(int? id);
        Task<Team> GetTeam(int? id);
        bool TeamExists(int id);
    }
}
