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
    public class CreateModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public CreateModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Semester Semester { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Semesters == null || Semester == null)
            {
                return Page();
            }

            // Kiểm tra trùng lặp SemesterCode
            bool isDuplicateCode = await _context.Semesters
                .AnyAsync(s => s.SemesterCode == Semester.SemesterCode);

            if (isDuplicateCode)
            {
                ModelState.AddModelError("Semester.SemesterCode", "Mã học kỳ đã tồn tại. Vui lòng chọn mã khác.");
                return Page();
            }

            // Kiểm tra trùng lặp StartDate và EndDate
            bool isDuplicateDates = await _context.Semesters
                .AnyAsync(s => s.StartDate == Semester.StartDate || s.EndDate == Semester.EndDate);

            if (isDuplicateDates)
            {
                ModelState.AddModelError(string.Empty, "Ngày bắt đầu và ngày kết thúc đã tồn tại. Vui lòng chọn ngày khác.");
                return Page();
            }

            _context.Semesters.Add(Semester);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
