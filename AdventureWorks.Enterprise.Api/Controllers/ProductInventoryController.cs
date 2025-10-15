using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks.Enterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public ProductInventoryController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista el inventario de productos con opciones de filtrado y paginación
        /// </summary>
        /// <param name="filterDto">Filtros a aplicar</param>
        /// <returns>Lista de registros de inventario</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductInventoryReadDto>>>> FncListarInventario([FromQuery] ProductInventoryFilterDto filterDto)
        {
            try
            {
                // Validar parámetros de paginación
                if (filterDto.Page <= 0)
                    filterDto.Page = 1;

                if (filterDto.PageSize <= 0 || filterDto.PageSize > 100)
                    filterDto.PageSize = 10;

                // Consulta base
                var queryInventario = _context.ProductInventories
                    .Include(pi => pi.Product)
                    .AsQueryable();

                // Aplicar filtros
                if (filterDto.ProductID.HasValue)
                {
                    queryInventario = queryInventario.Where(pi => pi.ProductID == filterDto.ProductID.Value);
                }

                if (filterDto.LocationID.HasValue)
                {
                    queryInventario = queryInventario.Where(pi => pi.LocationID == filterDto.LocationID.Value);
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Shelf))
                {
                    queryInventario = queryInventario.Where(pi => pi.Shelf.Contains(filterDto.Shelf));
                }

                if (filterDto.MinQuantity.HasValue)
                {
                    queryInventario = queryInventario.Where(pi => pi.Quantity >= filterDto.MinQuantity.Value);
                }

                if (filterDto.MaxQuantity.HasValue)
                {
                    queryInventario = queryInventario.Where(pi => pi.Quantity <= filterDto.MaxQuantity.Value);
                }

                // Obtener total para paginación
                var intTotal = await queryInventario.CountAsync();

                // Aplicar paginación
                var lstInventario = await queryInventario
                    .OrderBy(pi => pi.ProductID)
                    .ThenBy(pi => pi.LocationID)
                    .Skip((filterDto.Page - 1) * filterDto.PageSize)
                    .Take(filterDto.PageSize)
                    .Select(pi => new ProductInventoryReadDto
                    {
                        ProductID = pi.ProductID,
                        ProductName = pi.Product.Name,
                        ProductNumber = pi.Product.ProductNumber,
                        LocationID = pi.LocationID,
                        Shelf = pi.Shelf,
                        Bin = pi.Bin,
                        Quantity = pi.Quantity,
                        ModifiedDate = pi.ModifiedDate
                    })
                    .ToListAsync();

                // Mensaje con información de filtros y paginación
                string strMensaje = $"Se encontraron {intTotal} registros de inventario";
                
                if (filterDto.ProductID.HasValue)
                {
                    strMensaje += $" para el producto ID {filterDto.ProductID.Value}";
                }
                
                strMensaje += $". Mostrando página {filterDto.Page} de {Math.Ceiling((double)intTotal / filterDto.PageSize)}.";

                return Ok(ApiResponse<IEnumerable<ProductInventoryReadDto>>.Success(lstInventario, strMensaje));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ProductInventoryReadDto>>.Error(
                    "Error al listar el inventario de productos.", ex.Message));
            }
        }

        /// <summary>
        /// Lista el inventario de un producto específico y ubicación
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <param name="locationid">ID de la ubicación</param>
        /// <returns>Registro de inventario del producto en la ubicación</returns>
        [HttpGet("product/{id}/{locationid}")]
        public async Task<ActionResult<ApiResponse<ProductInventoryReadDto>>> FncListarInventarioProducto(int id, short locationid)
        {
            try
            {
                // Verificar que el producto existe
                var blnProductoExiste = await _context.Products.AnyAsync(p => p.ProductID == id);
                if (!blnProductoExiste)
                {
                    return NotFound(ApiResponse<ProductInventoryReadDto>.Error(
                        $"No se encontró el producto con ID {id}"));
                }

                // Consultar inventario del producto en la ubicación
                var inventario = await _context.ProductInventories
                    .Include(pi => pi.Product)
                    .Where(pi => pi.ProductID == id && pi.LocationID == locationid)
                    .Select(pi => new ProductInventoryReadDto
                    {
                        ProductID = pi.ProductID,
                        ProductName = pi.Product.Name,
                        ProductNumber = pi.Product.ProductNumber,
                        LocationID = pi.LocationID,
                        Shelf = pi.Shelf,
                        Bin = pi.Bin,
                        Quantity = pi.Quantity,
                        ModifiedDate = pi.ModifiedDate
                    })
                    .FirstOrDefaultAsync();

                if (inventario == null)
                {
                    return Ok(ApiResponse<ProductInventoryReadDto>.Success(
                        null, $"No se encontró registro de inventario para el producto con ID {id} en la ubicación {locationid}"));
                }

                return Ok(ApiResponse<ProductInventoryReadDto>.Success(
                    inventario, $"Registro de inventario encontrado para el producto {inventario.ProductName} en la ubicación {locationid}"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductInventoryReadDto>.Error(
                    "Error al consultar el inventario del producto.", ex.Message));
            }
        }

        /// <summary>
        /// Actualiza el inventario de un producto
        /// </summary>
        /// <param name="updateDto">Datos para actualizar el inventario</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut]
        public async Task<ActionResult<ApiResponse<ProductInventoryReadDto>>> FncActualizarInventario(ProductInventoryUpdateDto updateDto)
        {
            try
            {
                // Verificar que el producto existe
                var blnProductoExiste = await _context.Products.AnyAsync(p => p.ProductID == updateDto.ProductID);
                if (!blnProductoExiste)
                {
                    return NotFound(ApiResponse<ProductInventoryReadDto>.Error(
                        $"No se encontró el producto con ID {updateDto.ProductID}"));
                }

                // Buscar registro de inventario
                var objInventario = await _context.ProductInventories
                    .FirstOrDefaultAsync(pi => pi.ProductID == updateDto.ProductID && pi.LocationID == updateDto.LocationID);

                if (objInventario == null)
                {
                    return NotFound(ApiResponse<ProductInventoryReadDto>.Error(
                        $"No se encontró registro de inventario para el producto {updateDto.ProductID} en la ubicación {updateDto.LocationID}"));
                }

                // Actualizar propiedades
                objInventario.Shelf = updateDto.Shelf;
                objInventario.Bin = updateDto.Bin;
                objInventario.Quantity = updateDto.Quantity;
                objInventario.ModifiedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                // Obtener producto para incluir en la respuesta
                var objProducto = await _context.Products.FindAsync(updateDto.ProductID);

                // Crear DTO para la respuesta
                var inventarioDto = new ProductInventoryReadDto
                {
                    ProductID = objInventario.ProductID,
                    ProductName = objProducto?.Name ?? "Producto desconocido",
                    ProductNumber = objProducto?.ProductNumber ?? "Desconocido",
                    LocationID = objInventario.LocationID,
                    Shelf = objInventario.Shelf,
                    Bin = objInventario.Bin,
                    Quantity = objInventario.Quantity,
                    ModifiedDate = objInventario.ModifiedDate
                };

                return Ok(ApiResponse<ProductInventoryReadDto>.Success(
                    inventarioDto, "Inventario actualizado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductInventoryReadDto>.Error(
                    "Error al actualizar el inventario.", ex.Message));
            }
        }

        /// <summary>
        /// Crea un nuevo registro en el inventario
        /// </summary>
        /// <param name="createDto">Datos para el nuevo registro</param>
        /// <returns>Registro creado</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductInventoryReadDto>>> FncCrearRegistroInventario(ProductInventoryCreateDto createDto)
        {
            try
            {
                // Verificar que el producto existe
                var objProducto = await _context.Products.FindAsync(createDto.ProductID);
                if (objProducto == null)
                {
                    return NotFound(ApiResponse<ProductInventoryReadDto>.Error(
                        $"No se encontró el producto con ID {createDto.ProductID}"));
                }

                // Verificar si ya existe un registro para ese producto y ubicación
                var blnExiste = await _context.ProductInventories.AnyAsync(
                    pi => pi.ProductID == createDto.ProductID && pi.LocationID == createDto.LocationID);

                if (blnExiste)
                {
                    return BadRequest(ApiResponse<ProductInventoryReadDto>.Error(
                        $"Ya existe un registro de inventario para el producto {createDto.ProductID} en la ubicación {createDto.LocationID}"));
                }

                // Crear nuevo registro de inventario
                var objNuevoInventario = new ProductInventory
                {
                    ProductID = createDto.ProductID,
                    LocationID = createDto.LocationID,
                    Shelf = createDto.Shelf,
                    Bin = createDto.Bin,
                    Quantity = createDto.Quantity,
                    RowGuid = Guid.NewGuid(),
                    ModifiedDate = DateTime.Now
                };

                _context.ProductInventories.Add(objNuevoInventario);
                await _context.SaveChangesAsync();

                // Crear DTO para la respuesta
                var inventarioDto = new ProductInventoryReadDto
                {
                    ProductID = objNuevoInventario.ProductID,
                    ProductName = objProducto.Name,
                    ProductNumber = objProducto.ProductNumber,
                    LocationID = objNuevoInventario.LocationID,
                    Shelf = objNuevoInventario.Shelf,
                    Bin = objNuevoInventario.Bin,
                    Quantity = objNuevoInventario.Quantity,
                    ModifiedDate = objNuevoInventario.ModifiedDate
                };

                return CreatedAtAction(nameof(FncListarInventarioProducto), new { id = createDto.ProductID, locationid = createDto.LocationID },
                    ApiResponse<ProductInventoryReadDto>.Success(inventarioDto, "Registro de inventario creado correctamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductInventoryReadDto>.Error(
                    "Error al crear el registro de inventario.", ex.Message));
            }
        }

        /// <summary>
        /// Obtiene productos con bajo inventario usando el stored procedure sp_BajoInventario
        /// </summary>
        /// <param name="cantidadLimite">Cantidad límite de inventario</param>
        /// <returns>Listado de productos con bajo inventario</returns>
        [HttpGet("low-inventory/{cantidadLimite}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductLowInventoryDto>>>> GetLowInventory(int cantidadLimite)
        {
            try
            {
                var result = await _context.Set<ProductLowInventoryDto>()
                    .FromSqlRaw("EXEC Production.sp_BajoInventario @CantidadLimite", new SqlParameter("@CantidadLimite", cantidadLimite))
                    .ToListAsync();
                return Ok(ApiResponse<IEnumerable<ProductLowInventoryDto>>.Success(result, "Productos con bajo inventario obtenidos correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<ProductLowInventoryDto>>.Error("Error al consultar bajo inventario.", ex.Message));
            }
        }
    }
}