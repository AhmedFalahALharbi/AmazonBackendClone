using Dapper;
using Microsoft.Data.SqlClient;
using AmazonBackend.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace AmazonBackend.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId);
        Task<int> AddOrder(Order order);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Order>> GetOrdersByCustomerId(int customerId)
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId";

            var orders = await connection.QueryAsync<Order>(sql, new { CustomerId = customerId });
            return orders;
        }

        public async Task<int> AddOrder(Order order)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Insert the order and get the generated ID
                string insertOrderSql = @"
                    INSERT INTO Orders (CustomerId, OrderDate, TotalAmount)
                    VALUES (@CustomerId, @OrderDate, @TotalAmount);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

                int orderId = await connection.ExecuteScalarAsync<int>(
                    insertOrderSql, 
                    new { order.UserId, order.OrderDate, order.TotalAmount }, 
                    transaction
                );

                // Insert order items
                string insertOrderItemSql = @"
                    INSERT INTO OrderItems (OrderId, ProductId, Quantity, Price)
                    VALUES (@OrderId, @ProductId, @Quantity, @Price);";

                foreach (var item in order.OrderItems)
                {
                    await connection.ExecuteAsync(
                        insertOrderItemSql, 
                        new { OrderId = orderId, item.ProductId, item.Quantity, item.Price }, 
                        transaction
                    );
                }

                // Commit transaction
                await transaction.CommitAsync();
                return orderId;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
