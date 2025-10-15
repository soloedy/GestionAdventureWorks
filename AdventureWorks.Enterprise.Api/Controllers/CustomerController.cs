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
    public class CustomerController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public CustomerController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Registra un nuevo cliente
        /// </summary>
        /// <param name="customerDto">Datos del cliente a registrar</param>
        /// <returns>Datos del cliente registrado</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CustomerReadDto>>> FncRegistrarCliente(CustomerCreateDto customerDto)
        {
            try
            {
                // Verificar si se proporcionó al menos un identificador válido
                if (!customerDto.PersonID.HasValue && !customerDto.StoreID.HasValue)
                {
                    return BadRequest(ApiResponse<CustomerReadDto>.Error("Se debe proporcionar al menos un PersonID o StoreID válido."));
                }

                // Verificar que la persona existe si se proporciona el ID
                if (customerDto.PersonID.HasValue)
                {
                    // Aquí iría la validación de la existencia de la persona
                    // Este es un ejemplo simulado - en una implementación real se verificaría contra la tabla Person.Person
                    // var personExists = await _context.Personas.AnyAsync(p => p.BusinessEntityID == customerDto.PersonID.Value);
                    // if (!personExists)
                    // {
                    //     return BadRequest(ApiResponse<CustomerReadDto>.Error($"La persona con ID {customerDto.PersonID.Value} no existe."));
                    // }
                }

                // Crear nuevo cliente
                var objNuevoCliente = new Customer
                {
                    PersonID = customerDto.PersonID,
                    StoreID = customerDto.StoreID,
                    TerritoryID = customerDto.TerritoryID,
                    RowGuid = Guid.NewGuid(),
                    ModifiedDate = DateTime.Now
                };

                _context.Customers.Add(objNuevoCliente);
                await _context.SaveChangesAsync();

                // Mapear a DTO de respuesta
                var clienteDto = new CustomerReadDto
                {
                    CustomerID = objNuevoCliente.CustomerID,
                    PersonID = objNuevoCliente.PersonID,
                    PersonName = "Información no disponible", // En una implementación real se obtendría de Person
                    StoreID = objNuevoCliente.StoreID,
                    StoreName = "Información no disponible", // En una implementación real se obtendría de Store
                    TerritoryID = objNuevoCliente.TerritoryID,
                    TerritoryName = "Información no disponible", // En una implementación real se obtendría de SalesTerritory
                    AccountNumber = objNuevoCliente.AccountNumber,
                    ModifiedDate = objNuevoCliente.ModifiedDate
                };

                return CreatedAtAction(nameof(FncConsultarCliente), new { id = clienteDto.CustomerID }, 
                    ApiResponse<CustomerReadDto>.Success(clienteDto, "Cliente registrado exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CustomerReadDto>.Error("Error al registrar el cliente.", ex.Message));
            }
        }

        /// <summary>
        /// Consulta un cliente por su ID
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Datos del cliente</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CustomerReadDto>>> FncConsultarCliente(int id)
        {
            try
            {
                // Buscar el cliente
                var objCliente = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == id);

                if (objCliente == null)
                {
                    return NotFound(ApiResponse<CustomerReadDto>.Error($"No se encontró el cliente con ID {id}"));
                }

                // Mapear a DTO
                var clienteDto = new CustomerReadDto
                {
                    CustomerID = objCliente.CustomerID,
                    PersonID = objCliente.PersonID,
                    PersonName = "Información no disponible", // En una implementación real se obtendría de Person
                    StoreID = objCliente.StoreID,
                    StoreName = "Información no disponible", // En una implementación real se obtendría de Store
                    TerritoryID = objCliente.TerritoryID,
                    TerritoryName = "Información no disponible", // En una implementación real se obtendría de SalesTerritory
                    AccountNumber = objCliente.AccountNumber,
                    ModifiedDate = objCliente.ModifiedDate
                };

                return Ok(ApiResponse<CustomerReadDto>.Success(clienteDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CustomerReadDto>.Error("Error al consultar el cliente.", ex.Message));
            }
        }

        /// <summary>
        /// Actualiza los datos de un cliente
        /// </summary>
        /// <param name="customerDto">Datos del cliente a actualizar</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> FncActualizarCliente(CustomerUpdateDto customerDto)
        {
            try
            {
                // Buscar el cliente
                var objCliente = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == customerDto.CustomerID);

                if (objCliente == null)
                {
                    return NotFound(ApiResponse<bool>.Error($"No se encontró el cliente con ID {customerDto.CustomerID}"));
                }

                // Actualizar propiedades
                objCliente.PersonID = customerDto.PersonID;
                objCliente.StoreID = customerDto.StoreID;
                objCliente.TerritoryID = customerDto.TerritoryID;
                objCliente.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                return Ok(ApiResponse<bool>.Success(true, "Cliente actualizado exitosamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Error("Error al actualizar el cliente.", ex.Message));
            }
        }

        /// <summary>
        /// Lista los clientes con opción de filtrado y paginación
        /// </summary>
        /// <param name="filterDto">Filtros y paginación</param>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CustomerReadDto>>>> FncListarClientes([FromQuery] CustomerFilterDto filterDto)
        {
            try
            {
                // Validar parámetros de paginación
                if (filterDto.Page <= 0)
                    filterDto.Page = 1;

                if (filterDto.PageSize <= 0 || filterDto.PageSize > 100)
                    filterDto.PageSize = 10;

                // Consulta base
                IQueryable<Customer> queryClientes = _context.Customers;

                // Aplicar filtro si se proporciona término de búsqueda
                if (!string.IsNullOrWhiteSpace(filterDto.SearchTerm))
                {
                    queryClientes = queryClientes.Where(c => 
                        c.AccountNumber.Contains(filterDto.SearchTerm));
                }

                // Obtener el total de resultados para la paginación
                var intTotal = await queryClientes.CountAsync();

                // Aplicar paginación
                var lstClientes = await queryClientes
                    .Skip((filterDto.Page - 1) * filterDto.PageSize)
                    .Take(filterDto.PageSize)
                    .ToListAsync();

                // Mapear a DTOs
                var lstClientesDto = lstClientes.Select(c => new CustomerReadDto
                {
                    CustomerID = c.CustomerID,
                    PersonID = c.PersonID,
                    PersonName = "Información no disponible", // En una implementación real se obtendría de Person
                    StoreID = c.StoreID,
                    StoreName = "Información no disponible", // En una implementación real se obtendría de Store
                    TerritoryID = c.TerritoryID,
                    TerritoryName = "Información no disponible", // En una implementación real se obtendría de SalesTerritory
                    AccountNumber = c.AccountNumber,
                    ModifiedDate = c.ModifiedDate
                }).ToList();

                return Ok(ApiResponse<IEnumerable<CustomerReadDto>>.Success(lstClientesDto, 
                    $"Se encontraron {intTotal} clientes. Mostrando página {filterDto.Page} de {Math.Ceiling((double)intTotal / filterDto.PageSize)}."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<CustomerReadDto>>.Error("Error al listar los clientes.", ex.Message));
            }
        }

        /// <summary>
        /// Lista las órdenes de un cliente específico
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <returns>Lista de órdenes del cliente</returns>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<ApiResponse<CustomerOrdersDto>>> FncListarOrdenesPorCliente(int id)
        {
            try
            {
                // Verificar que el cliente existe
                var objCliente = await _context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerID == id);

                if (objCliente == null)
                {
                    return NotFound(ApiResponse<CustomerOrdersDto>.Error($"No se encontró el cliente con ID {id}"));
                }

                // Buscar las órdenes del cliente
                var lstOrdenes = await _context.SalesOrderHeaders
                    .Where(o => o.CustomerID == id)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                // Crear DTO de respuesta
                var objRespuesta = new CustomerOrdersDto
                {
                    CustomerID = objCliente.CustomerID,
                    AccountNumber = objCliente.AccountNumber,
                    CustomerName = "Información no disponible", // En una implementación real se obtendría de Person o Store
                    Orders = lstOrdenes.Select(o => new CustomerOrderReadDto
                    {
                        SalesOrderID = o.SalesOrderID,
                        SalesOrderNumber = o.SalesOrderNumber,
                        OrderDate = o.OrderDate,
                        DueDate = o.DueDate,
                        Status = o.Status,
                        TotalDue = o.TotalDue
                    }).ToList()
                };

                return Ok(ApiResponse<CustomerOrdersDto>.Success(objRespuesta, 
                    $"Se encontraron {objRespuesta.Orders.Count} órdenes para el cliente {objRespuesta.AccountNumber}."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CustomerOrdersDto>.Error("Error al listar las órdenes del cliente.", ex.Message));
            }
        }
    }
}