using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voorraad.WebApp.Pages
{
    public class MonteurModel : PageModel
    {
        private readonly ILogger<MonteurModel> _logger;

        public MonteurModel(ILogger<MonteurModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync(int MonteurID)
        {
            try
            {
                // Your existing code to retrieve data
                List<string> results = new List<string>(); // Assuming your data is a list of strings
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=britt;Database=Voorraadmodule";

                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                // Retrieve all rows
                await using (var cmd = new NpgsqlCommand("SELECT quantity FROM packages WHERE monteurid = @monteurID", conn))
                {
                    cmd.Parameters.AddWithValue("@monteurID", MonteurID);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fieldValue = reader.GetInt32(0); // Read the integer value from the database
                            results.Add(fieldValue.ToString()); // Convert the integer to a string before adding it to the results list
                        }
                    }
                }

                TempData["MonteurResults"] = results; // Store results in TempData
                return RedirectToPage("/SuccessPage"); // Redirect to SuccessPage
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the form submission.");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return Page();
            }
        }
    }
}
