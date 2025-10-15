using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    // DTOs para entrada
    public class CustomerCreateDto
    {
        public int? PersonID { get; set; }
        public int? StoreID { get; set; }
        public int? TerritoryID { get; set; }
    }

    public class CustomerUpdateDto
    {
        [Required(ErrorMessage = "El CustomerID es obligatorio")]
        public int CustomerID { get; set; }
        public int? PersonID { get; set; }
        public int? StoreID { get; set; }
        public int? TerritoryID { get; set; }
    }

    // DTOs para salida
    public class CustomerReadDto
    {
        public int CustomerID { get; set; }
        public int? PersonID { get; set; }
        public string PersonName { get; set; } // Campo adicional para mostrar el nombre de la persona
        public int? StoreID { get; set; }
        public string StoreName { get; set; } // Campo adicional para mostrar el nombre de la tienda
        public int? TerritoryID { get; set; }
        public string TerritoryName { get; set; } // Campo adicional para mostrar el nombre del territorio
        public string AccountNumber { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    // DTO para listar clientes con paginación
    public class CustomerFilterDto
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
    }

    // DTO para mostrar órdenes de un cliente
    public class CustomerOrdersDto
    {
        public int CustomerID { get; set; }
        public string AccountNumber { get; set; }
        public string CustomerName { get; set; }
        public List<CustomerOrderReadDto> Orders { get; set; }
    }

    public class CustomerOrderReadDto
    {
        public int SalesOrderID { get; set; }
        public string SalesOrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public byte Status { get; set; }
        public decimal TotalDue { get; set; }
    }
}