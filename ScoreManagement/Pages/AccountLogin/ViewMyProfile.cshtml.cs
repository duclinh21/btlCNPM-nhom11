using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;
using System.Threading.Tasks;

namespace ScoreManagement.Pages.AccountLogin
{
    [Authorize]
    public class ViewMyProfileModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public ViewMyProfileModel(Project_PRN221Context context)
        {
            _context = context;
        }

        public Account AccountInfo { get; set; }
        public Lecturer LecturerInfo { get; set; } // Thông tin giảng viên
        public Student StudentInfo { get; set; } // Thông tin sinh viên

        public async Task<IActionResult> OnGetAsync()
        {
            var accountIdClaim = User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;

            if (accountIdClaim == null || !int.TryParse(accountIdClaim, out int accountId))
            {
                return NotFound(); // Hoặc xử lý lỗi khác nếu không tìm thấy AccountId
            }

            AccountInfo = await _context.Accounts.FindAsync(accountId);

            if (AccountInfo == null)
            {
                return NotFound(); // Nếu không tìm thấy tài khoản
            }

            if (AccountInfo.Role == "LECTURER")
            {
                LecturerInfo = await _context.Lecturers.FirstOrDefaultAsync(l => l.AccountId == AccountInfo.AccountId);
            }
            else if (AccountInfo.Role == "STUDENT")
            {
                StudentInfo = await _context.Students.FirstOrDefaultAsync(s => s.AccountId == AccountInfo.AccountId);
            }

            return Page();
        }


    }
}
