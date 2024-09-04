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
    public class AssignmentUsersController : Controller
    {
        private readonly To_Do_ListContext _context;

        public AssignmentUsersController(To_Do_ListContext context)
        {
            _context = context;
        }

        // GET: AssignmentUsers
        public async Task<IActionResult> Index()
        {
            var to_Do_ListContext = _context.AssignmentUser.Include(a => a.Department).Include(a => a.User).Include(a => a.Vacancy);
            return View(await to_Do_ListContext.ToListAsync());
        }

        // GET: AssignmentUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentUser = await _context.AssignmentUser
                .Include(a => a.Department)
                .Include(a => a.User)
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentUser == null)
            {
                return NotFound();
            }

            return View(assignmentUser);
        }

        // GET: AssignmentUsers/Create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["VacancyId"] = new SelectList(_context.Vacancy, "Id", "Id");
            return View();
        }

        // POST: AssignmentUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,DepartmentId,VacancyId,AssignedDate,MovedDate")] AssignmentUser assignmentUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(assignmentUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", assignmentUser.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentUser.UserId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancy, "Id", "Id", assignmentUser.VacancyId);
            return View(assignmentUser);
        }

        // GET: AssignmentUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentUser = await _context.AssignmentUser.FindAsync(id);
            if (assignmentUser == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", assignmentUser.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentUser.UserId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancy, "Id", "Id", assignmentUser.VacancyId);
            return View(assignmentUser);
        }

        // POST: AssignmentUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,DepartmentId,VacancyId,AssignedDate,MovedDate")] AssignmentUser assignmentUser)
        {
            if (id != assignmentUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignmentUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentUserExists(assignmentUser.Id))
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
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Id", assignmentUser.DepartmentId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", assignmentUser.UserId);
            ViewData["VacancyId"] = new SelectList(_context.Vacancy, "Id", "Id", assignmentUser.VacancyId);
            return View(assignmentUser);
        }

        // GET: AssignmentUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignmentUser = await _context.AssignmentUser
                .Include(a => a.Department)
                .Include(a => a.User)
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (assignmentUser == null)
            {
                return NotFound();
            }

            return View(assignmentUser);
        }

        // POST: AssignmentUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignmentUser = await _context.AssignmentUser.FindAsync(id);
            if (assignmentUser != null)
            {
                _context.AssignmentUser.Remove(assignmentUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignmentUserExists(int id)
        {
            return _context.AssignmentUser.Any(e => e.Id == id);
        }
    }
}
