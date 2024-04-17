using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Npgsql;

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
                List<string[]> results = new List<string[]>(); 
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

                await using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                await using (var cmd = new NpgsqlCommand("SELECT * FROM packages WHERE mechanicid = @monteurID AND in_use = TRUE", conn))
                {
                    cmd.Parameters.AddWithValue("@monteurID", MonteurID);
                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var fieldValue = reader.GetInt32(0);
                            
                            results.Add(new string[] { 
                                fieldValue.ToString(), 
                                reader.GetInt32(1).ToString(),  
                                reader.GetInt32(2).ToString(),
                                reader.GetInt32(3).ToString(),
                                reader.GetBoolean(4).ToString(),
                                reader.GetString(5)
                            });
                        }
                    }
                }

                TempData["MonteurResults"] = JsonSerializer.Serialize(results);

                return RedirectToPage("/SuccessPage");
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
