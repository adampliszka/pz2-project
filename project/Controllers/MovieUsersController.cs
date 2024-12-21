using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;

namespace project.Controllers
{
    public class MovieUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MovieUsers
        public async Task<IActionResult> Index()
        {
              return _context.MovieUsers != null ? 
                          View(await _context.MovieUsers.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Users'  is null.");
        }

        // GET: MovieUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.MovieUsers == null)
            {
                return NotFound();
            }

            var movieUser = await _context.MovieUsers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movieUser == null)
            {
                return NotFound();
            }

            return View(movieUser);
        }

        // GET: MovieUsers/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovieUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create([Bind("ID,forename,surname,email")] MovieUser movieUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movieUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieUser);
        }

        // GET: MovieUsers/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MovieUsers == null)
            {
                return NotFound();
            }

            var movieUser = await _context.MovieUsers.FindAsync(id);
            if (movieUser == null)
            {
                return NotFound();
            }
            return View(movieUser);
        }

        // POST: MovieUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,forename,surname,email")] MovieUser movieUser)
        {
            if (id != movieUser.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieUserExists(movieUser.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movieUser);
        }

        // GET: MovieUsers/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MovieUsers == null)
            {
                return NotFound();
            }

            var movieUser = await _context.MovieUsers
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movieUser == null)
            {
                return NotFound();
            }

            return View(movieUser);
        }

        // POST: MovieUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MovieUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
            }
            var movieUser = await _context.MovieUsers.FindAsync(id);
            if (movieUser != null)
            {
                _context.MovieUsers.Remove(movieUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieUserExists(int id)
        {
          return (_context.MovieUsers?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
