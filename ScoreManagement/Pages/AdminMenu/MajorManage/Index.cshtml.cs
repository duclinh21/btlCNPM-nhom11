using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ScoreManagement.Models;

namespace ScoreManagement.Pages.AdminMenu.MajorManage
{
    public class IndexModel : PageModel
    {
        private readonly ScoreManagement.Models.Project_PRN221Context _context;

        public IndexModel(ScoreManagement.Models.Project_PRN221Context context)
        {
            _context = context;
        }

        public IList<Major> Major { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Majors != null)
            {
                Major = await _context.Majors.ToListAsync();
            }
        }
    }
}
