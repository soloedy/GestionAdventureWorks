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
    public class ProductController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public ProductController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Consulta un producto por su ID
        /// </summary>
        /// <param name="id">ID del producto</param>
        /// <returns>Datos del producto</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductReadDto>>> FncConsultarProducto(int id)
        {
            try
            {
                // Buscar el producto
                var objProducto = await _context.Products
                    .FirstOrDefaultAsync(p => p.ProductID == id);

                if (objProducto == null)
                {
                    return NotFound(ApiResponse<ProductReadDto>.Error($"No se encontró el producto con ID {id}"));
                }

                // Mapear a DTO
                var productoDto = new ProductReadDto
                {
                    ProductID = objProducto.ProductID,
                    Name = objProducto.Name,
                    ProductNumber = objProducto.ProductNumber,
                    MakeFlag = objProducto.MakeFlag,
                    FinishedGoodsFlag = objProducto.FinishedGoodsFlag,
                    Color = objProducto.Color,
                    SafetyStockLevel = objProducto.SafetyStockLevel,
                    ReorderPoint = objProducto.ReorderPoint,
                    StandardCost = objProducto.StandardCost,
                    ListPrice = objProducto.ListPrice,
                    Size = objProducto.Size,
                    SizeUnitMeasureCode = objProducto.SizeUnitMeasureCode,
                    WeightUnitMeasureCode = objProducto.WeightUnitMeasureCode,
                    Weight = objProducto.Weight,
                    DaysToManufacture = objProducto.DaysToManufacture,
                    ProductLine = objProducto.ProductLine,
                    Class = objProducto.Class,
                    Style = objProducto.Style,
                    ProductSubcategoryID = objProducto.ProductSubcategoryID,
                    ProductSubcategoryName = "Información no disponible", // En una implementación real se obtendría de ProductSubcategory
                    ProductModelID = objProducto.ProductModelID,
                    ProductModelName = "Información no disponible", // En una implementación real se obtendría de ProductModel
                    SellStartDate = objProducto.SellStartDate,
                    SellEndDate = objProducto.SellEndDate,
                    DiscontinuedDate = objProducto.DiscontinuedDate
                };

                return Ok(ApiResponse<ProductReadDto>.Success(productoDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductReadDto>.Error("Error al consultar el producto.", ex.Message));
            }
        }

        /// <summary>
        /// Lista productos con opción de filtrado y paginación
        /// </summary>
        /// <param name="filterDto">Filtros y paginación</param>
        /// <returns>Lista de productos</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductReadDto>>>> FncListarProductos([FromQuery] ProductFilterDto filterDto)
        {
            try
            {
                // Validar parámetros de paginación
                if (filterDto.Page <= 0)
                    filterDto.Page = 1;

                if (filterDto.PageSize <= 0 || filterDto.PageSize > 100)
                    filterDto.PageSize = 10;

                // Consulta base
                IQueryable<Product> queryProductos = _context.Products;

                // Aplicar filtros
                if (!string.IsNullOrWhiteSpace(filterDto.SearchTerm))
                {
                    queryProductos = queryProductos.Where(p => 
                        p.Name.Contains(filterDto.SearchTerm) ||
                        p.ProductNumber.Contains(filterDto.SearchTerm));
                }

                if (filterDto.MinPrice.HasValue)
                {
                    queryProductos = queryProductos.Where(p => p.ListPrice >= filterDto.MinPrice.Value);
                }

                if (filterDto.MaxPrice.HasValue)
                {
                    queryProductos = queryProductos.Where(p => p.ListPrice <= filterDto.MaxPrice.Value);
                }

                if (!string.IsNullOrWhiteSpace(filterDto.Color))
                {
                    queryProductos = queryProductos.Where(p => p.Color == filterDto.Color);
                }

                if (filterDto.ProductSubcategoryID.HasValue)
                {
                    queryProductos = queryProductos.Where(p => p.ProductSubcategoryID == filterDto.ProductSubcategoryID.Value);
                }

                if (filterDto.ProductModelID.HasValue)
                {
                    queryProductos = queryProductos.Where(p => p.ProductModelID == filterDto.ProductModelID.Value);
                }

                // Por defecto excluir productos descontinuados a menos que se especifique lo contrario
                if (!filterDto.IncludeDiscontinued.HasValue || !filterDto.IncludeDiscontinued.Value)
                {
                    queryProductos = queryProductos.Where(p => !p.DiscontinuedDate.HasValue);
                }

                // Obtener el total de resultados para la paginación
                var intTotal = await queryProductos.CountAsync();

                // Ordenar y aplicar paginación
                var lstProductos = await queryProductos
                    .OrderBy(p => p.Name)
                    .Skip((filterDto.Page - 1) * filterDto.PageSize)
                    .Take(filterDto.PageSize)
                    .ToListAsync();

                // Validar si la consulta no devuelve resultados
                if (!lstProductos.Any())
                {
                    return Ok(ApiResponse<IEnumerable<ProductReadDto>>.Success(
                        Enumerable.Empty<ProductReadDto>(),
                        "No se encontraron productos que coincidan con los filtros aplicados."));
                }

                // Mapear a DTOs
                var lstProductosDto = lstProductos.Select(p => new ProductReadDto
                {
                    ProductID = p.ProductID,
                    Name = p.Name ?? "Nombre no disponible",
                    ProductNumber = p.ProductNumber ?? "Número no disponible",
                    MakeFlag = p.MakeFlag,
                    FinishedGoodsFlag = p.FinishedGoodsFlag,
                    Color = p.Color ?? "Color no disponible",
                    SafetyStockLevel = p.SafetyStockLevel,
                    ReorderPoint = p.ReorderPoint,
                    StandardCost = p.StandardCost,
                    ListPrice = p.ListPrice,
                    Size = p.Size ?? "Tamaño no disponible",
                    SizeUnitMeasureCode = p.SizeUnitMeasureCode ?? "Unidad no disponible",
                    WeightUnitMeasureCode = p.WeightUnitMeasureCode ?? "Unidad no disponible",
                    Weight = p.Weight,
                    DaysToManufacture = p.DaysToManufacture,
                    ProductLine = p.ProductLine ?? "Línea no disponible",
                    Class = p.Class ?? "Clase no disponible",
                    Style = p.Style ?? "Estilo no disponible",
                    ProductSubcategoryID = p.ProductSubcategoryID,
                    ProductSubcategoryName = "Información no disponible", // En una implementación real se obtendría de ProductSubcategory
                    ProductModelID = p.ProductModelID,
                    ProductModelName = "Información no disponible", // En una implementación real se obtendría de ProductModel
                    SellStartDate = p.SellStartDate,
                    SellEndDate = p.SellEndDate,
                    DiscontinuedDate = p.DiscontinuedDate
                }).ToList();

                // Crear mensaje con información de los filtros aplicados
                string strMensaje = $"Se encontraron {intTotal} productos.";
                
                if (!string.IsNullOrWhiteSpace(filterDto.SearchTerm))
                    strMensaje += $" Búsqueda: '{filterDto.SearchTerm}'.";

                strMensaje += $" Mostrando página {filterDto.Page} de {Math.Ceiling((double)intTotal / filterDto.PageSize)}.";

                return Ok(ApiResponse<IEnumerable<ProductReadDto>>.Success(lstProductosDto, strMensaje));
            }
            catch (Exception ex)
            {
                // Mejorar la observación para incluir el stack trace
                return StatusCode(500, ApiResponse<IEnumerable<ProductReadDto>>.Error(
                    "Error al listar los productos.",
                    $"{ex.Message} | StackTrace: {ex.StackTrace}")
                );
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        /// <param name="createDto">Datos del producto a crear</param>
        /// <returns>Producto creado</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductReadDto>>> FncCrearProducto([FromBody] ProductCreateDto createDto)
        {
            try
            {
                // Validar datos obligatorios
                if (string.IsNullOrWhiteSpace(createDto.Name) || string.IsNullOrWhiteSpace(createDto.ProductNumber))
                {
                    return BadRequest(ApiResponse<ProductReadDto>.Error("El nombre y el número de producto son obligatorios."));
                }

                // Validar y obtener subcategoría
                int? subcategoryId = createDto.ProductSubcategoryID;
                if (!subcategoryId.HasValue || !_context.ProductSubcategories.Any(s => s.ProductSubcategoryID == subcategoryId.Value))
                {
                    var defaultSubcategory = await _context.ProductSubcategories.FirstOrDefaultAsync();
                    if (defaultSubcategory == null)
                    {
                        return BadRequest(ApiResponse<ProductReadDto>.Error("No existe ninguna subcategoría en la base de datos. No se puede crear el producto."));
                    }
                    subcategoryId = defaultSubcategory.ProductSubcategoryID;
                }

                // Verificar y asignar unidad de medida de tamaño
                string? sizeUnitCode = null;
                if (!string.IsNullOrWhiteSpace(createDto.Size))
                {
                    if (string.IsNullOrWhiteSpace(createDto.SizeUnitMeasureCode))
                    {
                        // Si se especifica un tamaño pero no una unidad, buscar una unidad por defecto
                        var defaultUnit = await _context.UnitMeasures.FirstOrDefaultAsync();
                        if (defaultUnit == null)
                        {
                            return BadRequest(ApiResponse<ProductReadDto>.Error("No existen unidades de medida en la base de datos. No se puede crear el producto con tamaño especificado."));
                        }
                        sizeUnitCode = defaultUnit.UnitMeasureCode;
                    }
                    else
                    {
                        // Verificar que la unidad especificada existe
                        var unitExists = await _context.UnitMeasures.AnyAsync(u => u.UnitMeasureCode == createDto.SizeUnitMeasureCode);
                        if (!unitExists)
                        {
                            return BadRequest(ApiResponse<ProductReadDto>.Error($"La unidad de medida '{createDto.SizeUnitMeasureCode}' no existe en la base de datos."));
                        }
                        sizeUnitCode = createDto.SizeUnitMeasureCode;
                    }
                }

                // Verificar y asignar unidad de medida de peso
                string? weightUnitCode = null;
                if (createDto.Weight.HasValue)
                {
                    if (string.IsNullOrWhiteSpace(createDto.WeightUnitMeasureCode))
                    {
                        // Si se especifica un peso pero no una unidad, buscar una unidad por defecto
                        var defaultUnit = await _context.UnitMeasures.FirstOrDefaultAsync();
                        if (defaultUnit == null)
                        {
                            return BadRequest(ApiResponse<ProductReadDto>.Error("No existen unidades de medida en la base de datos. No se puede crear el producto con peso especificado."));
                        }
                        weightUnitCode = defaultUnit.UnitMeasureCode;
                    }
                    else
                    {
                        // Verificar que la unidad especificada existe
                        var unitExists = await _context.UnitMeasures.AnyAsync(u => u.UnitMeasureCode == createDto.WeightUnitMeasureCode);
                        if (!unitExists)
                        {
                            return BadRequest(ApiResponse<ProductReadDto>.Error($"La unidad de medida '{createDto.WeightUnitMeasureCode}' no existe en la base de datos."));
                        }
                        weightUnitCode = createDto.WeightUnitMeasureCode;
                    }
                }

                var nuevoProducto = new Product
                {
                    Name = createDto.Name,
                    ProductNumber = createDto.ProductNumber,
                    Color = createDto.Color,
                    ListPrice = createDto.ListPrice,
                    Size = createDto.Size,
                    SizeUnitMeasureCode = sizeUnitCode,
                    WeightUnitMeasureCode = weightUnitCode,
                    Weight = createDto.Weight,
                    ProductSubcategoryID = subcategoryId,
                    SellStartDate = createDto.SellStartDate,
                    MakeFlag = createDto.MakeFlag,
                    FinishedGoodsFlag = createDto.FinishedGoodsFlag,
                    SafetyStockLevel = createDto.SafetyStockLevel,
                    ReorderPoint = createDto.ReorderPoint,
                    StandardCost = createDto.StandardCost,
                    DaysToManufacture = createDto.DaysToManufacture,
                    ProductLine = createDto.ProductLine,
                    Class = createDto.Class,
                    Style = createDto.Style,
                    ProductModelID = createDto.ProductModelID,
                    SellEndDate = createDto.SellEndDate,
                    DiscontinuedDate = createDto.DiscontinuedDate,
                    RowGuid = Guid.NewGuid(),
                    ModifiedDate = DateTime.Now
                };

                _context.Products.Add(nuevoProducto);
                await _context.SaveChangesAsync();

                // Mapear a DTO de respuesta
                var productoDto = new ProductReadDto
                {
                    ProductID = nuevoProducto.ProductID,
                    Name = nuevoProducto.Name,
                    ProductNumber = nuevoProducto.ProductNumber,
                    MakeFlag = nuevoProducto.MakeFlag,
                    FinishedGoodsFlag = nuevoProducto.FinishedGoodsFlag,
                    Color = nuevoProducto.Color,
                    SafetyStockLevel = nuevoProducto.SafetyStockLevel,
                    ReorderPoint = nuevoProducto.ReorderPoint,
                    StandardCost = nuevoProducto.StandardCost,
                    ListPrice = nuevoProducto.ListPrice,
                    Size = nuevoProducto.Size,
                    SizeUnitMeasureCode = nuevoProducto.SizeUnitMeasureCode,
                    WeightUnitMeasureCode = nuevoProducto.WeightUnitMeasureCode,
                    Weight = nuevoProducto.Weight,
                    DaysToManufacture = nuevoProducto.DaysToManufacture,
                    ProductLine = nuevoProducto.ProductLine,
                    Class = nuevoProducto.Class,
                    Style = nuevoProducto.Style,
                    ProductSubcategoryID = nuevoProducto.ProductSubcategoryID,
                    ProductSubcategoryName = "Información no disponible",
                    ProductModelID = nuevoProducto.ProductModelID,
                    ProductModelName = "Información no disponible",
                    SellStartDate = nuevoProducto.SellStartDate,
                    SellEndDate = nuevoProducto.SellEndDate,
                    DiscontinuedDate = nuevoProducto.DiscontinuedDate
                };

                return Ok(ApiResponse<ProductReadDto>.Success(productoDto, "Producto creado correctamente."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ProductReadDto>.Error("Error al crear el producto.", ex.Message));
            }
        }
    }
}