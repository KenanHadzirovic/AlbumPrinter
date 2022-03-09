using AlbumPrinter.API.Controllers;
using AlbumPrinter.Dto;
using AlbumPrinter.Services.Interfaces;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace AlbumPrinter.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTests
    {
        private readonly OrderController _orderController;
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly Fixture _fixture;

        public OrderControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _orderController = new OrderController(_orderServiceMock.Object);
            _fixture = new Fixture();
        }

        #region GetOrderDetails
        [TestMethod]
        public async Task GetOrderDetails_ValidOrderId_ShouldReturnSuccess()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            OrderDto expectedOrder = _fixture.Create<OrderDto>();
            _orderServiceMock.Setup(_ => _.GetOrderInfo(orderId)).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderController.GetOrderDetails(orderId) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedOrder);
        }

        [TestMethod]
        public async Task GetOrderDetails_InValidOrderId_ShouldReturnBadRequest()
        {
            // Arrange
            Guid orderId = Guid.NewGuid();
            _orderServiceMock.Setup(_ => _.GetOrderInfo(orderId)).ThrowsAsync(new ArgumentException());

            // Act
            var result = await _orderController.GetOrderDetails(orderId) as BadRequestObjectResult;

            // Assert
            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task GetOrderDetails_InvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            Guid orderId = new Guid();
            _orderController.ModelState.AddModelError("Error", "Error message");

            // Act
            var result = await _orderController.GetOrderDetails(orderId) as BadRequestObjectResult;

            // Assert
            result.StatusCode.Should().Be(400);

            // Cleanup
            _orderController.ModelState.Clear();
        }
        #endregion GetOrderDetails

        #region CreateOrder
        [TestMethod]
        public async Task CreateOrder_ValidOrder_ShouldReturnSuccess()
        {
            // Arrange
            CreateOrderRequestDto createOrderRequest = _fixture.Create<CreateOrderRequestDto>();
            OrderBaseResponseDto expectedOrder = _fixture.Create<OrderBaseResponseDto>();
            _orderServiceMock.Setup(_ => _.CreateOrder(createOrderRequest)).ReturnsAsync(expectedOrder);

            // Act
            var result = await _orderController.CreateOrder(createOrderRequest) as OkObjectResult;

            // Assert
            result.Value.Should().BeEquivalentTo(expectedOrder);
        }

        [TestMethod]
        public async Task CreateOrder_InValidOrderArgument_ShouldReturnBadRequest()
        {
            // Arrange
            CreateOrderRequestDto createOrderRequest = _fixture.Create<CreateOrderRequestDto>();
            _orderServiceMock.Setup(_ => _.CreateOrder(createOrderRequest)).ThrowsAsync(new ArgumentException());

            // Act
            var result = await _orderController.CreateOrder(createOrderRequest) as BadRequestObjectResult;

            // Assert
            result.StatusCode.Should().Be(400);
        }

        [TestMethod]
        public async Task CreateOrder_InvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            CreateOrderRequestDto createOrderRequest = _fixture.Create<CreateOrderRequestDto>();
            _orderController.ModelState.AddModelError("Error", "Error message");

            // Act
            var result = await _orderController.CreateOrder(createOrderRequest) as BadRequestObjectResult;

            // Assert
            result.StatusCode.Should().Be(400);

            // Cleanup
            _orderController.ModelState.Clear();
        }
        #endregion CreateOrder
    }
}