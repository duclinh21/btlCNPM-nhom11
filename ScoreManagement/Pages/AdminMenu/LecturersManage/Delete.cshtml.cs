using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.LecturersManage
{
    public class DeleteModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public DeleteModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
      public Lecturer Lecturer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Lecturers == null)
            {
                return NotFound();
            }

            var lecturer = await _context.Lecturers
                .Include(s => s.Account)
                .FirstOrDefaultAsync(m => m.LecturerId == id);

            if (lecturer == null)
            {
                return NotFound();
            }
            else 
            {
                Lecturer = lecturer;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Lecturers == null)
            {
                return NotFound();
            }
            var lecturer = await _context.Lecturers.FindAsync(id);

            if (lecturer != null)
            {
                Lecturer = lecturer;
                _context.Lecturers.Remove(Lecturer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
