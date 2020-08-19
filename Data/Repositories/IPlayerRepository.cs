using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IPlayerRepository
    {
        Task<IEnumerable<Player>> GetPlayers();
        Task<IEnumerable<Team>> GetTeams();
        Task AddPlayer(Player player);
        Task SaveChanges();
        Task RemoveTeamAssignment(TeamAssignment teamAssignment);
        Task RemovePlayer(Player player);
        Task<Player> GetPlayerWithTeams(int? id);
        Task<Player> GetPlayerWithTeamsAsNoTracking(int? id);
        Task<Player> GetPlayer(int? id);
        bool PlayerExists(int id);
    }
}
