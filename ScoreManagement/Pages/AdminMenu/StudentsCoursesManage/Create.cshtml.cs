using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.StudentsCoursesManage
{
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
        ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
        ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "LecturerId");
        ViewData["SemesterId"] = new SelectList(_context.Semesters, "SemesterId", "SemesterId");
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
    }
}
