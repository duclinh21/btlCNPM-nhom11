using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.AccountManage
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
            // Thiết lập danh sách vai trò
            ViewData["RoleOptions"] = new List<SelectListItem>
            {
                new SelectListItem { Value = "LECTURER", Text = "Lecturer" },
                new SelectListItem { Value = "STUDENT", Text = "Student" }
            };

            return Page();
        }

        [BindProperty]
        public Account Account { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra nếu Username đã tồn tại
            var existingAccount = await _context.Accounts
                .AnyAsync(a => a.Username == Account.Username);

            if (existingAccount)
            {
                // Thêm lỗi vào ModelState nếu Username đã tồn tại
                ModelState.AddModelError("Account.Username", "Username đã tồn tại. Vui lòng chọn Username khác.");
                return Page();
            }

            // Nếu không có lỗi, thêm Account mới vào database
            _context.Accounts.Add(Account);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
