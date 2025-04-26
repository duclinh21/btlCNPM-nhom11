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

namespace ScoreManagement.Pages.AdminMenu.StudentClassesManage
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
        public StudentClass StudentClass { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.StudentClasses == null)
            {
                return NotFound();
            }

            var studentclass = await _context.StudentClasses.FirstOrDefaultAsync(m => m.StudentClassId == id);
            if (studentclass == null)
            {
                return NotFound();
            }

            StudentClass = studentclass;

            // Lấy danh sách ID của các học sinh đã được gán vào lớp, ngoại trừ học sinh hiện tại
            var assignedStudentIds = _context.StudentClasses
                                             .Where(sc => sc.StudentClassId != id)
                                             .Select(sc => sc.StudentId)
                                             .ToList();

            // Lọc các học sinh chưa được gán hoặc là học sinh hiện tại
            var availableStudents = _context.Students
                                             .Where(s => !assignedStudentIds.Contains(s.StudentId) || s.StudentId == studentclass.StudentId)
                                             .ToList();

            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "ClassCode");
            ViewData["StudentId"] = new SelectList(availableStudents, "StudentId", "StudentCode");

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

            _context.Attach(StudentClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentClassExists(StudentClass.StudentClassId))
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

        private bool StudentClassExists(int id)
        {
          return (_context.StudentClasses?.Any(e => e.StudentClassId == id)).GetValueOrDefault();
        }
    }
}
