using Xunit;
using AdventureWorks.Enterprise.Api.Controllers;
using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace AdventureWorks.Enterprise.Api.Tests
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task GetProduct_NotFound_Returns404()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Product_NotFound")
                .Options;
            var dbContext = new AdventureWorksDbContext(options);
            var controller = new ProductController(dbContext);
            var result = await controller.FncConsultarProducto(-1);
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_InvalidModel_Returns400()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Product_InvalidModel")
                .Options;
            var dbContext = new AdventureWorksDbContext(options);
            var controller = new ProductController(dbContext);
            controller.ModelState.AddModelError("error", "error");
            var result = await controller.FncCrearProducto(new ProductCreateDto());
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateProduct_Success_ReturnsOk()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Product_CreateSuccess")
                .Options;
            var dbContext = new AdventureWorksDbContext(options);
            // Se requiere al menos una subcategoría para crear el producto
            dbContext.ProductSubcategories.Add(new ProductSubcategory
            {
                ProductSubcategoryID = 1,
                ProductCategoryID = 1,
                Name = "TestSubcategory",
                RowGuid = Guid.NewGuid(),
                ModifiedDate = DateTime.Now
            });
            dbContext.SaveChanges();
            var controller = new ProductController(dbContext);
            var dto = new ProductCreateDto
            {
                Name = "TestProduct",
                ProductNumber = "TP-001",
                ListPrice = 100,
                ProductSubcategoryID = 1,
                SellStartDate = DateTime.Now
            };
            var result = await controller.FncCrearProducto(dto);
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetProduct_Success_ReturnsOk()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_Product_GetSuccess")
                .Options;
            var dbContext = new AdventureWorksDbContext(options);
            dbContext.Products.Add(new Product
            {
                ProductID = 1,
                Name = "TestProduct",
                ProductNumber = "TP-001",
                ListPrice = 100,
                SellStartDate = DateTime.Now,
                ProductSubcategoryID = null,
                MakeFlag = true,
                FinishedGoodsFlag = true,
                SafetyStockLevel = 10,
                ReorderPoint = 5,
                StandardCost = 50,
                DaysToManufacture = 1,
                RowGuid = Guid.NewGuid(),
                ModifiedDate = DateTime.Now
            });
            dbContext.SaveChanges();
            var controller = new ProductController(dbContext);
            var result = await controller.FncConsultarProducto(1);
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(200, objectResult.StatusCode);
        }
    }
}
