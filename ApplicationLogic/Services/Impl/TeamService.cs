using Core.Models;
using Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLogic.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }
        public async Task AddTeam(Team team)
        {
            await _teamRepository.AddTeam(team);
        }

        public Team CreateTeam()
        {
            var newTeam = new Team();
            return newTeam;
        }

        public Team CreateTeam(Team team, string[] selectedPlayers)
        {
            if (selectedPlayers != null)
            {
                team.PlayerAssignments.Clear();
                foreach (var player in selectedPlayers)
                {
                    var playerToAdd = new TeamAssignment { PlayerId = int.Parse(player), TeamId = team.Id };
                    team.PlayerAssignments.Add(playerToAdd);
                }
            }
            return team;
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            var players = await _teamRepository.GetPlayers();
            return players;
        }

        public async Task<List<AssignedTeamData>> GetPopulatedAssignedPlayerData(Team team)
        {
            var teamPlayers = new HashSet<int>(team.PlayerAssignments.Select(t => t.TeamId));
            var viewModel = new List<AssignedTeamData>();
            foreach (var player in await GetPlayers())
            {
                viewModel.Add(new AssignedTeamData
                {
                    Id = player.Id,
                    Title = player.Name,
                    IsAssigned = teamPlayers.Contains(player.Id)
                });
            }
            return viewModel;
        }

        public async Task<Team> GetTeam(int? id)
        {
            var team = await _teamRepository.GetTeam(id);
            return team;
        }

        public async Task<IEnumerable<Team>> GetTeams()
        {
            var teams = await _teamRepository.GetTeams();
            return teams;
        }

        public async Task<Team> GetTeamWithPlayers(int? id)
        {
            var team = await _teamRepository.GetTeamWithPlayers(id);
            return team;
        }

        public async Task<Team> GetTeamWithPlayersAsNoTracking(int? id)
        {
            var team = await _teamRepository.GetTeamWithPlayersAsNoTracking(id);
            return team;
        }

        public async Task RemoveTeam(Team team)
        {
            await _teamRepository.RemoveTeam(team);
        }

        public async Task RemoveTeamAssignment(TeamAssignment teamAssignment)
        {
            await _teamRepository.RemoveTeamAssignment(teamAssignment);
        }

        public async Task SaveChanges()
        {
            await _teamRepository.SaveChanges();
        }

        public bool TeamExists(int id)
        {
            return _teamRepository.TeamExists(id);
        }

        public async Task UpdateTeamPlayers(string[] selectedPlayers, Team team)
        {
            if (selectedPlayers == null)
            {
                team.PlayerAssignments = new List<TeamAssignment>();
                return;
            }

            var selectedPlayersHS = new HashSet<string>(selectedPlayers);
            var teamPlayers = new HashSet<int>(team.PlayerAssignments.Select(t => t.Team.Id));
            foreach (var player in await GetPlayers())
            {
                if (selectedPlayersHS.Contains(team.Id.ToString()))
                {
                    if (!teamPlayers.Contains(team.Id))
                    {
                        player.TeamAssignments.Add(new TeamAssignment { PlayerId = player.Id, TeamId = team.Id });
                    }
                }
                else
                {
                    if (teamPlayers.Contains(team.Id))
                    {
                        TeamAssignment playerToRemove = player.TeamAssignments.FirstOrDefault(t => t.TeamId == team.Id);
                        await RemoveTeamAssignment(playerToRemove);
                    }
                }
            }
        }
    }
}
