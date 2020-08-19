using Data.Repositories;
using Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AutoMapper;

namespace ApplicationLogic.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _repository;

        public PlayerService(IPlayerRepository repository)
        {
            _repository = repository;
        }

        public Player CreatePlayer()
        {
            var newPlayerDTO = new Player();
            return newPlayerDTO;
        }

        public Player CreatePlayer(Player player, string[] selectedTeams)
        {
            if (selectedTeams != null)
            {
                player.TeamAssignments.Clear();
                foreach (var team in selectedTeams)
                {
                    var teamToAdd = new TeamAssignment { PlayerId = player.Id, TeamId = int.Parse(team) };
                    player.TeamAssignments.Add(teamToAdd);
                }
            }
            return player;
        }

        public async Task<IEnumerable<Player>> GetPlayers()
        {
            var players = await _repository.GetPlayers();
            return players;
        }
        public async Task<IEnumerable<Team>> GetTeams()
        {
            var teams = await _repository.GetTeams();
            return teams;
        }

        public async Task AddPlayer(Player player)
        {
            await _repository.AddPlayer(player);
        }

        public async Task SaveChanges()
        {
            await _repository.SaveChanges();
        }

        public async Task RemoveTeamAssignment(TeamAssignment teamAssignment)
        {
            await _repository.RemoveTeamAssignment(teamAssignment);
        }

        public async Task RemovePlayer(Player player)
        {
            await _repository.RemovePlayer(player);
        }

        public async Task<Player> GetPlayerWithTeams(int? id)
        {
            var player =  await _repository.GetPlayerWithTeams(id);
            return player;
        }

        public async Task<Player> GetPlayerWithTeamsAsNoTracking(int? id)
        {
            var player =  await _repository.GetPlayerWithTeamsAsNoTracking(id);
            return player;
        }

        public async Task<Player> GetPlayer(int? id)
        {
            return await _repository.GetPlayer(id);
        }

        public bool PlayerExists(int id)
        {
            return _repository.PlayerExists(id);
        }

        //-------------------private---------------------

        public async Task<List<AssignedTeamData>> GetPopulatedAssignedTeamData(Player player)
        {
            var playerTeams = new HashSet<int>(player.TeamAssignments.Select(t => t.TeamId));
            var viewModel = new List<AssignedTeamData>();
            foreach (var team in await GetTeams())
            {
                viewModel.Add(new AssignedTeamData
                {
                    Id = team.Id,
                    Title = team.Name,
                    IsAssigned = playerTeams.Contains(team.Id)
                });
            }
            return viewModel;
        }

        public async Task UpdatePlayerTeams(string[] selectedTeams, Player player)
        {
            if (selectedTeams == null)
            {
                player.TeamAssignments = new List<TeamAssignment>();
                return;
            }

            var selectedTeamsHS = new HashSet<string>(selectedTeams);
            var playerTeams = new HashSet<int>(player.TeamAssignments.Select(t => t.Team.Id));
            foreach (var team in await GetTeams())
            {
                if (selectedTeamsHS.Contains(team.Id.ToString()))
                {
                    if (!playerTeams.Contains(team.Id))
                    {
                        player.TeamAssignments.Add(new TeamAssignment { PlayerId = player.Id, TeamId = team.Id });
                    }
                }
                else
                {
                    if (playerTeams.Contains(team.Id))
                    {
                        TeamAssignment teamToRemoveDTO = player.TeamAssignments.FirstOrDefault(t => t.TeamId == team.Id);
                        await RemoveTeamAssignment(teamToRemoveDTO);
                    }
                }
            }
        }

    }
}
