using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ScoreManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ScoreManagement.Pages.LectureMenu
{
    [Authorize(Roles = "LECTURER")]
    public class CreateGradeModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public CreateGradeModel(Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty]
        // Liên kết với form trong Razor Page
        public Grade StudentGrade { get; set; } = new Grade();

        [BindProperty]
        // Liên kết với ID của sinh viên để giữ lại khi form submit
        public int StudentId { get; set; }

        [BindProperty]
        public int CourseId { get; set; } // Thêm thuộc tính CourseId

        // Tên của sinh viên để hiển thị trên giao diện
        public string? StudentName { get; private set; }

        // Phương thức `OnGetAsync` sẽ được gọi khi trang được tải
        public async Task<IActionResult> OnGetAsync(int studentId, int courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
            // Lấy thông tin sinh viên (dùng cho mục đích hiển thị tên)
            var student = await _context.Students.FindAsync(StudentId);
            if (student == null)
            {
                return NotFound();
            }
            StudentName = student.FullName; // Gán tên sinh viên để hiển thị

            return Page();
        }

        // Phương thức `OnPostAsync` được gọi khi form submit
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            StudentGrade.CalculateAverageAndStatus();

            var studentCourse = _context.StudentsCourses
                 .FirstOrDefault(sc => sc.StudentId == StudentId && sc.CourseId == CourseId); // Tìm StudentCourseId theo cả StudentId và CourseId

            if (studentCourse != null)
            {
                StudentGrade.StudentCourseId = studentCourse.StudentCourseId;
                _context.Grades.Add(StudentGrade);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/LectureMenu/StudentGrades", new { StudentId = StudentId, CourseId = CourseId }); // Chuyển hướng với cả hai tham số
        }
    }
}
