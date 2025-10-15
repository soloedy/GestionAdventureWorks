using System;
using System.Collections.Generic;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class Product
    {
        public int ProductID { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? ProductNumber { get; set; } = string.Empty;
        public bool MakeFlag { get; set; }
        public bool FinishedGoodsFlag { get; set; }
        public string? Color { get; set; } = string.Empty;
        public short SafetyStockLevel { get; set; }
        public short ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; } = string.Empty;
        public string? SizeUnitMeasureCode { get; set; } = string.Empty;
        public string? WeightUnitMeasureCode { get; set; } = string.Empty;
        public decimal? Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string? ProductLine { get; set; } = string.Empty;
        public string? Class { get; set; } = string.Empty;
        public string? Style { get; set; } = string.Empty;
        public int? ProductSubcategoryID { get; set; }
        public int? ProductModelID { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navegación
        public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
        public ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();
        public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
        public ProductSubcategory? ProductSubcategory { get; set; }
    }
}