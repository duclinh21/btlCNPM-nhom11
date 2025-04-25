using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;
using OfficeOpenXml;

namespace ScoreManagement.Pages.AdminMenu.AccountManage
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public IndexModel(Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Account> Account { get; set; } = new List<Account>();

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? RoleFilter { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Accounts.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTerm))
            {
                query = query.Where(a =>
                    a.Username.Contains(SearchTerm) || a.Role.Contains(SearchTerm));
            }

            if (!string.IsNullOrEmpty(RoleFilter))
            {
                query = query.Where(a => a.Role == RoleFilter);
            }

            Account = await query.ToListAsync();
        }

        public async Task<IActionResult> OnPostResetPasswordAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                TempData["Message"] = $"Reset thất bại: Không tìm thấy tài khoản với ID {id}.";
                return RedirectToPage();
            }

            account.PasswordHash = BCrypt.Net.BCrypt.HashPassword("2025");
            await _context.SaveChangesAsync();
            TempData["Message"] = $"✅ Đã đặt lại mật khẩu cho {account.Username} về '2025'.";
            return RedirectToPage();
        }

    }
}
