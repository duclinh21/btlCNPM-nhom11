using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.ClassesManage
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
        public Class Class { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }

            var selectedClass = await _context.Classes
                .Include(s => s.Semester)
                .FirstOrDefaultAsync(m => m.ClassId == id);

            if (selectedClass == null)
            {
                return NotFound();
            }
            else
            {
                Class = selectedClass;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Classes == null)
            {
                return NotFound();
            }
            var selectedClass = await _context.Classes.FindAsync(id);

            if (selectedClass != null)
            {
                Class = selectedClass;

                try
                {
                    _context.Classes.Remove(Class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty, "Không thể xóa lớp học này do có dữ liệu liên quan trong bảng khác.");
                    return Page();
                }
                
            }

            return RedirectToPage("./Index");
        }
    }
}
