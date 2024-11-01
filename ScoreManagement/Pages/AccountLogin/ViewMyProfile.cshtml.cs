using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Linq;

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

        public IActionResult OnGet()
        {
            var username = User.Identity.Name;

            AccountInfo = _context.Accounts.SingleOrDefault(a => a.Username == username);

            if (AccountInfo == null)
            {
                return RedirectToPage("/AccountLogin/Login");
            }

            return Page();
        }
    }
}
