using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using ScoreManagement.Models;
using System.Linq;

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
        public Grade StudentGrade { get; set; }

        public string StudentName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public void OnGet()
        {
            var studentCourse = _context.StudentsCourses
                .Where(sc => sc.StudentId == StudentId)
                .FirstOrDefault();

            if (studentCourse != null)
            {
                //StudentName = studentCourse.Student.FullName;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tìm StudentCourse để lưu điểm
            var studentCourse = _context.StudentsCourses
                .Where(sc => sc.StudentId == StudentId)
                .FirstOrDefault();

            if (studentCourse != null)
            {
                StudentGrade.StudentCourseId = studentCourse.StudentCourseId; // Gán StudentCourseId
                _context.Grades.Add(StudentGrade);
                _context.SaveChanges();
            }

            return RedirectToPage("/LectureMenu/StudentGrades", new { StudentId });
        }
    }
}
