using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Voorraad.WebApp.Pages
{
    public class SuccessPageModel : PageModel
    {
        public List<string> MonteurResults { get; set; }

        public void OnGet()
        {
            MonteurResults = TempData["MonteurResults"] as List<string>;
        }
    }
}
