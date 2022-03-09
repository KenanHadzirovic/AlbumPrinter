using AlbumPrinter.DataAccess;
using AlbumPrinter.Models;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumPrinter.Tests.Repositories
{
    [TestClass]
    public class ProductTypeRepositoryTests
    {
        private readonly Fixture _fixture;

        public ProductTypeRepositoryTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
        }

        #region GetProductTypes
        [TestMethod]
        public async Task GetProductTypes_ShouldReturnEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "GetProductTypesEmpty")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            ProductTypeRepository repository = new ProductTypeRepository(context);

            // Act
            var result = await repository.GetProductTypes();

            // Assert
            result.Should().BeEquivalentTo(new List<ProductType>());
        }

        [TestMethod]
        public async Task GetProductTypes_ShouldReturnList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AlbumPrinterContext>()
                            .UseInMemoryDatabase(databaseName: "GetProductTypes")
                            .Options;
            AlbumPrinterContext context = new AlbumPrinterContext(options);
            ProductTypeRepository repository = new ProductTypeRepository(context);
            List<ProductType> productTypes = _fixture.CreateMany<ProductType>(2).ToList();
            context.ProductTypes.AddRange(productTypes);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetProductTypes();

            // Assert
            result.Should().BeEquivalentTo(productTypes);
        }
        #endregion GetProductTypes
    }
}
