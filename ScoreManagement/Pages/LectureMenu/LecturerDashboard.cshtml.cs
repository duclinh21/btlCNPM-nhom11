using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication;

namespace ScoreManagement.Pages.LectureMenu
{
    [Authorize(Roles = "LECTURER")]
    public class LecturerDashboardModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public LecturerDashboardModel(Project_PRN221Context context)
        {
            _context = context;
        }

        public int LecturerId { get; private set; }
        public List<ClassInfo>? TeachingClasses { get; set; }
        public Class? SelectedClass { get; set; }
        public Course? SelectedCourse { get; set; }
        public List<StudentInfo>? Students { get; set; }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/AccountLogin/Login");
        }

        public IActionResult OnGet(int? ClassId)
        {
            // Retrieve LecturerId from claims
            foreach (var claim in User.Claims)
            {
                System.Diagnostics.Debug.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
            }
            var lecturerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "LecturerId");
            if (lecturerIdClaim != null)
            {
                LecturerId = int.Parse(lecturerIdClaim.Value);
            }
            else
            {
                // Redirect to login if LecturerId is not available in claims
                return RedirectToPage("/AccountLogin/Login");
            }



            // Get the list of teaching classes
            TeachingClasses = _context.ClassCourses
                .Where(cc => cc.LecturerId == LecturerId)
                .Select(cc => new ClassInfo
                {
                    ClassCode = cc.Class.ClassCode,
                    CourseName = cc.Course.CourseName ?? "Unknown Course",
                    CourseCode = cc.Course.CourseCode,
                    ClassId = cc.Class.ClassId
                })
                .ToList();
            if (TeachingClasses == null)
            {
                TeachingClasses = new List<ClassInfo>();
            }

            // If ClassId is provided, load class details, courses, and students
            if (ClassId.HasValue)
            {
                SelectedClass = _context.Classes.FirstOrDefault(c => c.ClassId == ClassId.Value);
                SelectedCourse = _context.ClassCourses
                    .Where(cc => cc.ClassId == ClassId.Value)
                    .Select(cc => cc.Course)
                    .FirstOrDefault();

                // Fetch student names and codes
                Students = _context.StudentClasses
                    .Where(sc => sc.ClassId == ClassId.Value)
                    .Join(
                        _context.Students,
                        sc => sc.StudentId,
                        s => s.StudentId,
                        (sc, s) => new StudentInfo
                        {
                            StudentName = s.FullName,
                            StudentCode = s.StudentCode
                        }
                    )
                    .ToList();
                if (Students == null)
                {
                    Students = new List<StudentInfo>();
                }
            }

            return Page();
        }
    }

    public class ClassInfo
    {
        public int? ClassId { get; set; }
        public string? ClassCode { get; set; }
        public string? CourseName { get; set; }
        public string? CourseCode { get; set; }
    }

    // New class to represent student information
    public class StudentInfo
    {
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
    }

   
}
