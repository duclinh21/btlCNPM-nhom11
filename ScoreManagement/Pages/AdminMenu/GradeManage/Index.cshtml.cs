using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.GradeManage
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Grade> Grade { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Grades != null)
            {
                Grade = await _context.Grades
                    .Include(g => g.StudentCourse)
                    .ThenInclude(sc => sc.Course) // Liên kết với Course qua StudentCourse
                    .Include(g => g.StudentCourse.Semester) // Liên kết với Semester qua StudentCourse
                    .Include(g => g.StudentCourse.Student) // Include thêm bảng Students thông qua StudentCourse
                    .Include(g => g.StudentCourse.Class) // Include thêm bảng Students thông qua StudentCourse
                    .ToListAsync();
            }
        }
        public async Task<IActionResult> OnGetExportExcelAsync()
        {
            var grades = await _context.Grades
                .Include(g => g.StudentCourse)
                .ThenInclude(sc => sc.Course)
                .Include(g => g.StudentCourse.Semester)
                .Include(g => g.StudentCourse.Student)
                .Include(g => g.StudentCourse.Class)
                .ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Grades");

            // Thêm tiêu đề cột
            worksheet.Cells[1, 1].Value = "StudentCode";
            worksheet.Cells[1, 2].Value = "FullName";
            worksheet.Cells[1, 3].Value = "CourseCode";
            worksheet.Cells[1, 4].Value = "SemesterCode";
            worksheet.Cells[1, 5].Value = "ClassCode";
            worksheet.Cells[1, 6].Value = "Assignment1";
            worksheet.Cells[1, 7].Value = "Assignment2";
            worksheet.Cells[1, 8].Value = "Assignment3";
            worksheet.Cells[1, 9].Value = "ProgressTest1";
            worksheet.Cells[1, 10].Value = "ProgressTest2";
            worksheet.Cells[1, 11].Value = "ProgressTest3";
            worksheet.Cells[1, 12].Value = "FinalExam";
            worksheet.Cells[1, 13].Value = "AverageScore";
            worksheet.Cells[1, 14].Value = "Status";

            // Thêm dữ liệu vào các hàng
            for (int i = 0; i < grades.Count; i++)
            {
                var grade = grades[i];
                worksheet.Cells[i + 2, 1].Value = grade.StudentCourse.Student.StudentCode;
                worksheet.Cells[i + 2, 2].Value = grade.StudentCourse.Student.FullName;
                worksheet.Cells[i + 2, 3].Value = grade.StudentCourse.Course.CourseCode;
                worksheet.Cells[i + 2, 4].Value = grade.StudentCourse.Semester.SemesterCode;
                worksheet.Cells[i + 2, 5].Value = grade.StudentCourse.Class.ClassCode;
                worksheet.Cells[i + 2, 6].Value = grade.Assignment1;
                worksheet.Cells[i + 2, 7].Value = grade.Assignment2;
                worksheet.Cells[i + 2, 8].Value = grade.Assignment3;
                worksheet.Cells[i + 2, 9].Value = grade.ProgressTest1;
                worksheet.Cells[i + 2, 10].Value = grade.ProgressTest2;
                worksheet.Cells[i + 2, 11].Value = grade.ProgressTest3;
                worksheet.Cells[i + 2, 12].Value = grade.FinalExam;
                worksheet.Cells[i + 2, 13].Value = grade.GetFormattedAverageScore();
                worksheet.Cells[i + 2, 14].Value = grade.Status;
            }

            // Xuất file Excel
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            var fileName = "Grades.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

    }
}
