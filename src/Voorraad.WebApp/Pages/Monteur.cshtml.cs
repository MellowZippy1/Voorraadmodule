using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Voorraad.WebApp.Pages;

public class MonteurModel : PageModel
{
    private readonly ILogger<MonteurModel> _logger;

    public MonteurModel(ILogger<MonteurModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
