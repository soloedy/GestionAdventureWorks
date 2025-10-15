using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Enterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public ProductCategoryController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductCategoryDto>>>> GetAll()
        {
            var list = await _context.Set<ProductCategory>().OrderBy(c => c.Name).ToListAsync();
            var dto = list.Select(c => new ProductCategoryDto { ProductCategoryID = c.ProductCategoryID, Name = c.Name }).ToList();
            return Ok(ApiResponse<IEnumerable<ProductCategoryDto>>.Success(dto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductCategoryDto>>> Get(int id)
        {
            var obj = await _context.Set<ProductCategory>().FindAsync(id);
            if (obj == null) return NotFound(ApiResponse<ProductCategoryDto>.Error("Categoría no encontrada"));
            return Ok(ApiResponse<ProductCategoryDto>.Success(new ProductCategoryDto { ProductCategoryID = obj.ProductCategoryID, Name = obj.Name }));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductCategoryDto>>> Create(ProductCategoryCreateDto create)
        {
            var entity = new ProductCategory { Name = create.Name, RowGuid = Guid.NewGuid(), ModifiedDate = DateTime.Now };
            _context.Set<ProductCategory>().Add(entity);
            await _context.SaveChangesAsync();
            var dto = new ProductCategoryDto { ProductCategoryID = entity.ProductCategoryID, Name = entity.Name };
            return CreatedAtAction(nameof(Get), new { id = entity.ProductCategoryID }, ApiResponse<ProductCategoryDto>.Success(dto, "Categoría creada"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductCategoryDto>>> Update(int id, ProductCategoryCreateDto update)
        {
            var entity = await _context.Set<ProductCategory>().FindAsync(id);
            if (entity == null) return NotFound(ApiResponse<ProductCategoryDto>.Error("Categoría no encontrada"));
            entity.Name = update.Name;
            entity.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            var dto = new ProductCategoryDto { ProductCategoryID = entity.ProductCategoryID, Name = entity.Name };
            return Ok(ApiResponse<ProductCategoryDto>.Success(dto, "Categoría actualizada"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var entity = await _context.Set<ProductCategory>().FindAsync(id);
            if (entity == null) return NotFound(ApiResponse<object>.Error("Categoría no encontrada"));
            // Optional: check for related subcategories/products
            _context.Set<ProductCategory>().Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<object>.Success(null!, "Categoría eliminada"));
        }
    }

    public class ProductCategoryDto
    {
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ProductCategoryCreateDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
