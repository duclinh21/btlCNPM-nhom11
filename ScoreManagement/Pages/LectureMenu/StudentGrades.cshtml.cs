using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace ScoreManagement.Pages.LectureMenu
{
    [Authorize(Roles = "LECTURER")]
    public class StudentGradesModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public StudentGradesModel(Project_PRN221Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Grade? StudentGrade { get; set; }
        public string? StudentName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public void OnGet()
        {
            // Lấy thông tin điểm của sinh viên dựa trên StudentId
            StudentGrade = _context.Grades
                .Where(g => g.StudentCourse.Student.StudentId == StudentId)
                .Select(g => new Grade
                {
                    Assignment1 = g.Assignment1,
                    Assignment2 = g.Assignment2,
                    Assignment3 = g.Assignment3,
                    ProgressTest1 = g.ProgressTest1,
                    ProgressTest2 = g.ProgressTest2,
                    ProgressTest3 = g.ProgressTest3,
                    FinalExam = g.FinalExam,
                    AverageScore = g.AverageScore,
                    Status = g.Status,
                    StudentCourse = g.StudentCourse // Ensure you also select StudentCourse to access Student
                })
                .FirstOrDefault();

            // Check for nulls before accessing FullName
            if (StudentGrade != null && StudentGrade.StudentCourse != null && StudentGrade.StudentCourse.Student != null)
            {
                StudentName = StudentGrade.StudentCourse.Student.FullName; // Lấy tên sinh viên
            }
            else
            {
                StudentName = "Unknown Student"; // Or handle accordingly
            }
        }


        public IActionResult OnPostUpdate()
        {
            // Cập nhật thông tin điểm
            var gradeToUpdate = _context.Grades.Find(StudentGrade?.GradeId);
            if (gradeToUpdate != null)
            {
                gradeToUpdate.Assignment1 = StudentGrade.Assignment1;
                gradeToUpdate.Assignment2 = StudentGrade.Assignment2;
                gradeToUpdate.Assignment3 = StudentGrade.Assignment3;
                gradeToUpdate.ProgressTest1 = StudentGrade.ProgressTest1;
                gradeToUpdate.ProgressTest2 = StudentGrade.ProgressTest2;
                gradeToUpdate.ProgressTest3 = StudentGrade.ProgressTest3;
                gradeToUpdate.FinalExam = StudentGrade.FinalExam;
                gradeToUpdate.AverageScore = StudentGrade.AverageScore;
                gradeToUpdate.Status = StudentGrade.Status;

                _context.SaveChanges();
            }

            return RedirectToPage(new { StudentId });
        }

        public IActionResult OnPostDelete()
        {
            // Xóa điểm
            var gradeToDelete = _context.Grades.Find(StudentId);
            if (gradeToDelete != null)
            {
                _context.Grades.Remove(gradeToDelete);
                _context.SaveChanges();
            }

            return RedirectToPage("/LectureMenu/LecturerDashboard"); // Điều hướng về trang danh sách lớp
        }
    }
}
