using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Voorraad.WebApp.Pages
{
    public class SuccessPageModel : PageModel
    {
        public List<string[]> MonteurResults { get; set; }

        public void OnGet()
        {
            // This method is executed when the page is requested via HTTP GET
        }
    }
}
