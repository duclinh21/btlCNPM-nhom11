using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace ScoreManagement.Pages.AccountLogin
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

            public bool RememberMe { get; set; } // Thêm thuộc tính RememberMe
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.SingleOrDefault(a => a.Username == Input.Username);
                if (account != null && VerifyPassword(Input.Password, account.PasswordHash))
                {
                    // Tạo các claim dựa trên vai trò của người dùng
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Username),
                new Claim(ClaimTypes.Role, account.Role) // Thêm vai trò
            };

                    // Tạo đối tượng ClaimsIdentity với claims
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Đăng nhập người dùng với cookie
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Điều hướng dựa trên vai trò
                    if (account.Role == "ADMIN")
                    {
                        return RedirectToPage("/AdminMenu/AdminDashboard");
                    }
                    else if (account.Role == "LECTURER")
                    {
                        return RedirectToPage("/LecturerDashboard");
                    }
                    else if (account.Role == "STUDENT")
                    {
                        var student = _context.Students.SingleOrDefault(s => s.AccountId == account.AccountId);
                        if (student != null)
                        {
                            return RedirectToPage("/StudentMenu/StudentDashboard", new { studentId = student.StudentId });
                        }
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
