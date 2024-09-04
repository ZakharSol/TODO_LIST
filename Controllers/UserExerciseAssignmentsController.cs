using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using To_Do_List.Data;
using To_Do_List.Models;

namespace To_Do_List.Controllers
{
    [Authorize]
    public class UserExerciseAssignmentsController : Controller
    {
        private readonly To_Do_ListContext _context;

        public UserExerciseAssignmentsController(To_Do_ListContext context)
        {
            _context = context;
        }

        // GET: UserExerciseAssignments
        public async Task<IActionResult> Index()
        {
            var to_Do_ListContext = _context.UserExerciseAssignment.Include(u => u.User);
            return View(await to_Do_ListContext.ToListAsync());
        }

        // GET: UserExerciseAssignments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExerciseAssignment = await _context.UserExerciseAssignment
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExerciseAssignment == null)
            {
                return NotFound();
            }

            return View(userExerciseAssignment);
        }

        // GET: UserExerciseAssignments/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UserExerciseAssignments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TaskId,AssignedDate")] UserExerciseAssignment userExerciseAssignment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userExerciseAssignment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userExerciseAssignment.UserId);
            return View(userExerciseAssignment);
        }

        // GET: UserExerciseAssignments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExerciseAssignment = await _context.UserExerciseAssignment.FindAsync(id);
            if (userExerciseAssignment == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userExerciseAssignment.UserId);
            return View(userExerciseAssignment);
        }

        // POST: UserExerciseAssignments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TaskId,AssignedDate")] UserExerciseAssignment userExerciseAssignment)
        {
            if (id != userExerciseAssignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userExerciseAssignment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExerciseAssignmentExists(userExerciseAssignment.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userExerciseAssignment.UserId);
            return View(userExerciseAssignment);
        }

        // GET: UserExerciseAssignments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userExerciseAssignment = await _context.UserExerciseAssignment
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userExerciseAssignment == null)
            {
                return NotFound();
            }

            return View(userExerciseAssignment);
        }

        // POST: UserExerciseAssignments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userExerciseAssignment = await _context.UserExerciseAssignment.FindAsync(id);
            if (userExerciseAssignment != null)
            {
                _context.UserExerciseAssignment.Remove(userExerciseAssignment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExerciseAssignmentExists(int id)
        {
            return _context.UserExerciseAssignment.Any(e => e.Id == id);
        }
    }
}
