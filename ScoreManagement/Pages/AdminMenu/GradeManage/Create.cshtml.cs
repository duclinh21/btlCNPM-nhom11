using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.GradeManage
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
        ViewData["StudentCourseId"] = new SelectList(_context.StudentsCourses, "StudentCourseId", "StudentCourseId");
            return Page();
        }

        [BindProperty]
        public Grade Grade { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Grades == null || Grade == null)
            {
                return Page();
            }
            Grade.CalculateAverageAndStatus();
            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
