using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.SemestersManage
{
    public class DeleteModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DeleteModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
      public Semester Semester { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }

            var semester = await _context.Semesters.FirstOrDefaultAsync(m => m.SemesterId == id);

            if (semester == null)
            {
                return NotFound();
            }
            else 
            {
                Semester = semester;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Semesters == null)
            {
                return NotFound();
            }
            var semester = await _context.Semesters.FindAsync(id);

            if (semester != null)
            {
                Semester = semester;
                _context.Semesters.Remove(Semester);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
