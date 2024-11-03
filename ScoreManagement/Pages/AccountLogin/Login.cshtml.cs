using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ScoreManagement.Models;
using System.Security.Claims;
using System.Linq;

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

            public bool RememberMe { get; set; }
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.SingleOrDefault(a => a.Username == Input.Username);
                if (account != null && VerifyPassword(Input.Password, account.PasswordHash))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Username),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim("AccountId", account.AccountId.ToString())
            };

                    if (account.Role == "LECTURER")
                    {
                        var lecturer = _context.Lecturers.SingleOrDefault(s => s.AccountId == account.AccountId);
                        if (lecturer != null)
                        {
                            // Add LecturerId claim only if lecturer is found
                            claims.Add(new Claim("LecturerId", lecturer.LecturerId.ToString()));
                        }
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Redirect based on role
                    if (account.Role == "LECTURER")
                    {
                        return RedirectToPage("/LectureMenu/LecturerDashboard");
                    }
                    else if (account.Role == "STUDENT")
                    {
                        var student = _context.Students.SingleOrDefault(s => s.AccountId == account.AccountId);
                        if (student != null)
                        {
                            return RedirectToPage("/StudentMenu/StudentDashboard", new { studentId = student.StudentId });
                        }
                    }
                    else if (account.Role == "ADMIN")
                    {
                        return RedirectToPage("/AdminMenu/AdminDashboard");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            Console.WriteLine("Logout function called");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/AccountLogin/Login");
        }



        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return enteredPassword == storedPasswordHash;
        }

        
    }
}
