using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;


namespace ScoreManagement.Pages.AdminMenu
{
    [Authorize(Roles = "ADMIN")]
    public class AdminDashboardModel : PageModel
    {

        
        public void OnGet()
        {
        }
    }
}
