using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    /// <summary>
    /// DTO para lectura de inventario de productos
    /// </summary>
    public class ProductInventoryReadDto
    {
        /// <summary>
        /// ID del producto
        /// </summary>
        public int ProductID { get; set; }
        
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// Número del producto
        /// </summary>
        public string ProductNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// ID de la ubicación
        /// </summary>
        public short LocationID { get; set; }
        
        /// <summary>
        /// Estante donde se encuentra el producto
        /// </summary>
        public string Shelf { get; set; } = string.Empty;
        
        /// <summary>
        /// Bin donde se encuentra el producto
        /// </summary>
        public byte Bin { get; set; }
        
        /// <summary>
        /// Cantidad disponible del producto
        /// </summary>
        public short Quantity { get; set; }
        
        /// <summary>
        /// Fecha de modificación
        /// </summary>
        public DateTime ModifiedDate { get; set; }
    }
    
    /// <summary>
    /// DTO para crear un nuevo registro de inventario
    /// </summary>
    public class ProductInventoryCreateDto
    {
        /// <summary>
        /// ID del producto
        /// </summary>
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int ProductID { get; set; }
        
        /// <summary>
        /// ID de la ubicación
        /// </summary>
        [Required(ErrorMessage = "El ID de la ubicación es obligatorio")]
        public short LocationID { get; set; }
        
        /// <summary>
        /// Estante donde se encuentra el producto
        /// </summary>
        [Required(ErrorMessage = "El estante es obligatorio")]
        [StringLength(10, ErrorMessage = "El estante no puede tener más de 10 caracteres")]
        public string Shelf { get; set; } = string.Empty;
        
        /// <summary>
        /// Bin donde se encuentra el producto
        /// </summary>
        [Required(ErrorMessage = "El bin es obligatorio")]
        public byte Bin { get; set; }
        
        /// <summary>
        /// Cantidad disponible del producto
        /// </summary>
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public short Quantity { get; set; }
    }
    
    /// <summary>
    /// DTO para actualizar un registro de inventario
    /// </summary>
    public class ProductInventoryUpdateDto
    {
        /// <summary>
        /// ID del producto
        /// </summary>
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int ProductID { get; set; }
        
        /// <summary>
        /// ID de la ubicación
        /// </summary>
        [Required(ErrorMessage = "El ID de la ubicación es obligatorio")]
        public short LocationID { get; set; }
        
        /// <summary>
        /// Estante donde se encuentra el producto
        /// </summary>
        [Required(ErrorMessage = "El estante es obligatorio")]
        [StringLength(10, ErrorMessage = "El estante no puede tener más de 10 caracteres")]
        public string Shelf { get; set; } = string.Empty;
        
        /// <summary>
        /// Bin donde se encuentra el producto
        /// </summary>
        [Required(ErrorMessage = "El bin es obligatorio")]
        public byte Bin { get; set; }
        
        /// <summary>
        /// Cantidad disponible del producto
        /// </summary>
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        public short Quantity { get; set; }
    }
    
    /// <summary>
    /// DTO para filtrar el inventario de productos
    /// </summary>
    public class ProductInventoryFilterDto
    {
        /// <summary>
        /// ID del producto para filtrar
        /// </summary>
        public int? ProductID { get; set; }
        
        /// <summary>
        /// ID de la ubicación para filtrar
        /// </summary>
        public short? LocationID { get; set; }
        
        /// <summary>
        /// Estante para filtrar
        /// </summary>
        public string? Shelf { get; set; }
        
        /// <summary>
        /// Cantidad mínima para filtrar
        /// </summary>
        public short? MinQuantity { get; set; }
        
        /// <summary>
        /// Cantidad máxima para filtrar
        /// </summary>
        public short? MaxQuantity { get; set; }
        
        /// <summary>
        /// Página actual para la paginación
        /// </summary>
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// Tamaño de página para la paginación
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}