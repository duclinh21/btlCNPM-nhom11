using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.StudentsCoursesManage
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<StudentsCourse> StudentsCourse { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.StudentsCourses != null)
            {
                StudentsCourse = await _context.StudentsCourses
                .Include(s => s.Class)
                .Include(s => s.Course)
                .Include(s => s.Lecturer)
                .Include(s => s.Semester)
                .Include(s => s.Student).ToListAsync();
            }
        }
    }
}
