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

        public ActionResult TeacherChat(int id)
        {
            TeacherStudenViewModel viewmodel = new TeacherStudenViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewmodel.Teacher = _context.Teachers.Where(t => t.IdentityUserId == userId).FirstOrDefault();
            viewmodel.Student = _context.Students.Where(t => t.Id == id).FirstOrDefault();
            viewmodel.Parent = _context.Parents.Where(t => t.Id == viewmodel.Student.ParentId).FirstOrDefault();

            if (viewmodel.Student == null)
            {
                return RedirectToAction("SelectedClassroom");

            }

            return View(viewmodel);
        }


        public ActionResult SelectedStudent(int? id)
        {
            TeacherStudenViewModel student = new TeacherStudenViewModel();
            student.Student = _context.Students.Where(s => s.Id == id).FirstOrDefault();
            student.Classroom = _context.Classroom.Where(c => c.Id == student.Student.ClassId).FirstOrDefault();
            student.Skills = _context.Skill.Where(sk => sk.ClassId == 0).ToList();
            //var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //classroom.Teacher = _context.Teachers.Where(t => t.IdentityUserId == userId).FirstOrDefault();
            //classroom.AllStudents = _context.Students.ToList();
            //classroom.Students = _context.Students.Where(c => c.ClassId == id).ToList();
            //classroom.Classroom = _context.Classroom.Where(cs => cs.Id == id).FirstOrDefault();
            //classroom.Points = _context.Points.Where(p => p.TeacherId == classroom.Teacher.Id).ToList();

            return View(student);
        }

        public ActionResult SelectedClassroom(int? id)
        {
            TeacherClassroomViewModel classroom = new TeacherClassroomViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            classroom.Teacher = _context.Teachers.Where(t => t.IdentityUserId == userId).FirstOrDefault();
            classroom.AllStudents = _context.Students.ToList();
            classroom.Students = _context.Students.Where(c => c.ClassId == id).ToList();
            classroom.Classroom = _context.Classroom.Where(cs => cs.Id == id).FirstOrDefault();
            //classroom.Points = _context.Points.Where(p => p.TeacherId == classroom.Teacher.Id).ToList();
            classroom.Homework = _context.Homeworks.Where(hw => hw.ClassId == id && hw.Date == DateTime.Now.Date).FirstOrDefault();
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
        public IActionResult AddDailyNote()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddDailyNote(int id, [Bind("Description")] DailyNote dailyNote)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _context.Students.Where(s => s.Id == id).FirstOrDefault();
            dailyNote.StudentId = student.Id;
            dailyNote.Date = DateTime.Now.Date;
            dailyNote.Description = dailyNote.Description;
            
            if(dailyNote != null)
            {

            _context.DailyNotes.Add(dailyNote);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(SelectedStudent), new { id });

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

        [HttpGet]
        public IActionResult AddPoints()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPoints(int id, [FromForm(Name = "selectedSkill")] List<int> selectedSkills)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var student = _context.Students.FirstOrDefault(x => x.Id == id);

            foreach (var skillId in selectedSkills)
            {
                var skill = _context.Skill.FirstOrDefault(s => s.Id == skillId);
                if (skill != null)
                {
                    StudentSkill studentSkill = new StudentSkill();
                    studentSkill.ClassId = student.ClassId;
                    studentSkill.Date = DateTime.Now.Date;
                    studentSkill.Point = skill.Point;
                    studentSkill.SkillId = skill.Id;
                    studentSkill.StudentId = student.Id;

                    student.Point += skill.Point;
                    _context.StudentSkill.Add(studentSkill);
                    _context.Students.Update(student);
                }
            }
            
            _context.SaveChanges();
            return RedirectToAction(nameof(SelectedStudent), new { id });

        }

        //public IActionResult AssignHomework()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult AssignHomework(int id)
        //{
        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var homeworks = _context.Homeworks.Where(h => h.ClassId == id).ToList();
        //    foreach (var homework in homeworks)
        //    {
        //        homework.ClassId = id
        //    }

        //    return RedirectToAction(nameof(SelectedClassroom), new { id });

        //}

        public IActionResult CreateHomework()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateHomework(int id, [Bind("Name,Description")] Homework homework)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = _context.Teachers.Where(c => c.IdentityUserId == userId).FirstOrDefault();
            homework.ClassId = id;
            homework.Date = DateTime.Now.Date;
            _context.Homeworks.Add(homework);
            _context.SaveChanges();

            var startDate = DateTime.Now.Date; // 2020-08-19 00:00
            var endDate = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1); // +1 = 2020-08-20 00:00 // -1 ms => 2020-08-19 23:59:59 99999

            var data = _context.Homeworks.Where(x => x.Date >= startDate && x.Date <= endDate).ToList();

            return RedirectToAction(nameof(SelectedClassroom), new { id });

        }

        public IActionResult Attendance()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Attendance(int id, [FromForm(Name = "selectedStudent")] List<int> selectedStudents)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var studentId in selectedStudents)
            {
                Attendance attendance = new Attendance();
                var student = _context.Students.FirstOrDefault(x => x.Id == studentId);

                if (student != null)
                {
                    attendance.StudentId = student.Id;
                    attendance.Description = "absent";
                    attendance.Date = DateTime.Now.Date;
                    _context.Add(attendance);
                }
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(SelectedClassroom), new { id });

        }


        public IActionResult ResetAllPoints()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetAllPoints(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var filteredStudent = _context.Students.Where(s => s.ClassId == id).ToList();
            foreach (var student in filteredStudent)
            {
                student.Point = 0;
                _context.Update(student);
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

        //public int CalculateTotalPoint()
        //{
        //    int totalPoint = 0;

        //    for (int i = 0; i < Points.Count; i++)
        //    {
        //        totalPoint += Points[i].Point;
        //    }


        //    return totalPoint;
        //}
    }
}
