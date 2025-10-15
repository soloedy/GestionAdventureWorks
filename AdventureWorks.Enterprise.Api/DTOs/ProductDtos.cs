using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    // DTOs para salida
    public class ProductReadDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public bool MakeFlag { get; set; }
        public bool FinishedGoodsFlag { get; set; }
        public string Color { get; set; }
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string Size { get; set; }
        public string SizeUnitMeasureCode { get; set; }
        public string WeightUnitMeasureCode { get; set; }
        public decimal? Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string ProductLine { get; set; }
        public string Class { get; set; }
        public string Style { get; set; }
        public int? ProductSubcategoryID { get; set; }
        public string ProductSubcategoryName { get; set; } // Nombre de la subcategoría
        public int? ProductModelID { get; set; }
        public string ProductModelName { get; set; } // Nombre del modelo
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }

    // DTO para filtrar productos
    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; } // Hacer explícitamente opcional
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Color { get; set; } // Hacer explícitamente opcional
        public int? ProductSubcategoryID { get; set; }
        public int? ProductModelID { get; set; }
        public bool? IncludeDiscontinued { get; set; } = false;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    // DTO para crear productos
    public class ProductCreateDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(25)]
        public string ProductNumber { get; set; } = string.Empty;

        public string? Color { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ListPrice { get; set; }

        public string? Size { get; set; }

        [StringLength(3)]
        public string? SizeUnitMeasureCode { get; set; }

        [StringLength(3)]
        public string? WeightUnitMeasureCode { get; set; }

        public decimal? Weight { get; set; }

        public int? ProductSubcategoryID { get; set; }

        [Required]
        public DateTime SellStartDate { get; set; } = DateTime.Now;

        public bool MakeFlag { get; set; } = true;
        public bool FinishedGoodsFlag { get; set; } = true;
        public short SafetyStockLevel { get; set; } = 10;
        public short ReorderPoint { get; set; } = 5;
        public decimal StandardCost { get; set; } = 0;
        public int DaysToManufacture { get; set; } = 1;
        public string? ProductLine { get; set; }
        public string? Class { get; set; }
        public string? Style { get; set; }
        public int? ProductModelID { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
    }
}