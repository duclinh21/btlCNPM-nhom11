using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.StudentsCoursesManage
{
    [Authorize(Roles = "ADMIN")]
    public class CreateModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public CreateModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId");
        ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName");
        ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "LecturerName");
        ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterCode");
        ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return Page();
        }

        [BindProperty]
        public StudentsCourse StudentsCourse { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.StudentsCourses == null || StudentsCourse == null)
            {
                return Page();
            }

            _context.StudentsCourses.Add(StudentsCourse);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
        public async Task<JsonResult> OnGetStudentInfo(int studentId)
        {
            var student = await _context.Students
                .Where(s => s.StudentId == studentId)
                .Select(s => new { s.StudentCode, s.FullName })
                .FirstOrDefaultAsync();

            return new JsonResult(student);
        }
        public async Task<JsonResult> OnGetClassCode(int classId)
        {
            var classCode = await _context.Classes
                .Where(c => c.ClassId == classId)
                .Select(c => c.ClassCode)
                .FirstOrDefaultAsync();

            return new JsonResult(classCode);
        }
    }
}
