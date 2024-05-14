using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Npgsql;

namespace Voorraad.WebApp.Pages
{
    public class SuccessPageModel : PageModel
    {
        private readonly ILogger<SuccessPageModel> _logger;

        public SuccessPageModel(ILogger<SuccessPageModel> logger)
        {
            _logger = logger;
        }

         public ProductModel NewProduct { get; set; }
        public List<string[]> MonteurResults { get; set; }

        public void OnGet()
        {
            // This method is executed when the page is requested via HTTP GET
        }

         public async void OnPostAddProductAsync()
        {
            try
            {
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                // Assuming you have a table named 'products' with columns 'product_name', 'price', etc.
                using (var cmd = new NpgsqlCommand("INSERT INTO products (product_name) VALUES (@productName)", conn))
                {
                    cmd.Parameters.AddWithValue("@productName", NewProduct.ProductName);
                    await cmd.ExecuteNonQueryAsync();
                }

                RedirectToPage("/SuccessPage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the add operation.");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                Page();
            }
        }

        public void OnPostBtnLogin_Click1(int index)
        {
            // Add your server-side logic here
            try
            {
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

                using var conn = new NpgsqlConnection(connString);
                conn.OpenAsync();

                using (var cmd = new NpgsqlCommand("DELETE FROM packages WHERE packageid = @packageId", conn))
                {
                    cmd.Parameters.AddWithValue("@packageID", index);
                    cmd.ExecuteNonQueryAsync();
                }

                // RedirectToPage("/SuccesPage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the remove operation.");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                Page();
            }
        }

    }

    public class ProductModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
