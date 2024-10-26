using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.ClassCoursesManage
{
    public class DetailsModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DetailsModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

      public ClassCourse ClassCourse { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ClassCourses == null)
            {
                return NotFound();
            }

            var classcourse = await _context.ClassCourses.FirstOrDefaultAsync(m => m.ClassCourseId == id);
            if (classcourse == null)
            {
                return NotFound();
            }
            else 
            {
                ClassCourse = classcourse;
            }
            return Page();
        }
    }
}
