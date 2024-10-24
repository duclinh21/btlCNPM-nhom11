using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly Project_PRN221Context _context;
        public LoginModel(Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.SingleOrDefault(a => a.Username == Input.Username);
                if (account != null && VerifyPassword(Input.Password, account.PasswordHash))
                {
                    if (account.Role == "ADMIN")
                    {
                        return RedirectToPage("/AdminDashboard");
                    }
                    else if (account.Role == "LECTURER")
                    {
                        return RedirectToPage("/LecturerDashboard");
                    }
                    else if (account.Role == "STUDENT")
                    {
                        return RedirectToPage("/StudentDashboard");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return enteredPassword == storedPasswordHash;
        }
    }
}
