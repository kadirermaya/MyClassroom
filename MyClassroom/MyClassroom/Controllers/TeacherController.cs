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
    public class TeacherController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public ActionResult Index()
        {
            TeacherIndexViewModel viewmodel = new TeacherIndexViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewmodel.Teacher = _context.Teachers.Where(t => t.IdentityUserId == userId).FirstOrDefault();


            if (viewmodel.Teacher == null)
            {
                return RedirectToAction("Create");

            }

            viewmodel.MyClassrooms = _context.Classroom.Where(c => c.TeacherID == viewmodel.Teacher.Id).ToList();
            return View(viewmodel);
        }

        public ActionResult SelectedClassroom(int? id)
        {
            TeacherClassroomViewModel classroom = new TeacherClassroomViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            classroom.Teacher = _context.Teachers.Where(t => t.IdentityUserId == userId).FirstOrDefault();
            classroom.AllStudents = _context.Students.ToList();
            classroom.Students = _context.Students.Where(c => c.ClassId == id).ToList();
            classroom.Classroom = _context.Classroom.Where(cs => cs.Id == id).FirstOrDefault();
            classroom.Points = _context.Points.Where(p => p.TeacherId == classroom.Teacher.Id).ToList();
            classroom.Student = _context.Students.Where(c => c.ClassId == id).FirstOrDefault();
            classroom.Student.Point = 0;

            return View(classroom);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.Include(t => t.IdentityUser).FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        [HttpGet]
        public IActionResult AddStudent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent(int id, [FromForm(Name = "selectedStudent")] List<int> selectedStudents)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var studentId in selectedStudents)
            {
                var student = _context.Students.FirstOrDefault(x => x.Id == studentId);
                if (student != null)
                {
                    student.ClassId = id;
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(SelectedClassroom), new { id });

        }

        //GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }



        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,IdentityUserId,FirstName,LastName")] Teacher teacher)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            teacher.IdentityUserId = userId;


            _context.Add(teacher);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult CreateClassroom()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateClassroom([Bind("TeacherId,Name")] Classroom classroom)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = _context.Teachers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            classroom.TeacherID = teacher.Id;



            _context.Classroom.Add(classroom);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", teacher.IdentityUserId);
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentityUserId,FirstName,LastName")] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.Id))
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
            ViewData["IdentityUserId"] = new SelectList(_context.Users, "Id", "Id", teacher.IdentityUserId);
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teachers
                .Include(t => t.IdentityUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teachers.Any(e => e.Id == id);
        }

        public int CalculateTotalPoint(List<Points> Points)
        {
            int totalPoint = 0;

            for (int i = 0; i < Points.Count; i++)
            {
                totalPoint += Points[i].Point;
            }
   
            
            return totalPoint;
        }
    }
}
