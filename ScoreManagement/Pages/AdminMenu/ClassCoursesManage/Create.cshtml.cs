using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.ClassCoursesManage
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
        ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassCode");
        ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseName");
        ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "LecturerName");
            return Page();
        }

        [BindProperty]
        public ClassCourse ClassCourse { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.ClassCourses == null || ClassCourse == null)
            {
                return Page();
            }

            _context.ClassCourses.Add(ClassCourse);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
