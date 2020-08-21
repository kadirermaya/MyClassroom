using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyClassroom.Data;
using MyClassroom.Models;

namespace MyClassroom.Controllers
{
    public class ParentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Parents
        public IActionResult Index()
        {
            //ParentIndexView parentIndex = new ParentIndexView();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parent = _context.Parents.Where(p => p.IdentityUserId == userId).FirstOrDefault();
            if (parent == null)
            {
                return RedirectToAction("Create");
            }
            //parentIndex.Parent = _context.Parents.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            //parentIndex.Student = _context.Students.Where(s => s.ParentId == parentIndex.Parent.Id).FirstOrDefault();
            //parentIndex.DailyNote = _context.DailyNotes.Where(d => d.StudentId == parentIndex.Student.Id && d.Date == DateTime.Now.Date).FirstOrDefault();

            return View(parent);
        }

        public IActionResult ParentChat()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parent = _context.Parents.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            TeacherStudenViewModel viewmodel = new TeacherStudenViewModel();
            viewmodel.Teacher = _context.Teachers.Where(t => t.IdentityUserId == userId).FirstOrDefault();
            viewmodel.Student = _context.Students.Where(t => t.Id == parent.Id).FirstOrDefault();
            viewmodel.Parent = _context.Parents.Where(t => t.Id == viewmodel.Student.ParentId).FirstOrDefault();

            if (viewmodel.Student == null)
            {
                return RedirectToAction("SelectedClassroom");

            }

            return View(viewmodel);
        }

        // GET: Parents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .Include(p => p.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        // GET: Parents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,IdentityUserId,FirstName,LastName")] Parent parent, string StudentFirstName, string StudentLastName)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            parent.IdentityUserId = userId;
            var student = _context.Students.Where(s => s.FirstName == StudentFirstName && s.LastName == StudentLastName).FirstOrDefault();
            if (student == null)
            {
                return RedirectToAction(nameof(Create));
            }
            student.ParentId = parent.Id;

            _context.Students.Update(student);
            _context.Add(parent);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Parents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", parent.IdentityUserId);
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityUserId,FirstName,LastName")] Parent parent)
        {
            if (id != parent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentExists(parent.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", parent.IdentityUserId);
            return View(parent);
        }

        // GET: Parents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .Include(p => p.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }

        //public void UpdateParentId(string firstName, string lastName, int ParentId)
        //{
        //    var student = _context.Students.Where(s => s.FirstName == firstName && s.LastName == lastName).FirstOrDefault();
        //    if(student == null)
        //    {

        //    }
        //    student.ParentId = ParentId;
        //    _context.Students.Update(student);
        //    _context.SaveChanges();
        //}
    }
}
