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
    public class DetailsModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DetailsModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

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
    }
}
