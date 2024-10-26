using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.StudentsManage
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
            // Lấy danh sách các Account không phải Admin và chưa được liên kết với Student hoặc Lecturer
            var availableAccounts = _context.Accounts
                .Where(a => a.Role == "STUDENT" &&  // Loại bỏ các tài khoản Admin
                            !_context.Lecturers.Any(l => l.AccountId == a.AccountId) &&
                            !_context.Students.Any(s => s.AccountId == a.AccountId))
                .Select(a => new SelectListItem
                {
                    Value = a.AccountId.ToString(), // Sử dụng AccountId làm giá trị
                    Text = a.Username               // Sử dụng Username làm tên hiển thị
                })
                .ToList();

            // Thêm một mục trống vào đầu danh sách để dropdown mặc định là trống
            availableAccounts.Insert(0, new SelectListItem
            {
                Value = "",    // Giá trị trống
                Text = "-- Select an Account --" // Hoặc hiển thị một thông báo tùy ý
            });

            ViewData["AccountId"] = availableAccounts;

            // Danh sách Major
            ViewData["MajorId"] = new SelectList(_context.Majors, "MajorId", "MajorName");

            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra nếu StudentCode hoặc FullName đã tồn tại
            var existingStudent = await _context.Students
                .AnyAsync(s => s.StudentCode == Student.StudentCode);

            if (existingStudent)
            {
                // Thêm lỗi vào ModelState nếu StudentCode hoặc FullName đã tồn tại
                if (_context.Students.Any(s => s.StudentCode == Student.StudentCode))
                {
                    ModelState.AddModelError("Student.StudentCode", "Mã sinh viên đã tồn tại. Vui lòng chọn mã khác.");
                }

                return Page();
            }

            // Nếu không có lỗi, thêm Student mới vào database
            _context.Students.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }


    }
}
