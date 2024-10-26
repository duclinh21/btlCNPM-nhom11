using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.LecturersManage
{
    public class EditModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public EditModel(ScoreManagement.Models.Project_PRN221Context context)
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

            // Lấy Lecturer hiện tại để chỉnh sửa
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(m => m.LecturerId == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            Lecturer = lecturer;

            // Lấy danh sách các Account không phải Admin, chưa được liên kết, và bao gồm tài khoản hiện tại của Lecturer
            var availableAccounts = _context.Accounts
                .Where(a => a.Role == "LECTURER" &&
                            (!_context.Lecturers.Any(l => l.AccountId == a.AccountId) &&
                             !_context.Students.Any(s => s.AccountId == a.AccountId) ||
                             a.AccountId == Lecturer.AccountId)) // Bao gồm AccountId của Lecturer hiện tại
                .Select(a => new SelectListItem
                {
                    Value = a.AccountId.ToString(),
                    Text = a.Username
                })
                .ToList();

            ViewData["AccountId"] = availableAccounts;

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

            _context.Attach(Lecturer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LecturerExists(Lecturer.LecturerId))
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

        private bool LecturerExists(int id)
        {
          return (_context.Lecturers?.Any(e => e.LecturerId == id)).GetValueOrDefault();
        }
    }
}
