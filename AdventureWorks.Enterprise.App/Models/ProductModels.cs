using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AdventureWorks.Enterprise.App.Models
{
    public class ProductReadDto
    {
        public int ProductID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public string? Color { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public int? ProductSubcategoryID { get; set; }
        public int? ProductCategoryID { get; set; }
        public DateTime SellStartDate { get; set; }
        public DateTime? SellEndDate { get; set; }
        public DateTime? DiscontinuedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ProductCreateDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required, StringLength(25)]
        public string ProductNumber { get; set; } = string.Empty;
        public string? Color { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public int? ProductSubcategoryID { get; set; }
        public int? ProductCategoryID { get; set; }
        public DateTime SellStartDate { get; set; }
    }

    public class ProductInventoryReadDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public short LocationID { get; set; }
        public string Shelf { get; set; } = string.Empty;
        public byte Bin { get; set; }
        public short Quantity { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class ProductInventoryUpdateDto
    {
        public int ProductID { get; set; }
        public short LocationID { get; set; }
        public string Shelf { get; set; } = string.Empty;
        public byte Bin { get; set; }
        public short Quantity { get; set; }
    }

    public class ProductCategoryReadDto
    {
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ProductCategoryCreateDto
    {
        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }

    public class WorkOrderReadDto
    {
        public int WorkOrderID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int OrderQty { get; set; }
        public int StockedQty { get; set; }
        public int ScrappedQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public short? ScrapReasonID { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class WorkOrderCreateDto
    {
        public int ProductID { get; set; }
        public int OrderQty { get; set; }
        public int ScrappedQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public short? ScrapReasonID { get; set; }
    }
}
