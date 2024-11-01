using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.GradeManage
{
    [Authorize(Roles = "ADMIN")]
    public class DeleteModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DeleteModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
      public Grade Grade { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }

            var grade = await _context.Grades.FirstOrDefaultAsync(m => m.GradeId == id);

            if (grade == null)
            {
                return NotFound();
            }
            else 
            {
                Grade = grade;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Grades == null)
            {
                return NotFound();
            }
            var grade = await _context.Grades.FindAsync(id);

            if (grade != null)
            {
                Grade = grade;
                _context.Grades.Remove(Grade);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
