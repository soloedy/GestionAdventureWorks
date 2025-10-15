using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks.Enterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesPersonController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public SalesPersonController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Consulta un vendedor por su ID
        /// </summary>
        /// <param name="id">ID del vendedor</param>
        /// <returns>Datos del vendedor</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<SalesPersonReadDto>>> FncConsultarVendedor(int id)
        {
            try
            {
                // Buscar el vendedor
                var objVendedor = await _context.SalesPersons
                    .FirstOrDefaultAsync(s => s.BusinessEntityID == id);

                if (objVendedor == null)
                {
                    return NotFound(ApiResponse<SalesPersonReadDto>.Error($"No se encontró el vendedor con ID {id}"));
                }

                // Mapear a DTO
                var vendedorDto = new SalesPersonReadDto
                {
                    BusinessEntityID = objVendedor.BusinessEntityID,
                    SalesPersonName = "Información no disponible", // En una implementación real se obtendría de Person
                    TerritoryID = objVendedor.TerritoryID,
                    TerritoryName = "Información no disponible", // En una implementación real se obtendría de SalesTerritory
                    SalesQuota = objVendedor.SalesQuota,
                    Bonus = objVendedor.Bonus,
                    CommissionPct = objVendedor.CommissionPct,
                    SalesYTD = objVendedor.SalesYTD,
                    SalesLastYear = objVendedor.SalesLastYear
                };

                return Ok(ApiResponse<SalesPersonReadDto>.Success(vendedorDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SalesPersonReadDto>.Error("Error al consultar el vendedor.", ex.Message));
            }
        }

        /// <summary>
        /// Lista las órdenes de un vendedor específico, con opción de filtro por fechas
        /// </summary>
        /// <param name="filterDto">Filtros para la consulta</param>
        /// <returns>Lista de órdenes del vendedor</returns>
        [HttpGet("orders")]
        public async Task<ActionResult<ApiResponse<SalesPersonOrdersDto>>> FncListarOrdenesPorVendedor([FromQuery] SalesPersonOrderFilterDto filterDto)
        {
            try
            {
                // Verificar que el vendedor existe
                var objVendedor = await _context.SalesPersons
                    .FirstOrDefaultAsync(s => s.BusinessEntityID == filterDto.SalesPersonID);

                if (objVendedor == null)
                {
                    return NotFound(ApiResponse<SalesPersonOrdersDto>.Error($"No se encontró el vendedor con ID {filterDto.SalesPersonID}"));
                }

                // Validar parámetros de paginación
                if (filterDto.Page <= 0)
                    filterDto.Page = 1;

                if (filterDto.PageSize <= 0 || filterDto.PageSize > 100)
                    filterDto.PageSize = 10;

                // Consulta base
                var queryOrdenes = _context.SalesOrderHeaders
                    .Where(o => o.SalesPersonID == filterDto.SalesPersonID);

                // Aplicar filtro por fechas si se proporcionan
                if (filterDto.StartDate.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(o => o.OrderDate >= filterDto.StartDate.Value);
                }

                if (filterDto.EndDate.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(o => o.OrderDate <= filterDto.EndDate.Value);
                }

                // Ordenar por fecha descendente
                queryOrdenes = queryOrdenes.OrderByDescending(o => o.OrderDate);

                // Contar total para paginación
                var intTotal = await queryOrdenes.CountAsync();

                // Aplicar paginación
                var lstOrdenes = await queryOrdenes
                    .Include(o => o.Customer)
                    .Skip((filterDto.Page - 1) * filterDto.PageSize)
                    .Take(filterDto.PageSize)
                    .ToListAsync();

                // Crear DTO de respuesta
                var objRespuesta = new SalesPersonOrdersDto
                {
                    SalesPersonID = objVendedor.BusinessEntityID,
                    SalesPersonName = "Información no disponible", // En una implementación real se obtendría de Person
                    Orders = lstOrdenes.Select(o => new SalesPersonOrderReadDto
                    {
                        SalesOrderID = o.SalesOrderID,
                        SalesOrderNumber = o.SalesOrderNumber,
                        OrderDate = o.OrderDate,
                        CustomerID = o.CustomerID,
                        CustomerName = o.Customer?.AccountNumber ?? "Cliente no disponible",
                        Status = o.Status,
                        TotalDue = o.TotalDue
                    }).ToList()
                };

                // Crear mensaje con información de los filtros aplicados
                string strMensaje = $"Se encontraron {intTotal} órdenes para el vendedor ID {objVendedor.BusinessEntityID}.";
                
                if (filterDto.StartDate.HasValue && filterDto.EndDate.HasValue)
                {
                    strMensaje += $" Filtro aplicado: desde {filterDto.StartDate.Value:dd/MM/yyyy} hasta {filterDto.EndDate.Value:dd/MM/yyyy}.";
                }
                else if (filterDto.StartDate.HasValue)
                {
                    strMensaje += $" Filtro aplicado: desde {filterDto.StartDate.Value:dd/MM/yyyy}.";
                }
                else if (filterDto.EndDate.HasValue)
                {
                    strMensaje += $" Filtro aplicado: hasta {filterDto.EndDate.Value:dd/MM/yyyy}.";
                }

                strMensaje += $" Mostrando página {filterDto.Page} de {Math.Ceiling((double)intTotal / filterDto.PageSize)}.";

                return Ok(ApiResponse<SalesPersonOrdersDto>.Success(objRespuesta, strMensaje));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SalesPersonOrdersDto>.Error("Error al listar las órdenes del vendedor.", ex.Message));
            }
        }
    }
}