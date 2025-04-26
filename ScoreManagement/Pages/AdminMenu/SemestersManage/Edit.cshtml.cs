using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.SemestersManage
{
    [Authorize(Roles = "ADMIN")]
    public class EditModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public EditModel(ScoreManagement.Models.Project_PRN221Context context)
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

            var semester =  await _context.Semesters.FirstOrDefaultAsync(m => m.SemesterId == id);
            if (semester == null)
            {
                return NotFound();
            }
            Semester = semester;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra trùng lặp SemesterCode
            bool isDuplicateCode = await _context.Semesters
                .AnyAsync(s => s.SemesterCode == Semester.SemesterCode && s.SemesterId != Semester.SemesterId);

            if (isDuplicateCode)
            {
                ModelState.AddModelError("Semester.SemesterCode", "Mã học kỳ đã tồn tại. Vui lòng chọn mã khác.");
                return Page();
            }

            // Kiểm tra trùng lặp StartDate hoặc EndDate
            bool isDuplicateDate = await _context.Semesters
                .AnyAsync(s => (s.StartDate == Semester.StartDate || s.EndDate == Semester.EndDate) && s.SemesterId != Semester.SemesterId);

            if (isDuplicateDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày bắt đầu hoặc ngày kết thúc đã tồn tại. Vui lòng chọn ngày khác.");
                return Page();
            }

            _context.Attach(Semester).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SemesterExists(Semester.SemesterId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool SemesterExists(int id)
        {
          return (_context.Semesters?.Any(e => e.SemesterId == id)).GetValueOrDefault();
        }
    }
}
