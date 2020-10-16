using System;
using Herokufy.Data;
using Herokufy.Models;
using Herokufy.Models.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HerokufyUnitTest
{
    public class InventoryManagerTest
    {
        [Fact]
        public void GetterSetterProduct()
        {
            Product testProduct = new Product();

            Assert.Null(testProduct.Name);

            testProduct.Name = "Test Product";
            var actual = "Test Product";

            Assert.Equal(testProduct.Name, actual);
        }

        [Fact]
        public async void CanGetProductsFromDatabase()
        {
            DbContextOptions<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>()
                .UseInMemoryDatabase("CanGetProductsFromDatabase")
                .Options;

            using StoreDbContext context = new StoreDbContext(options);

            InventoryManager service = new InventoryManager(context);

            Assert.Equal(0, context.Products.CountAsync().Result);

            _ = await service.CreateProduct(new Product() { Name = "Test One" });

            Assert.Equal(1, context.Products.CountAsync().Result);

            _ = await service.CreateProduct(new Product() { Name = "Test Two" });

            Assert.Equal(2, context.Products.CountAsync().Result);

            var expected = context.Products.ToListAsync().Result;
            var actual = await service.GetProducts();

            Assert.Equal(expected, actual);
        }
    }
}
