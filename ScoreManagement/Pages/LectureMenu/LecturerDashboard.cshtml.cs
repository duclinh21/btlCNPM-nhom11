using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IActionResult OnGet(int? CourseId)
        {
            // Retrieve LecturerId from claims
            var lecturerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "LecturerId");
            if (lecturerIdClaim != null)
            {
                LecturerId = int.Parse(lecturerIdClaim.Value);
            }
            else
            {
                return RedirectToPage("/AccountLogin/Login");
            }

            // Lấy danh sách các lớp mà giảng viên đang dạy
            TeachingClasses = _context.ClassCourses
                .Where(cc => cc.LecturerId == LecturerId)
                .Select(cc => new ClassInfo
                {
                    ClassCode = cc.Class.ClassCode,
                    CourseName = cc.Course.CourseName ?? "Unknown Course",
                    CourseCode = cc.Course.CourseCode,
                    ClassId = cc.Class.ClassId,
                    CourseId = cc.CourseId
                })
                .ToList();

            if (CourseId.HasValue)
            {
                // Tìm ClassId dựa trên CourseId
                var classCourse = _context.ClassCourses.FirstOrDefault(cc => cc.CourseId == CourseId.Value);
                if (classCourse != null)
                {
                    SelectedClass = _context.Classes.FirstOrDefault(c => c.ClassId == classCourse.ClassId);
                    SelectedCourse = _context.Courses.FirstOrDefault(c => c.CourseId == CourseId.Value);

                    // Lấy danh sách sinh viên dựa trên ClassId
                    Students = _context.StudentClasses
                        .Where(sc => sc.ClassId == classCourse.ClassId)
                        .Join(
                            _context.Students,
                            sc => sc.StudentId,
                            s => s.StudentId,
                            (sc, s) => new StudentInfo
                            {
                                StudentId = s.StudentId,
                                StudentName = s.FullName,
                                StudentCode = s.StudentCode
                            }
                        )
                        .ToList();
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
        public int? CourseId { get; set; }
    }

    // New class to represent student information
    public class StudentInfo
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public string? StudentCode { get; set; }
    }
}
