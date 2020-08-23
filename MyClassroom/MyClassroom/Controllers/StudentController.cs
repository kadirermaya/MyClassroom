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
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public IActionResult Index()
        {
            StudentIndexView studentView = new StudentIndexView();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            studentView.Student = _context.Students.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            studentView.Homework = _context.Homeworks.Where(s => s.ClassId == studentView.Student.ClassId && s.Date == DateTime.Now.Date).FirstOrDefault();

            if (studentView.Student == null)
            {
                return RedirectToAction("Create");

            }
            return View(studentView);
        }

        public IActionResult Shop()
        {
            Student student = new Student();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            student = _context.Students.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            

            if (student == null)
            {
                return RedirectToAction("Create");

            }
            return View(student);
        }

        public IActionResult StudentChat()
        {
            TeacherStudenViewModel viewmodel = new TeacherStudenViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewmodel.Student = _context.Students.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            if (viewmodel.Student == null)
            {
                return RedirectToAction("Create");

            }

            return View(viewmodel);
        }


        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.Include(s => s.IdentityUser).FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,IdentityUserId,ClassId,ParentId,FirstName,LastName,Point")] Student student)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            student.IdentityUserId = userId;

            _context.Add(student);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", student.IdentityUserId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityUserId,ClassId,ParentId,FirstName,LastName")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", student.IdentityUserId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        public IActionResult UsePoint()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UsePoint(int id, int point)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _context.Students.Where(s => s.Id == id).FirstOrDefault();

            student.Point -= point;

            _context.Students.Update(student);
            _context.SaveChanges();
            return RedirectToAction("Shop");
        }


    }
}
