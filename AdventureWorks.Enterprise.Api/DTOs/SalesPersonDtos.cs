using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    // DTOs para salida
    public class SalesPersonReadDto
    {
        public int BusinessEntityID { get; set; }
        public string SalesPersonName { get; set; } // Nombre del vendedor (desde Person)
        public int? TerritoryID { get; set; }
        public string TerritoryName { get; set; } // Nombre del territorio
        public decimal? SalesQuota { get; set; }
        public decimal Bonus { get; set; }
        public decimal CommissionPct { get; set; }
        public decimal SalesYTD { get; set; }
        public decimal SalesLastYear { get; set; }
    }

    // DTO para mostrar órdenes de un vendedor
    public class SalesPersonOrdersDto
    {
        public int SalesPersonID { get; set; }
        public string SalesPersonName { get; set; }
        public List<SalesPersonOrderReadDto> Orders { get; set; }
    }

    public class SalesPersonOrderReadDto
    {
        public int SalesOrderID { get; set; }
        public string SalesOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public byte Status { get; set; }
        public decimal TotalDue { get; set; }
    }

    // DTO para filtrar órdenes por vendedor y fecha
    public class SalesPersonOrderFilterDto
    {
        [Required(ErrorMessage = "El ID del vendedor es obligatorio")]
        public int SalesPersonID { get; set; }
        
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}