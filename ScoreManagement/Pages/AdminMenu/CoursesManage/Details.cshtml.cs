using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.CoursesManage
{
    public class DetailsModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DetailsModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

      public Course Course { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            else 
            {
                Course = course;
            }
            return Page();
        }
    }
}
