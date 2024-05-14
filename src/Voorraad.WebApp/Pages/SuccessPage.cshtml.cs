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

        public List<string> ProductList { get; set; }

        // public void OnGet()
        // {
        //     // This method is executed when the page is requested via HTTP GET
        // }

        public async Task OnGetAsync()
        {
            try
            {
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                // Fetch the product names from the 'products' table
                var productList = new List<string>();
                using (var cmd = new NpgsqlCommand("SELECT product_name FROM products", conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            productList.Add(reader.GetString(0));
                        }
                    }
                }

                // Assign the product list to a property to be displayed in the view
                ProductList = productList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product data.");
                // Handle error, display message, or redirect as needed
            }
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

        public async Task OnPostBtnLogin_Click1(int index)
        {
            try
            {
                var connString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres";

                using var conn = new NpgsqlConnection(connString);
                await conn.OpenAsync();

                // Begin a transaction
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Create a command for deletion
                        using (var cmd = new NpgsqlCommand("DELETE FROM packages WHERE packageid = @packageId", conn))
                        {
                            // Assign parameter value
                            cmd.Parameters.AddWithValue("@packageId", index);

                            // Execute the deletion command
                            await cmd.ExecuteNonQueryAsync();
                        }

                        // Commit the transaction
                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        // Rollback the transaction in case of error
                        await transaction.RollbackAsync();
                        throw ex;
                    }
                }

                // Redirect to the same page or wherever appropriate
                RedirectToPage("/SuccessPage");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the remove operation.");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                // Optionally, handle the error here
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
