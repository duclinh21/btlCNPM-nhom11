using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScoreManagement.Models;
using System.Linq;
using ScoreManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace ScoreManagement.Pages.StudentMenu
{
    [Authorize(Roles = "STUDENT")]
    public class StudentDashboardModel : PageModel
    {
        private readonly Project_PRN221Context _context;

        public StudentDashboardModel(Project_PRN221Context context)
        {
            _context = context;
        }

        public List<StudentReportViewModel>? StudentReports { get; set; }
        // Thêm các thuộc tính này

        public int StudentId { get; set; }
        public string? FullName { get; set; }
        public string? StudentCode { get; set; }

        public List<string> Semesters { get; set; }
        public List<string> Statuses { get; set; }

        public Dictionary<string, double> AverageScorePerCourse { get; set; }

        public int PassCount { get; set; }
        public int NotPassCount { get; set; }

        public void CalculateStatistics()
        {
            if (StudentReports != null && StudentReports.Any())
            {
                AverageScorePerCourse = StudentReports
                    .GroupBy(report => report.CourseName)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Average(report => report.AverageScore ?? 0)
                    );
            }
        }

        public void OnGet(int studentId)
        {
            StudentId = studentId;
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
                                  CourseCode = c.CourseCode,
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



            CalculateStatistics();

            // Tính số lượng môn Pass và Not Pass
            PassCount = StudentReports.Count(r => r.Status == "Pass");
            NotPassCount = StudentReports.Count(r => r.Status == "Not Pass");
        }


        public IActionResult OnGetExportToExcel(int studentId)
        {
            var studentIdForExport = StudentId;
            // Lấy thông tin sinh viên
            var student = _context.Students.SingleOrDefault(s => s.StudentId == studentId);
            if (student == null)
            {
                return NotFound();
            }

            // Lấy thông tin điểm cho sinh viên
            var studentReports = (from sc in _context.StudentsCourses
                                  join s in _context.Students on sc.StudentId equals s.StudentId
                                  join sem in _context.Semesters on sc.SemesterId equals sem.SemesterId
                                  join c in _context.Courses on sc.CourseId equals c.CourseId
                                  join g in _context.Grades on sc.StudentCourseId equals g.StudentCourseId
                                  where s.StudentId == studentId
                                  select new StudentReportViewModel
                                  {
                                      SemesterCode = sem.SemesterCode,
                                      CourseName = c.CourseName,
                                      CourseCode = c.CourseCode,
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

            // Tạo file Excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Grade Report");
                worksheet.Cells[1, 1].Value = "Semester";
                worksheet.Cells[1, 2].Value = "Course Name";
                worksheet.Cells[1, 3].Value = "Course Code";
                worksheet.Cells[1, 4].Value = "Assignment 1";
                worksheet.Cells[1, 5].Value = "Assignment 2";
                worksheet.Cells[1, 6].Value = "Assignment 3";
                worksheet.Cells[1, 7].Value = "Progress Test 1";
                worksheet.Cells[1, 8].Value = "Progress Test 2";
                worksheet.Cells[1, 9].Value = "Progress Test 3";
                worksheet.Cells[1, 10].Value = "Final Exam";
                worksheet.Cells[1, 11].Value = "Average Score";
                worksheet.Cells[1, 12].Value = "Status";

                for (int i = 0; i < studentReports.Count; i++)
                {
                    var report = studentReports[i];
                    worksheet.Cells[i + 2, 1].Value = report.SemesterCode;
                    worksheet.Cells[i + 2, 2].Value = report.CourseName;
                    worksheet.Cells[i + 2, 3].Value = report.CourseCode;
                    worksheet.Cells[i + 2, 4].Value = report.Assignment1;
                    worksheet.Cells[i + 2, 5].Value = report.Assignment2;
                    worksheet.Cells[i + 2, 6].Value = report.Assignment3;
                    worksheet.Cells[i + 2, 7].Value = report.ProgressTest1;
                    worksheet.Cells[i + 2, 8].Value = report.ProgressTest2;
                    worksheet.Cells[i + 2, 9].Value = report.ProgressTest3;
                    worksheet.Cells[i + 2, 10].Value = report.FinalExam;
                    worksheet.Cells[i + 2, 11].Value = report.AverageScore;
                    worksheet.Cells[i + 2, 12].Value = report.Status;
                }

                // Đặt kích thước cột tự động
                worksheet.Cells.AutoFitColumns();

                // Trả về file Excel
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var content = stream.ToArray();
                var fileName = $"GradeReport_{student.FullName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}


