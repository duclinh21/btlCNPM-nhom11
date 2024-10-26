using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.StudentClassesManage
{
    public class CreateModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public CreateModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassId");
        ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
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
