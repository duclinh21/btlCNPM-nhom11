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

namespace ScoreManagement.Pages.StudentsManage
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Student> Student { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Students != null)
            {
                Student = await _context.Students
                .Include(s => s.Account)
                .Include(s => s.Major).ToListAsync();
            }
        }
        public async Task<IActionResult> OnGetExportToExcelAsync()
        {
            if (_context.Students == null)
                return NotFound();

            var students = await _context.Students
                .Include(s => s.Account)
                .Include(s => s.Major)
                .ToListAsync();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Students");

            // Thêm tiêu đề cho các cột
            worksheet.Cells[1, 1].Value = "StudentId";
            worksheet.Cells[1, 2].Value = "StudentCode";
            worksheet.Cells[1, 3].Value = "FullName";
            worksheet.Cells[1, 4].Value = "Date Of Birth";
            worksheet.Cells[1, 5].Value = "Gender";
            worksheet.Cells[1, 6].Value = "Address";
            worksheet.Cells[1, 7].Value = "PhoneNumber";
            worksheet.Cells[1, 8].Value = "Account";
            worksheet.Cells[1, 9].Value = "Major";

            // Thêm dữ liệu vào các hàng
            for (int i = 0; i < students.Count; i++)
            {
                var student = students[i];
                worksheet.Cells[i + 2, 1].Value = student.StudentId;
                worksheet.Cells[i + 2, 2].Value = student.StudentCode;
                worksheet.Cells[i + 2, 3].Value = student.FullName;
                worksheet.Cells[i + 2, 4].Value = student.Dob?.ToString("dd/MM/yyyy");
                worksheet.Cells[i + 2, 5].Value = student.Gender == true ? "Nam" : "Nữ";
                worksheet.Cells[i + 2, 6].Value = student.Address;
                worksheet.Cells[i + 2, 7].Value = student.PhoneNumber;
                worksheet.Cells[i + 2, 8].Value = student.Account?.Username;
                worksheet.Cells[i + 2, 9].Value = student.Major?.MajorName;
            }

            // Xuất file Excel
            var stream = new MemoryStream();
            package.SaveAs(stream);
            stream.Position = 0;

            var fileName = "Students.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
