using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.GradeManage
{
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Grade> Grade { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Grades != null)
            {
                Grade = await _context.Grades
                    .Include(g => g.StudentCourse)
                    .ThenInclude(sc => sc.Student) // Include thêm bảng Students thông qua StudentCourse
                    .ToListAsync();
            }
        }

    }
}
