using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.LecturersManage
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
            // Lấy danh sách các Account không phải là Admin và chưa liên kết với Lecturer hoặc Student
            var availableAccounts = _context.Accounts
                .Where(a => a.Role == "LECTURER" && // Loại bỏ các tài khoản có Role là "Admin"
                            !_context.Lecturers.Any(l => l.AccountId == a.AccountId) &&
                            !_context.Students.Any(s => s.AccountId == a.AccountId))
                .Select(a => new SelectListItem
                {
                    Value = a.AccountId.ToString(), // Sử dụng AccountId làm giá trị
                    Text = a.Username               // Sử dụng Username làm tên hiển thị
                })
                .ToList();
            availableAccounts.Insert(0, new SelectListItem
            {
                Value = "",    // Giá trị trống
                Text = "-- Select an Account --" // Hoặc hiển thị một thông báo tùy ý
            });
            ViewData["AccountId"] = availableAccounts;

            return Page();
        }


        [BindProperty]
        public Lecturer Lecturer { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Lecturers == null || Lecturer == null)
            {
                return Page();
            }

            _context.Lecturers.Add(Lecturer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
