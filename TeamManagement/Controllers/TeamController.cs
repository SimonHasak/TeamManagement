using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace TeamManager.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamService _service;

        public TeamController(ITeamService service)
        {
            _service = service;
        }

        // GET: Team
        public async Task<IActionResult> Index()
        {
            var allTeams = (List<Team>)await _service.GetTeams();
            return View(allTeams);
        }

        // GET: Team/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var team = await _service.GetTeamWithPlayersAsNoTracking(id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Team/Create
        public async Task<IActionResult> Create()
        {
            var newTeam = _service.CreateTeam();
            ViewData["Players"] = await _service.GetPopulatedAssignedPlayerData(newTeam);
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Team team, string[] selectedPlayers)
        {
            var newTeam = _service.CreateTeam(team, selectedPlayers);

            if (ModelState.IsValid)
            {
                await _service.AddTeam(newTeam);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Teams"] = await _service.GetPopulatedAssignedPlayerData(newTeam);
            return View(newTeam);
        }

        // GET: Team/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _service.GetTeamWithPlayersAsNoTracking(id);

            if (team == null)
            {
                return NotFound();
            }
            ViewData["Teams"] = await _service.GetPopulatedAssignedPlayerData(team);
            return View(team);
        }

        // POST: Team/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedPlayers)
        {
            if (id == null)
            {
                return NotFound();
            }
            var teamToUpdate = await _service.GetTeamWithPlayers(id);

            if (await TryUpdateModelAsync<Team>(teamToUpdate, "", s => s.Name, s => s.Description))
            {
                await _service.UpdateTeamPlayers(selectedPlayers, teamToUpdate);
                try
                {
                    //_service.UpdatePlayer(playerToUpdateDTO);
                    await _service.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists. ");
                }
                return RedirectToAction(nameof(Index));
            }
            await _service.UpdateTeamPlayers(selectedPlayers, teamToUpdate);
            ViewData["Teams"] = await _service.GetPopulatedAssignedPlayerData(teamToUpdate);
            return View(teamToUpdate);
        }

        // GET: Team/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _service.GetTeamWithPlayersAsNoTracking(id);
            if (team == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again later.";
            }

            return View(team);
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _service.GetTeam(id);

            if (team == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _service.RemoveTeam(team);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }
        }

        private bool TeamExists(int id)
        {
            return _service.TeamExists(id);
        }
    }
}