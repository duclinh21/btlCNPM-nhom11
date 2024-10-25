using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Linq;
using ScoreManagement.ViewModels;

namespace ScoreManagement.Pages.Student
{
    public class StudentDashboardModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public StudentDashboardModel(Project_PRN221Context context)
        {
            _context = context;
        }

        public List<StudentReportViewModel> StudentReports { get; set; }
        // Thêm các thuộc tính này
        public string FullName { get; set; }
        public string StudentCode { get; set; }


        public void OnGet(int studentId)
        {
            // Lấy thông tin sinh viên
            var student = _context.Students.SingleOrDefault(s => s.StudentId == studentId);
            if (student != null)
            {
                FullName = student.FullName;
                StudentCode = student.StudentCode;
            }

            // Lấy thông tin về các khóa học và điểm cho sinh viên cụ thể
            StudentReports = (from sc in _context.StudentsCourses
                              join s in _context.Students on sc.StudentId equals s.StudentId
                              join sem in _context.Semesters on sc.SemesterId equals sem.SemesterId
                              join c in _context.Courses on sc.CourseId equals c.CourseId
                              join g in _context.Grades on sc.StudentCourseId equals g.StudentCourseId
                              where s.StudentId == studentId
                              select new StudentReportViewModel
                              {
                                  SemesterCode = sem.SemesterCode,
                                  CourseName = c.CourseName,
                                  Assignment1 = g.Assignment1,
                                  Assignment2 = g.Assignment2,
                                  Assignment3 = g.Assignment3,
                                  ProgressTest1 = g.ProgressTest1,
                                  ProgressTest2 = g.ProgressTest2,
                                  ProgressTest3 = g.ProgressTest3,
                                  FinalExam = g.FinalExam,
                                  AverageScore = g.AverageScore,
                                  Status = g.Status
                              }).ToList();
        }
    }
}


