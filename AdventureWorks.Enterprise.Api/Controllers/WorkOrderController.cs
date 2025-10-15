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
    public class WorkOrderController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public WorkOrderController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista las �rdenes de trabajo con opciones de filtrado y paginaci�n
        /// </summary>
        /// <param name="filterDto">Filtros a aplicar</param>
        /// <returns>Lista de �rdenes de trabajo</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorkOrderReadDto>>>> FncListarOrdenesTrabajoAsync([FromQuery] WorkOrderFilterDto filterDto)
        {
            try
            {
                // Validar par�metros de paginaci�n
                if (filterDto.Page <= 0)
                    filterDto.Page = 1;

                if (filterDto.PageSize <= 0 || filterDto.PageSize > 100)
                    filterDto.PageSize = 10;

                // Consulta base
                var queryOrdenes = _context.WorkOrders
                    .Include(wo => wo.Product)
                    .AsQueryable();

                // Aplicar filtros
                if (filterDto.ProductID.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(wo => wo.ProductID == filterDto.ProductID.Value);
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Status))
                {
                    // Aplicar filtro por estado
                    switch (filterDto.Status.ToLower())
                    {
                        case "pendiente":
                            queryOrdenes = queryOrdenes.Where(wo => wo.EndDate == null);
                            break;
                        case "completada":
                            queryOrdenes = queryOrdenes.Where(wo => wo.EndDate != null);
                            break;
                        case "atrasada":
                            queryOrdenes = queryOrdenes.Where(wo => wo.EndDate == null && wo.DueDate < DateTime.Now);
                            break;
                    }
                }

                if (filterDto.StartDateFrom.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(wo => wo.StartDate >= filterDto.StartDateFrom.Value);
                }

                if (filterDto.StartDateTo.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(wo => wo.StartDate <= filterDto.StartDateTo.Value);
                }

                if (filterDto.DueDateFrom.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(wo => wo.DueDate >= filterDto.DueDateFrom.Value);
                }

                if (filterDto.DueDateTo.HasValue)
                {
                    queryOrdenes = queryOrdenes.Where(wo => wo.DueDate <= filterDto.DueDateTo.Value);
                }

                // Obtener total para paginaci�n
                var intTotal = await queryOrdenes.CountAsync();

                // Aplicar paginaci�n
                var lstOrdenes = await queryOrdenes
                    .OrderByDescending(wo => wo.WorkOrderID)
                    .Skip((filterDto.Page - 1) * filterDto.PageSize)
                    .Take(filterDto.PageSize)
                    .Select(wo => new WorkOrderReadDto
                    {
                        WorkOrderID = wo.WorkOrderID,
                        ProductID = wo.ProductID,
                        ProductName = wo.Product.Name,
                        ProductNumber = wo.Product.ProductNumber,
                        OrderQty = wo.OrderQty,
                        StockedQty = wo.StockedQty,
                        ScrappedQty = wo.ScrappedQty,
                        StartDate = wo.StartDate,
                        EndDate = wo.EndDate,
                        DueDate = wo.DueDate,
                        ScrapReasonID = wo.ScrapReasonID,
                        ModifiedDate = wo.ModifiedDate,
                        Status = DeterminarEstado(wo)
                    })
                    .ToListAsync();

                // Mensaje con informaci�n de filtros y paginaci�n
                string strMensaje = $"Se encontraron {intTotal} �rdenes de trabajo";
                
                if (filterDto.ProductID.HasValue)
                {
                    strMensaje += $" para el producto ID {filterDto.ProductID.Value}";
                }
                
                if (!string.IsNullOrWhiteSpace(filterDto.Status))
                {
                    strMensaje += $" con estado {filterDto.Status}";
                }
                
                strMensaje += $". Mostrando p�gina {filterDto.Page} de {Math.Ceiling((double)intTotal / filterDto.PageSize)}.";

                return Ok(ApiResponse<IEnumerable<WorkOrderReadDto>>.Success(lstOrdenes, strMensaje));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<WorkOrderReadDto>>.Error(
                    "Error al listar las �rdenes de trabajo.", ex.Message));
            }
        }

        /// <summary>
        /// Crea una nueva orden de trabajo
        /// </summary>
        /// <param name="createDto">Datos de la orden a crear</param>
        /// <returns>Orden de trabajo creada</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkOrderReadDto>>> FncCrearOrdenTrabajo(WorkOrderCreateDto createDto)
        {
            try
            {
                // Verificar que el producto existe
                var objProducto = await _context.Products.FindAsync(createDto.ProductID);
                if (objProducto == null)
                {
                    return NotFound(ApiResponse<WorkOrderReadDto>.Error(
                        $"No se encontr� el producto con ID {createDto.ProductID}"));
                }

                // Validar fechas
                if (createDto.DueDate < createDto.StartDate)
                {
                    return BadRequest(ApiResponse<WorkOrderReadDto>.Error(
                        "La fecha de vencimiento no puede ser anterior a la fecha de inicio."));
                }

                if (createDto.EndDate.HasValue && createDto.EndDate.Value < createDto.StartDate)
                {
                    return BadRequest(ApiResponse<WorkOrderReadDto>.Error(
                        "La fecha de finalizaci�n no puede ser anterior a la fecha de inicio."));
                }

                // Crear nueva orden de trabajo
                var objNuevaOrden = new WorkOrder
                {
                    ProductID = createDto.ProductID,
                    OrderQty = createDto.OrderQty,
                    ScrappedQty = createDto.ScrappedQty,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    DueDate = createDto.DueDate,
                    ScrapReasonID = createDto.ScrapReasonID,
                    ModifiedDate = DateTime.Now
                };

                _context.WorkOrders.Add(objNuevaOrden);
                await _context.SaveChangesAsync();

                // Calcular el StockedQty despu�s de que la entidad se haya guardado
                // y se haya calculado el valor de la columna computada
                await _context.Entry(objNuevaOrden).ReloadAsync();

                // Crear DTO para la respuesta
                var ordenDto = new WorkOrderReadDto
                {
                    WorkOrderID = objNuevaOrden.WorkOrderID,
                    ProductID = objNuevaOrden.ProductID,
                    ProductName = objProducto.Name,
                    ProductNumber = objProducto.ProductNumber,
                    OrderQty = objNuevaOrden.OrderQty,
                    StockedQty = objNuevaOrden.StockedQty,
                    ScrappedQty = objNuevaOrden.ScrappedQty,
                    StartDate = objNuevaOrden.StartDate,
                    EndDate = objNuevaOrden.EndDate,
                    DueDate = objNuevaOrden.DueDate,
                    ScrapReasonID = objNuevaOrden.ScrapReasonID,
                    ModifiedDate = objNuevaOrden.ModifiedDate,
                    Status = DeterminarEstado(objNuevaOrden)
                };

                return CreatedAtAction(nameof(FncObtenerOrdenTrabajoAsync), new { id = objNuevaOrden.WorkOrderID },
                    ApiResponse<WorkOrderReadDto>.Success(ordenDto, "Orden de trabajo creada correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<WorkOrderReadDto>.Error(
                    "Error al crear la orden de trabajo.", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene una orden de trabajo por su ID
        /// </summary>
        /// <param name="id">ID de la orden de trabajo</param>
        /// <returns>Orden de trabajo</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkOrderReadDto>>> FncObtenerOrdenTrabajoAsync(int id)
        {
            try
            {
                var objOrden = await _context.WorkOrders
                    .Include(wo => wo.Product)
                    .FirstOrDefaultAsync(wo => wo.WorkOrderID == id);

                if (objOrden == null)
                {
                    return NotFound(ApiResponse<WorkOrderReadDto>.Error(
                        $"No se encontr� la orden de trabajo con ID {id}"));
                }

                var ordenDto = new WorkOrderReadDto
                {
                    WorkOrderID = objOrden.WorkOrderID,
                    ProductID = objOrden.ProductID,
                    ProductName = objOrden.Product?.Name ?? "Producto desconocido",
                    ProductNumber = objOrden.Product?.ProductNumber ?? "Desconocido",
                    OrderQty = objOrden.OrderQty,
                    StockedQty = objOrden.StockedQty,
                    ScrappedQty = objOrden.ScrappedQty,
                    StartDate = objOrden.StartDate,
                    EndDate = objOrden.EndDate,
                    DueDate = objOrden.DueDate,
                    ScrapReasonID = objOrden.ScrapReasonID,
                    ModifiedDate = objOrden.ModifiedDate,
                    Status = DeterminarEstado(objOrden)
                };

                return Ok(ApiResponse<WorkOrderReadDto>.Success(ordenDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<WorkOrderReadDto>.Error(
                    "Error al obtener la orden de trabajo.", ex.Message));
            }
        }

        /// <summary>
        /// Actualiza una orden de trabajo
        /// </summary>
        /// <param name="updateDto">Datos para actualizar la orden</param>
        /// <returns>Resultado de la operaci�n</returns>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<WorkOrderReadDto>>> FncActualizarOrdenTrabajo(WorkOrderUpdateDto updateDto)
        {
            try
            {
                var objOrden = await _context.WorkOrders
                    .Include(wo => wo.Product)
                    .FirstOrDefaultAsync(wo => wo.WorkOrderID == updateDto.WorkOrderID);

                if (objOrden == null)
                {
                    return NotFound(ApiResponse<WorkOrderReadDto>.Error(
                        $"No se encontr� la orden de trabajo con ID {updateDto.WorkOrderID}"));
                }

                // Validar fechas
                if (updateDto.DueDate < objOrden.StartDate)
                {
                    return BadRequest(ApiResponse<WorkOrderReadDto>.Error(
                        "La fecha de vencimiento no puede ser anterior a la fecha de inicio."));
                }

                if (updateDto.EndDate.HasValue && updateDto.EndDate.Value < objOrden.StartDate)
                {
                    return BadRequest(ApiResponse<WorkOrderReadDto>.Error(
                        "La fecha de finalizaci�n no puede ser anterior a la fecha de inicio."));
                }

                // Actualizar propiedades
                objOrden.OrderQty = updateDto.OrderQty;
                objOrden.ScrappedQty = updateDto.ScrappedQty;
                objOrden.EndDate = updateDto.EndDate;
                objOrden.DueDate = updateDto.DueDate;
                objOrden.ScrapReasonID = updateDto.ScrapReasonID;
                objOrden.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                // Actualizar el StockedQty despu�s de que la entidad se haya guardado
                await _context.Entry(objOrden).ReloadAsync();

                // Crear DTO para la respuesta
                var ordenDto = new WorkOrderReadDto
                {
                    WorkOrderID = objOrden.WorkOrderID,
                    ProductID = objOrden.ProductID,
                    ProductName = objOrden.Product?.Name ?? "Producto desconocido",
                    ProductNumber = objOrden.Product?.ProductNumber ?? "Desconocido",
                    OrderQty = objOrden.OrderQty,
                    StockedQty = objOrden.StockedQty,
                    ScrappedQty = objOrden.ScrappedQty,
                    StartDate = objOrden.StartDate,
                    EndDate = objOrden.EndDate,
                    DueDate = objOrden.DueDate,
                    ScrapReasonID = objOrden.ScrapReasonID,
                    ModifiedDate = objOrden.ModifiedDate,
                    Status = DeterminarEstado(objOrden)
                };

                return Ok(ApiResponse<WorkOrderReadDto>.Success(
                    ordenDto, "Orden de trabajo actualizada correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<WorkOrderReadDto>.Error(
                    "Error al actualizar la orden de trabajo.", ex.Message));
            }
        }

        /// <summary>
        /// Elimina una orden de trabajo
        /// </summary>
        /// <param name="id">ID de la orden a eliminar</param>
        /// <returns>Resultado de la operaci�n</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> FncEliminarOrdenTrabajo(int id)
        {
            try
            {
                var objOrden = await _context.WorkOrders.FindAsync(id);
                if (objOrden == null)
                {
                    return NotFound(ApiResponse<bool>.Error(
                        $"No se encontr� la orden de trabajo con ID {id}"));
                }

                // Verificar si se puede eliminar (por ejemplo, si a�n no ha comenzado)
                if (objOrden.EndDate.HasValue)
                {
                    return BadRequest(ApiResponse<bool>.Error(
                        "No se puede eliminar una orden de trabajo que ya ha sido completada."));
                }

                _context.WorkOrders.Remove(objOrden);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse<bool>.Success(true, $"Orden de trabajo {id} eliminada correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.Error(
                    "Error al eliminar la orden de trabajo.", ex.Message));
            }
        }

        /// <summary>
        /// Determina el estado de una orden de trabajo
        /// </summary>
        /// <param name="orden">Orden de trabajo</param>
        /// <returns>Estado como texto</returns>
        private static string DeterminarEstado(WorkOrder orden)
        {
            if (orden.EndDate.HasValue)
            {
                return "Completada";
            }
            else if (orden.DueDate < DateTime.Now)
            {
                return "Atrasada";
            }
            else
            {
                return "Pendiente";
            }
        }
    }
}