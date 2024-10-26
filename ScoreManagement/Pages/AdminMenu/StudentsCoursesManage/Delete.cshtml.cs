using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.StudentsCoursesManage
{
    public class DeleteModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DeleteModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
      public StudentsCourse StudentsCourse { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudentsCourses == null)
            {
                return NotFound();
            }

            var studentscourse = await _context.StudentsCourses.FirstOrDefaultAsync(m => m.StudentCourseId == id);

            if (studentscourse == null)
            {
                return NotFound();
            }
            else 
            {
                StudentsCourse = studentscourse;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.StudentsCourses == null)
            {
                return NotFound();
            }
            var studentscourse = await _context.StudentsCourses.FindAsync(id);

            if (studentscourse != null)
            {
                StudentsCourse = studentscourse;
                _context.StudentsCourses.Remove(StudentsCourse);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
