using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ScoreManagement.Pages.LectureMenu
{
    [Authorize(Roles = "LECTURER")]
    public class StudentGradesModel : PageModel
    {
        private readonly Project_PRN221Context _context;
        private readonly ILogger<StudentGradesModel> _logger;

        public StudentGradesModel(Project_PRN221Context context, ILogger<StudentGradesModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)]
        public int CourseId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Grade? StudentGrade { get; set; }
        public string? StudentName { get; private set; }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public void OnGet(int? studentId, int? courseId)
        {
            if (studentId.HasValue && courseId.HasValue)
            {
                var studentCourse = _context.StudentsCourses
                    .FirstOrDefault(sc => sc.StudentId == studentId && sc.CourseId == courseId);

                

                if (studentCourse != null)
                {
                    StudentGrade = _context.Grades
                        .FirstOrDefault(g => g.StudentCourseId == studentCourse.StudentCourseId);

                    // Tính toán AverageScore và Status
                    StudentGrade?.CalculateAverageAndStatus();

                    var studentName = _context.Students
    .Where(s => s.StudentId == studentId) // Thay đổi điều kiện dựa trên cách bạn lưu trữ sinh viên
    .Select(s => s.FullName) // Giả sử FullName là thuộc tính tên sinh viên
    .FirstOrDefault();
                    StudentName = studentName;

                }
                else
                {
                    TempData["ErrorMessage"] = "studentCourse không có dữ liệu.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "StudentId hoặc courseId không có dữ liệu.";
            }
        }

        public IActionResult OnPostUpdate()
        {

            if (StudentGrade == null)
            {
                TempData["ErrorMessage"] = "Cập nhật không thành công. Vui lòng kiểm tra dữ liệu StudentGrade.";
                return RedirectToPage(new { StudentId, CourseId });
            }

            // Kiểm tra xem StudentCourseId có giá trị không
            if (StudentGrade.StudentCourseId == null)
            {
                TempData["ErrorMessage"] = "Cập nhật không thành công. Vui lòng kiểm tra dữ liệu StudentCourseId.";
                return RedirectToPage(new { StudentId, CourseId });
            }

            // Tìm StudentCourse dựa trên StudentId và CourseId
            var studentCourse = _context.StudentsCourses
                .FirstOrDefault(sc => sc.StudentId == StudentId && sc.CourseId == CourseId);

            if (studentCourse != null)
            {
                // Tìm Grade dựa trên StudentCourseId
                var gradeToUpdate = _context.Grades
                    .FirstOrDefault(g => g.StudentCourseId == studentCourse.StudentCourseId);

                if (gradeToUpdate != null)
                {
                    // Cập nhật điểm
                    gradeToUpdate.Assignment1 = StudentGrade.Assignment1;
                    gradeToUpdate.Assignment2 = StudentGrade.Assignment2;
                    gradeToUpdate.Assignment3 = StudentGrade.Assignment3;
                    gradeToUpdate.ProgressTest1 = StudentGrade.ProgressTest1;
                    gradeToUpdate.ProgressTest2 = StudentGrade.ProgressTest2;
                    gradeToUpdate.ProgressTest3 = StudentGrade.ProgressTest3;
                    gradeToUpdate.FinalExam = StudentGrade.FinalExam;
                    gradeToUpdate.CalculateAverageAndStatus();
                    try
                    {
                        _context.SaveChanges();
                        _logger.LogInformation("Grade updated successfully for StudentId: {StudentId}", StudentId);
                        TempData["SuccessMessage"] = "Cập nhật điểm thành công!";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error updating grade for StudentId: {StudentId}", StudentId);
                        TempData["ErrorMessage"] = "Cập nhật không thành công. Vui lòng thử lại.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Không tìm thấy điểm để cập nhật.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin khóa học cho sinh viên.";
            }

            return RedirectToPage(new { StudentId, CourseId });
        }


        public IActionResult OnPostDelete()
        {
            var studentCourse = _context.StudentsCourses
                .FirstOrDefault(sc => sc.StudentId == StudentId && sc.CourseId == CourseId);

            if (studentCourse != null)
            {
                var gradeToDelete = _context.Grades
                    .FirstOrDefault(g => g.StudentCourseId == studentCourse.StudentCourseId);

                if (gradeToDelete != null)
                {
                    _context.Grades.Remove(gradeToDelete);
                    _context.SaveChanges();
                    _logger.LogInformation("Grade deleted successfully for StudentId: {StudentId} in CourseId: {CourseId}", StudentId, CourseId);
                }
            }

            return RedirectToPage(new { StudentId, CourseId });
        }
    }
}
