using AlbumPrinter.DataAccess;
using AlbumPrinter.Models;
using AlbumPrinter.Resources;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace AlbumPrinter.Tests.Repositories
{
    [TestClass]
    public class OrderRepositoryTests
    {
        private readonly Fixture _fixture;
        public OrderRepositoryTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }

        #region GetOrderInfo
        [TestMethod]
        public void GetOrderInfo_EmptyGuid_ShouldThrowArgumentException()
        {
            // Arrange
            Guid orderId = new Guid();
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "GetOrderInfoDb")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            OrderRepository repository = new OrderRepository(context);

            // Act
            var result = () => repository.GetOrderInfo(orderId);

            // Assert
            result.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task GetOrderInfo_ValidOrderId_ShouldReturnOrder()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "GetOrderInfoDb")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            OrderRepository repository = new OrderRepository(context);
            Guid orderId = Guid.NewGuid();
            Order expectedOrder = _fixture.Build<Order>()
                                          .With(x => x.OrderId, orderId)
                                          .Create(); 
            context.Orders.Add(expectedOrder);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetOrderInfo(orderId);

            // Assert
            result.Should().BeEquivalentTo(expectedOrder);
        }
        #endregion GetOrderInfo

        #region SaveOrder
        [TestMethod]
        public async Task SaveOrder_ExistingOrder_ShouldThrowArgumentException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "SaveOrder")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            OrderRepository repository = new OrderRepository(context);
            Guid orderId = Guid.NewGuid();
            Order expectedOrder = _fixture.Build<Order>()
                                          .With(x => x.OrderId, orderId)
                                          .Create();
            context.Orders.Add(expectedOrder);
            await context.SaveChangesAsync();

            // Act
            var result = () => repository.SaveOrder(expectedOrder);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(String.Format(DomainExceptionMessages.OrderExists, orderId));
        }

        [TestMethod]
        public async Task SaveOrder_NullOrder_ShouldThrowArgumentException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "SaveOrder")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            OrderRepository repository = new OrderRepository(context);

            // Act
            var result = () => repository.SaveOrder(null);

            // Assert
            result.Should().ThrowAsync<ArgumentNullException>();
        }

        [TestMethod]
        public async Task SaveOrder_ValidOrder_ShouldThrowArgumentException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "SaveOrder")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            OrderRepository repository = new OrderRepository(context);
            Guid orderId = Guid.NewGuid();
            Order expectedOrder = _fixture.Build<Order>()
                                          .With(x => x.OrderId, orderId)
                                          .Create();

            // Act
            var result = () => repository.SaveOrder(expectedOrder);

            // Assert
            result.Should().ThrowAsync<ArgumentException>().WithMessage(String.Format(DomainExceptionMessages.OrderExists, orderId));
        }
        #endregion SaveOrder

    }
}
