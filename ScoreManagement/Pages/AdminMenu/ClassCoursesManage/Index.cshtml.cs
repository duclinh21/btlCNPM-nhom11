using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.ClassCoursesManage
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<ClassCourse> ClassCourse { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.ClassCourses != null)
            {
                ClassCourse = await _context.ClassCourses
                .Include(c => c.Class)
                .Include(c => c.Course)
                .Include(c => c.Lecturer).ToListAsync();
            }
        }
    }
}
