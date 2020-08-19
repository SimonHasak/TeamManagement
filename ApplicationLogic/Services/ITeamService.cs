using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLogic.Services
{
    public interface ITeamService
    {
        Team CreateTeam();
        Team CreateTeam(Team team, string[] selectedPlayers);
        Task<IEnumerable<Player>> GetPlayers();
        Task<IEnumerable<Team>> GetTeams();
        Task AddTeam(Team team);
        Task SaveChanges();
        Task RemoveTeamAssignment(TeamAssignment teamAssignment);
        Task RemoveTeam(Team team);
        Task<Team> GetTeamWithPlayers(int? id);
        Task<Team> GetTeamWithPlayersAsNoTracking(int? id);
        Task<Team> GetTeam(int? id);
        Task<List<AssignedTeamData>> GetPopulatedAssignedPlayerData(Team team);
        Task UpdateTeamPlayers(string[] selectedPlayers, Team team);
        bool TeamExists(int id);
    }
}
