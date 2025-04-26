using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.StudentClassesManage
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
            // Lấy danh sách ID học sinh đã được gán vào lớp
            var assignedStudentIds = _context.StudentClasses.Select(sc => sc.StudentId).ToList();

            // Lọc học sinh chưa được gán vào lớp
            var availableStudents = _context.Students
                                             .Where(s => !assignedStudentIds.Contains(s.StudentId))
                                             .ToList();

            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassCode");
            ViewData["StudentId"] = new SelectList(availableStudents, "StudentId", "StudentCode");

            return Page();
        }

        [BindProperty]
        public StudentClass StudentClass { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.StudentClasses == null || StudentClass == null)
            {
                return Page();
            }

            _context.StudentClasses.Add(StudentClass);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
