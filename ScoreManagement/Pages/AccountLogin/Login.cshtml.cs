using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using ScoreManagement.Models;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Identity;

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
            [BindProperty(SupportsGet = true)]
            public string captchaInput { get; set; }

        }

        public IActionResult OnPost()
        {
            // Kiểm tra captcha trước khi kiểm tra mật khẩu
            var sessionCaptcha = HttpContext.Session.GetString("CaptchaCode");
            if (string.IsNullOrEmpty(Input.captchaInput) || Input.captchaInput != sessionCaptcha)
            {
                // Nếu captcha sai, báo lỗi
                ModelState.AddModelError(string.Empty, "Mã xác nhận không đúng.");
                return Page(); // Quay lại trang đăng nhập với lỗi captcha
            }

            // Kiểm tra tài khoản và mật khẩu
            if (ModelState.IsValid)
            {
                var account = _context.Accounts.SingleOrDefault(a => a.Username == Input.Username);
                if (account != null && VerifyPassword(Input.Password, account.PasswordHash))
                {
                    // Nếu mật khẩu đúng, tạo các claim
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Input.Username),
                new Claim(ClaimTypes.Role, account.Role),
                new Claim("AccountId", account.AccountId.ToString())
            };

                    // Thêm thông tin người dùng nếu có
                    if (account.Role == "LECTURER")
                    {
                        var lecturer = _context.Lecturers.SingleOrDefault(s => s.AccountId == account.AccountId);
                        if (lecturer != null)
                            claims.Add(new Claim("LecturerId", lecturer.LecturerId.ToString()));
                    }

                    // Đăng nhập người dùng và tạo session
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    // Chuyển hướng người dùng dựa trên vai trò
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
                else
                {
                    // Nếu mật khẩu sai, báo lỗi
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }

            return Page(); // Quay lại trang đăng nhập nếu có lỗi
        }


        public async Task<IActionResult> OnPostLogoutAsync()
        {
            Console.WriteLine("Logout function called");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/AccountLogin/Login");
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        public IActionResult OnGetCaptchaImage()
        {
            var captchaText = GenerateCaptchaText();
            HttpContext.Session.SetString("CaptchaCode", captchaText);

            var captchaImage = GenerateCaptchaImage(captchaText);
            return File(captchaImage, "image/png");
        }
       
        private string GenerateCaptchaText()
        {
            var chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private byte[] GenerateCaptchaImage(string captchaText)
        {
            int width = 120;
            int height = 40;
            using var bitmap = new System.Drawing.Bitmap(width, height);
            using var graphics = System.Drawing.Graphics.FromImage(bitmap);
            var font = new System.Drawing.Font("Arial", 20, System.Drawing.FontStyle.Bold);

            graphics.Clear(System.Drawing.Color.White);
            graphics.DrawString(captchaText, font, System.Drawing.Brushes.Black, new System.Drawing.PointF(10, 5));

            using var ms = new MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

    }
}
