using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace TeamManager.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IPlayerService _service;

        public PlayerController(IPlayerService service)
        {
            _service = service;
        }

        // GET: Player
        public async Task<IActionResult> Index()
        {
            var allPlayers = (List<Player>) await _service.GetPlayers();
            return View(allPlayers);
        }

        // GET: Player/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var player = await _service.GetPlayerWithTeamsAsNoTracking(id);

            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public async Task<IActionResult> Create()
        {
            var newPlayer = _service.CreatePlayer();
            ViewData["Teams"] = await _service.GetPopulatedAssignedTeamData(newPlayer);
            return View();
        }

        // POST: Player/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] Player player, string[] selectedTeams)
        {
            var newPlayer = _service.CreatePlayer(player, selectedTeams);

            if (ModelState.IsValid)
            {
                await _service.AddPlayer(newPlayer);
                return RedirectToAction(nameof(Index));
            }
            ViewData["Teams"] = await _service.GetPopulatedAssignedTeamData(newPlayer);
            return View(newPlayer);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _service.GetPlayerWithTeamsAsNoTracking(id);

            if (player == null)
            {
                return NotFound();
            }
            ViewData["Teams"] = await _service.GetPopulatedAssignedTeamData(player);
            return View(player);
        }

        // POST: Player/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedTeams)
        {
            if (id == null)
            {
                return NotFound();
            }
            var playerToUpdate = await _service.GetPlayerWithTeams(id);

            if (await TryUpdateModelAsync<Player>(playerToUpdate, "", s => s.Name, s => s.Description))
            {
                await _service.UpdatePlayerTeams(selectedTeams, playerToUpdate);
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
            await _service.UpdatePlayerTeams(selectedTeams, playerToUpdate);
            ViewData["Teams"] = await _service.GetPopulatedAssignedTeamData(playerToUpdate);
            return View(playerToUpdate);
        }

        // GET: Player/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _service.GetPlayerWithTeamsAsNoTracking(id);
            if (player == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again later.";
            }

            return View(player);
        }

        // POST: Player/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _service.GetPlayer(id);

            if (player == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _service.RemovePlayer(player);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }
        }

        private bool PlayerExists(int id)
        {
            return _service.PlayerExists(id);
        }
    }
}