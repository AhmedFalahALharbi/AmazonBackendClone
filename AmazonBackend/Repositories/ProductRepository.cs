using Dapper;
using Microsoft.Data.SqlClient;
using AmazonBackend.Models;
using System.Data;
using System.Threading.Tasks;

namespace AmazonBackend.Repositories
{
    public interface IProductRepository
    {
        Task<Product?> GetProductById(int productId);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Product?> GetProductById(int productId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Products WHERE ProductId = @ProductId";

            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { ProductId = productId });
            return product;
        }
    }
}
