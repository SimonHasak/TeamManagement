using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLogic.Services
{
    public interface IPlayerService
    {
        Player CreatePlayer();
        Player CreatePlayer(Player player, string[] selectedTeams);

        Task<IEnumerable<Player>> GetPlayers();
        Task<IEnumerable<Team>> GetTeams();

        Task AddPlayer(Player player);

        Task SaveChanges();

        Task RemoveTeamAssignment(TeamAssignment teamAssignment);

        Task RemovePlayer(Player player);

        Task<Player> GetPlayerWithTeams(int? id);

        Task<Player> GetPlayerWithTeamsAsNoTracking(int? id);

        Task<Player> GetPlayer(int? id);

        Task<List<AssignedTeamData>> GetPopulatedAssignedTeamData(Player player);
        Task UpdatePlayerTeams(string[] selectedTeams, Player player);

        bool PlayerExists(int id);
    }
}
