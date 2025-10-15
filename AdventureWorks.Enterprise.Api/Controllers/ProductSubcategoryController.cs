using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Enterprise.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductSubcategoryController : ControllerBase
    {
        private readonly AdventureWorksDbContext _context;

        public ProductSubcategoryController(AdventureWorksDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProductSubcategoryDto>>>> GetAll()
        {
            var list = await _context.ProductSubcategories.OrderBy(s => s.Name).ToListAsync();
            var dto = list.Select(s => new ProductSubcategoryDto { ProductSubcategoryID = s.ProductSubcategoryID, ProductCategoryID = s.ProductCategoryID, Name = s.Name }).ToList();
            return Ok(ApiResponse<IEnumerable<ProductSubcategoryDto>>.Success(dto));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ProductSubcategoryDto>>> Get(int id)
        {
            var obj = await _context.ProductSubcategories.FindAsync(id);
            if (obj == null) return NotFound(ApiResponse<ProductSubcategoryDto>.Error("Subcategoría no encontrada"));
            return Ok(ApiResponse<ProductSubcategoryDto>.Success(new ProductSubcategoryDto { ProductSubcategoryID = obj.ProductSubcategoryID, ProductCategoryID = obj.ProductCategoryID, Name = obj.Name }));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<ProductSubcategoryDto>>> Create(ProductSubcategoryCreateDto create)
        {
            var entity = new ProductSubcategory { ProductCategoryID = create.ProductCategoryID, Name = create.Name, RowGuid = Guid.NewGuid(), ModifiedDate = DateTime.Now };
            _context.ProductSubcategories.Add(entity);
            await _context.SaveChangesAsync();
            var dto = new ProductSubcategoryDto { ProductSubcategoryID = entity.ProductSubcategoryID, ProductCategoryID = entity.ProductCategoryID, Name = entity.Name };
            return CreatedAtAction(nameof(Get), new { id = entity.ProductSubcategoryID }, ApiResponse<ProductSubcategoryDto>.Success(dto, "Subcategoría creada"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<ProductSubcategoryDto>>> Update(int id, ProductSubcategoryCreateDto update)
        {
            var entity = await _context.ProductSubcategories.FindAsync(id);
            if (entity == null) return NotFound(ApiResponse<ProductSubcategoryDto>.Error("Subcategoría no encontrada"));
            entity.Name = update.Name;
            entity.ProductCategoryID = update.ProductCategoryID;
            entity.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            var dto = new ProductSubcategoryDto { ProductSubcategoryID = entity.ProductSubcategoryID, ProductCategoryID = entity.ProductCategoryID, Name = entity.Name };
            return Ok(ApiResponse<ProductSubcategoryDto>.Success(dto, "Subcategoría actualizada"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var entity = await _context.ProductSubcategories.FindAsync(id);
            if (entity == null) return NotFound(ApiResponse<object>.Error("Subcategoría no encontrada"));
            _context.ProductSubcategories.Remove(entity);
            await _context.SaveChangesAsync();
            return Ok(ApiResponse<object>.Success(null!, "Subcategoría eliminada"));
        }
    }

    public class ProductSubcategoryDto
    {
        public int ProductSubcategoryID { get; set; }
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ProductSubcategoryCreateDto
    {
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
