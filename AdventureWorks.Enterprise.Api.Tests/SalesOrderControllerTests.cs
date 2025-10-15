using Xunit;
using Moq;
using AdventureWorks.Enterprise.Api.Controllers;
using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace AdventureWorks.Enterprise.Api.Tests
{
    public class SalesOrderControllerTests
    {
        [Fact]
        public async Task GetOrder_NotFound_Returns404()
        {
            // Usar contexto InMemory para pruebas
            var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_SalesOrder_NotFound")
                .Options;
            var dbContext = new AdventureWorksDbContext(options);
            var controller = new SalesOrderController(dbContext);
            var result = await controller.FncConsultarOrden(-1);
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateOrder_InvalidModel_Returns400()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_SalesOrder_InvalidModel")
                .Options;
            var dbContext = new AdventureWorksDbContext(options);
            var controller = new SalesOrderController(dbContext);
            controller.ModelState.AddModelError("error", "error");
            var result = await controller.FncCrearOrden(new SalesOrderHeaderCreateDto());
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }
    }
}
