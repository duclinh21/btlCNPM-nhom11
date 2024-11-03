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

namespace ScoreManagement.Pages.AdminMenu.LecturersManage
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Lecturer> Lecturer { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Lecturers != null)
            {
                Lecturer = await _context.Lecturers
                .Include(l => l.Account).ToListAsync();
            }
        }
		public async Task<IActionResult> OnGetExportToExcelAsync()
		{
			if (_context.Lecturers == null)
				return NotFound();

			var lecturers = await _context.Lecturers
				.Include(l => l.Account).ToListAsync();

			using var package = new ExcelPackage();
			var worksheet = package.Workbook.Worksheets.Add("Lecturers");

			// Thêm tiêu đề cho các cột
			worksheet.Cells[1, 1].Value = "LecturerId";
			worksheet.Cells[1, 2].Value = "LecturerName";
			worksheet.Cells[1, 3].Value = "Date Of Birth";
			worksheet.Cells[1, 4].Value = "Gender";
			worksheet.Cells[1, 5].Value = "Address";
			worksheet.Cells[1, 6].Value = "PhoneNumber";
			worksheet.Cells[1, 7].Value = "Account";

			// Thêm dữ liệu vào các hàng
			for (int i = 0; i < lecturers.Count; i++)
			{
				var lecturer = lecturers[i];
				worksheet.Cells[i + 2, 1].Value = lecturer.LecturerId;
				worksheet.Cells[i + 2, 2].Value = lecturer.LecturerName;
				worksheet.Cells[i + 2, 3].Value = lecturer.Dob?.ToString("dd/MM/yyyy");
				worksheet.Cells[i + 2, 4].Value = lecturer.Gender == true ? "Nam" : "Nữ";
				worksheet.Cells[i + 2, 5].Value = lecturer.Address;
				worksheet.Cells[i + 2, 6].Value = lecturer.PhoneNumber;
				worksheet.Cells[i + 2, 7].Value = lecturer.Account?.Username;
			}

			// Xuất file Excel
			var stream = new MemoryStream();
			package.SaveAs(stream);
			stream.Position = 0;

			var fileName = "Lecturers.xlsx";
			return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
		}
	}
}
