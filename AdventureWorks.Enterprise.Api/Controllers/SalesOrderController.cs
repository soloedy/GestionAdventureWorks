using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks.Enterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public SalesOrderController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Crea una nueva orden de venta usando el stored procedure sp_CreacionOrdenes
        /// </summary>
        /// <param name="orderDto">Datos de la orden a crear</param>
        /// <returns>Respuesta con el ID de la orden creada</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> FncCrearOrden(SalesOrderHeaderCreateDto orderDto)
        {
            try
            {
                // Verificar que el cliente existe
                var blnClienteExiste = await _context.Customers.AnyAsync(c => c.CustomerID == orderDto.CustomerID);
                if (!blnClienteExiste)
                {
                    return BadRequest(ApiResponse<int>.Error("El cliente especificado no existe."));
                }

                // Verificar que el vendedor existe si se especifica
                if (orderDto.SalesPersonID.HasValue)
                {
                    var blnVendedorExiste = await _context.SalesPersons.AnyAsync(s => s.BusinessEntityID == orderDto.SalesPersonID.Value);
                    if (!blnVendedorExiste)
                    {
                        return BadRequest(ApiResponse<int>.Error("El vendedor especificado no existe."));
                    }
                }

                // Verificar que los productos existen
                foreach (var detalle in orderDto.OrderDetails)
                {
                    var blnProductoExiste = await _context.Products.AnyAsync(p => p.ProductID == detalle.ProductID);
                    if (!blnProductoExiste)
                    {
                        return BadRequest(ApiResponse<int>.Error($"El producto con ID {detalle.ProductID} no existe."));
                    }
                }

                // Crear el parámetro de salida para el ID de la orden
                var paramSalesOrderID = new SqlParameter
                {
                    ParameterName = "@SalesOrderID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                // Crear la tabla temporal para los detalles
                var dtbDetalles = new DataTable();
                dtbDetalles.Columns.Add("CarrierTrackingNumber", typeof(string));
                dtbDetalles.Columns.Add("OrderQty", typeof(short));
                dtbDetalles.Columns.Add("ProductID", typeof(int));
                dtbDetalles.Columns.Add("SpecialOfferID", typeof(int));
                dtbDetalles.Columns.Add("UnitPrice", typeof(decimal));
                dtbDetalles.Columns.Add("UnitPriceDiscount", typeof(decimal));

                foreach (var detalle in orderDto.OrderDetails)
                {
                    dtbDetalles.Rows.Add(
                        detalle.CarrierTrackingNumber,
                        detalle.OrderQty,
                        detalle.ProductID,
                        detalle.SpecialOfferID,
                        detalle.UnitPrice,
                        detalle.UnitPriceDiscount
                    );
                }

                // Crear el parámetro para la tabla de detalles
                var paramDetalles = new SqlParameter
                {
                    ParameterName = "@OrderDetails",
                    SqlDbType = SqlDbType.Structured,
                    TypeName = "Sales.SalesOrderDetailType",
                    Value = dtbDetalles
                };

                // Ejecutar el stored procedure
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC [Sales].[sp_CreacionOrdenes] " +
                    "@RevisionNumber, @OrderDate, @DueDate, @ShipDate, @Status, @OnlineOrderFlag, " +
                    "@PurchaseOrderNumber, @CustomerID, @SalesPersonID, @TerritoryID, @BillToAddressID, " +
                    "@ShipToAddressID, @ShipMethodID, @CreditCardID, @CreditCardApprovalCode, @CurrencyRateID, " +
                    "@SubTotal, @TaxAmt, @Freight, @Comment, @OrderDetails, @SalesOrderID OUTPUT",
                    new SqlParameter("@RevisionNumber", orderDto.RevisionNumber),
                    new SqlParameter("@OrderDate", orderDto.OrderDate),
                    new SqlParameter("@DueDate", orderDto.DueDate),
                    new SqlParameter("@ShipDate", orderDto.ShipDate ?? (object)DBNull.Value),
                    new SqlParameter("@Status", orderDto.Status),
                    new SqlParameter("@OnlineOrderFlag", orderDto.OnlineOrderFlag),
                    new SqlParameter("@PurchaseOrderNumber", string.IsNullOrEmpty(orderDto.PurchaseOrderNumber) ? (object)DBNull.Value : orderDto.PurchaseOrderNumber),
                    new SqlParameter("@CustomerID", orderDto.CustomerID),
                    new SqlParameter("@SalesPersonID", orderDto.SalesPersonID ?? (object)DBNull.Value),
                    new SqlParameter("@TerritoryID", orderDto.TerritoryID ?? (object)DBNull.Value),
                    new SqlParameter("@BillToAddressID", orderDto.BillToAddressID),
                    new SqlParameter("@ShipToAddressID", orderDto.ShipToAddressID),
                    new SqlParameter("@ShipMethodID", orderDto.ShipMethodID),
                    new SqlParameter("@CreditCardID", orderDto.CreditCardID ?? (object)DBNull.Value),
                    new SqlParameter("@CreditCardApprovalCode", string.IsNullOrEmpty(orderDto.CreditCardApprovalCode) ? (object)DBNull.Value : orderDto.CreditCardApprovalCode),
                    new SqlParameter("@CurrencyRateID", orderDto.CurrencyRateID ?? (object)DBNull.Value),
                    new SqlParameter("@SubTotal", orderDto.SubTotal),
                    new SqlParameter("@TaxAmt", orderDto.TaxAmt),
                    new SqlParameter("@Freight", orderDto.Freight),
                    new SqlParameter("@Comment", string.IsNullOrEmpty(orderDto.Comment) ? (object)DBNull.Value : orderDto.Comment),
                    paramDetalles,
                    paramSalesOrderID
                );

                // Verificar si la operación fue exitosa
                int intSalesOrderID = (int)paramSalesOrderID.Value;
                if (intSalesOrderID <= 0)
                {
                    return BadRequest(ApiResponse<int>.Error("Error al crear la orden."));
                }

                return Ok(ApiResponse<int>.Success(intSalesOrderID, "Orden creada exitosamente."));
            }
            catch (SqlException sqlEx)
            {
                return StatusCode(500, ApiResponse<int>.Error("Error al ejecutar el stored procedure.", sqlEx.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<int>.Error("Error al crear la orden.", ex.Message));
            }
        }

        /// <summary>
        /// Anula una orden de venta cambiando su estado a cancelado
        /// </summary>
        /// <param name="updateStatusDto">DTO con el ID de la orden y el nuevo estado</param>
        /// <returns>Respuesta con el resultado de la operación</returns>
        [HttpPut("anular")]
        public async Task<ActionResult<ApiResponse<bool>>> FncAnularOrden(SalesOrderUpdateStatusDto updateStatusDto)
        {
            try
            {
                // Buscar la orden
                var objOrden = await _context.SalesOrderHeaders
                    .FirstOrDefaultAsync(o => o.SalesOrderID == updateStatusDto.SalesOrderID);

                if (objOrden == null)
                {
                    return NotFound(ApiResponse<bool>.Error($"No se encontró la orden con ID {updateStatusDto.SalesOrderID}"));
                }

                // Verificar si la orden ya está cancelada
                if (objOrden.Status == 6) // 6 = Cancelled en AdventureWorks
                {
                    return BadRequest(ApiResponse<bool>.Error("La orden ya está anulada."));
                }

                // Actualizar estado y comentario
                objOrden.Status = 6; // Cancelled
                objOrden.Comment = updateStatusDto.Comment ?? "Orden anulada";
                objOrden.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(ApiResponse<bool>.Success(true, "Orden anulada exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Error("Error al anular la orden.", ex.Message));
            }
        }

        /// <summary>
        /// Consulta una orden por su ID
        /// </summary>
        /// <param name="id">ID de la orden a consultar</param>
        /// <returns>Datos de la orden</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SalesOrderHeaderReadDto>>> FncConsultarOrden(int id)
        {
            try
            {
                // Buscar la orden con sus detalles
                var objOrden = await _context.SalesOrderHeaders
                    .Include(o => o.SalesOrderDetails)
                    .ThenInclude(d => d.Product)
                    .Include(o => o.Customer)
                    .Include(o => o.SalesPerson)
                    .FirstOrDefaultAsync(o => o.SalesOrderID == id);

                if (objOrden == null)
                {
                    return NotFound(ApiResponse<SalesOrderHeaderReadDto>.Error($"No se encontró la orden con ID {id}"));
                }

                // Mapear a DTO
                var ordenDto = new SalesOrderHeaderReadDto
                {
                    SalesOrderID = objOrden.SalesOrderID,
                    RevisionNumber = objOrden.RevisionNumber,
                    OrderDate = objOrden.OrderDate,
                    DueDate = objOrden.DueDate,
                    ShipDate = objOrden.ShipDate,
                    Status = objOrden.Status,
                    OnlineOrderFlag = objOrden.OnlineOrderFlag,
                    SalesOrderNumber = objOrden.SalesOrderNumber,
                    PurchaseOrderNumber = objOrden.PurchaseOrderNumber,
                    AccountNumber = objOrden.AccountNumber,
                    CustomerID = objOrden.CustomerID,
                    CustomerName = objOrden.Customer?.AccountNumber ?? "Cliente no disponible",
                    SalesPersonID = objOrden.SalesPersonID,
                    TerritoryID = objOrden.TerritoryID,
                    BillToAddressID = objOrden.BillToAddressID,
                    ShipToAddressID = objOrden.ShipToAddressID,
                    ShipMethodID = objOrden.ShipMethodID,
                    CreditCardID = objOrden.CreditCardID,
                    CreditCardApprovalCode = objOrden.CreditCardApprovalCode,
                    CurrencyRateID = objOrden.CurrencyRateID,
                    SubTotal = objOrden.SubTotal,
                    TaxAmt = objOrden.TaxAmt,
                    Freight = objOrden.Freight,
                    TotalDue = objOrden.TotalDue,
                    Comment = objOrden.Comment,
                    ModifiedDate = objOrden.ModifiedDate,
                    OrderDetails = objOrden.SalesOrderDetails.Select(d => new SalesOrderDetailReadDto
                    {
                        SalesOrderID = d.SalesOrderID,
                        SalesOrderDetailID = d.SalesOrderDetailID,
                        CarrierTrackingNumber = d.CarrierTrackingNumber,
                        OrderQty = d.OrderQty,
                        ProductID = d.ProductID,
                        ProductName = d.Product?.Name ?? "Producto no disponible",
                        SpecialOfferID = d.SpecialOfferID,
                        UnitPrice = d.UnitPrice,
                        UnitPriceDiscount = d.UnitPriceDiscount,
                        LineTotal = d.LineTotal
                    }).ToList()
                };

                return Ok(ApiResponse<SalesOrderHeaderReadDto>.Success(ordenDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SalesOrderHeaderReadDto>.Error("Error al consultar la orden.", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene el historial de órdenes por rango de fecha
        /// </summary>
        /// <param name="rangoFechasDto">Rango de fechas a consultar</param>
        /// <returns>Lista de órdenes en el rango de fecha especificado</returns>
        [HttpGet("historial")]
        public async Task<ActionResult<ApiResponse<List<SalesOrderHeaderReadDto>>>> FncObtenerHistorialOrdenes([FromQuery] SalesOrderDateRangeDto rangoFechasDto)
        {
            try
            {
                if (rangoFechasDto.EndDate < rangoFechasDto.StartDate)
                {
                    return BadRequest(ApiResponse<List<SalesOrderHeaderReadDto>>.Error("La fecha final debe ser mayor o igual a la fecha inicial."));
                }

                // Buscar órdenes en el rango de fechas
                var lstOrdenes = await _context.SalesOrderHeaders
                    .Include(o => o.Customer)
                    .Include(o => o.SalesPerson)
                    .Where(o => o.OrderDate >= rangoFechasDto.StartDate && o.OrderDate <= rangoFechasDto.EndDate)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(100) // Limitamos a 100 para evitar sobrecarga
                    .ToListAsync();

                // Mapear a DTOs
                var lstOrdenesDto = lstOrdenes.Select(o => new SalesOrderHeaderReadDto
                {
                    SalesOrderID = o.SalesOrderID,
                    RevisionNumber = o.RevisionNumber,
                    OrderDate = o.OrderDate,
                    DueDate = o.DueDate,
                    ShipDate = o.ShipDate,
                    Status = o.Status,
                    OnlineOrderFlag = o.OnlineOrderFlag,
                    SalesOrderNumber = o.SalesOrderNumber,
                    PurchaseOrderNumber = o.PurchaseOrderNumber,
                    AccountNumber = o.AccountNumber,
                    CustomerID = o.CustomerID,
                    CustomerName = o.Customer?.AccountNumber ?? "Cliente no disponible",
                    SalesPersonID = o.SalesPersonID,
                    SalesPersonName = o.SalesPerson != null ? $"ID: {o.SalesPerson.BusinessEntityID}" : "No asignado",
                    TerritoryID = o.TerritoryID,
                    BillToAddressID = o.BillToAddressID,
                    ShipToAddressID = o.ShipToAddressID,
                    ShipMethodID = o.ShipMethodID,
                    CreditCardID = o.CreditCardID,
                    CreditCardApprovalCode = o.CreditCardApprovalCode,
                    CurrencyRateID = o.CurrencyRateID,
                    SubTotal = o.SubTotal,
                    TaxAmt = o.TaxAmt,
                    Freight = o.Freight,
                    TotalDue = o.TotalDue,
                    Comment = o.Comment,
                    ModifiedDate = o.ModifiedDate
                }).ToList();

                return Ok(ApiResponse<List<SalesOrderHeaderReadDto>>.Success(lstOrdenesDto, 
                    $"Se encontraron {lstOrdenesDto.Count} órdenes en el período del {rangoFechasDto.StartDate:dd/MM/yyyy} al {rangoFechasDto.EndDate:dd/MM/yyyy}."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SalesOrderHeaderReadDto>>.Error(
                    "Error al consultar el historial de órdenes.", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene el top 10 de productos más vendidos
        /// </summary>
        /// <returns>Lista de los productos más vendidos</returns>
        [HttpGet("TopProducts")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TopProductDto>>>> FncObtenerTopProductos()
        {
            try
            {
                var lstTopProductos = await _context.Set<TopProductDto>()
                    .FromSqlRaw("EXEC [dbo].[sp_TopProductosVentas]")
                    .ToListAsync();

                return Ok(ApiResponse<IEnumerable<TopProductDto>>.Success(lstTopProductos, 
                    $"Se encontraron {lstTopProductos.Count} productos en el top de ventas."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<TopProductDto>>.Error(
                    "Error al consultar el top de productos más vendidos.", ex.Message));
            }
        }
    }
}