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

namespace ScoreManagement.Pages.AdminMenu.ClassCoursesManage
{
    [Authorize(Roles = "ADMIN")]
    public class EditModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public EditModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public ClassCourse ClassCourse { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ClassCourses == null)
            {
                return NotFound();
            }

            var classcourse =  await _context.ClassCourses.FirstOrDefaultAsync(m => m.ClassCourseId == id);
            if (classcourse == null)
            {
                return NotFound();
            }
            ClassCourse = classcourse;
           ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId");
           ViewData["CourseId"] = new SelectList(_context.Courses, "CourseId", "CourseId");
           ViewData["LecturerId"] = new SelectList(_context.Lecturers, "LecturerId", "LecturerId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ClassCourse).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassCourseExists(ClassCourse.ClassCourseId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ClassCourseExists(int id)
        {
          return (_context.ClassCourses?.Any(e => e.ClassCourseId == id)).GetValueOrDefault();
        }
    }
}
