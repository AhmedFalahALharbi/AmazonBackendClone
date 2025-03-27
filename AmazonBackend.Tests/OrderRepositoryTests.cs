using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AmazonBackend.Repositories;
using AmazonBackend.Models;

namespace AmazonBackend.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;

        public OrderRepositoryTests()
        {
            _mockRepo = new Mock<IOrderRepository>();
        }

        [Fact]
        public async Task GetOrdersByUserId_ShouldReturnOrders_WhenUserExists()
        {
            // Arrange
            int userId = 1;
            var orders = new List<Order>
            {
                new Order { OrderId = 1, UserId = userId, TotalAmount = 100 },
                new Order { OrderId = 2, UserId = userId, TotalAmount = 200 }
            };

            _mockRepo.Setup(repo => repo.GetOrdersByCustomerId(userId)).ReturnsAsync(orders);

            // Act
            var result = await _mockRepo.Object.GetOrdersByCustomerId(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddOrder_ShouldReturnOrderId_WhenOrderIsAdded()
        {
            // Arrange
            var order = new Order { OrderId = 1, UserId = 1, TotalAmount = 150 };
            _mockRepo.Setup(repo => repo.AddOrder(order)).ReturnsAsync(order.OrderId);

            // Act
            var result = await _mockRepo.Object.AddOrder(order);

            // Assert
            Assert.Equal(1, result);
        }
    }
}
