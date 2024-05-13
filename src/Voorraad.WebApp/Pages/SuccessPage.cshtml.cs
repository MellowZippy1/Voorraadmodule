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

        public void OnPostBtnLogin_Click1(int index)
        {
            Console.WriteLine($"HI! {index} yaya");
            // Add your server-side logic here
        }

    }
}
