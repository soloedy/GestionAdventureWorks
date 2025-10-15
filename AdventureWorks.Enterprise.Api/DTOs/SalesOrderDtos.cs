using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    // DTOs para entrada
    public class SalesOrderHeaderCreateDto
    {
        [Required(ErrorMessage = "El campo RevisionNumber es obligatorio")]
        public byte RevisionNumber { get; set; }

        [Required(ErrorMessage = "El campo OrderDate es obligatorio")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "El campo DueDate es obligatorio")]
        public DateTime DueDate { get; set; }

        public DateTime? ShipDate { get; set; }

        [Required(ErrorMessage = "El campo Status es obligatorio")]
        [Range(1, 8, ErrorMessage = "El Status debe estar entre 1 y 8")]
        public byte Status { get; set; }

        [Required(ErrorMessage = "El campo OnlineOrderFlag es obligatorio")]
        public bool OnlineOrderFlag { get; set; }

        [StringLength(25, ErrorMessage = "El PurchaseOrderNumber no puede exceder los 25 caracteres")]
        public string PurchaseOrderNumber { get; set; }

        [Required(ErrorMessage = "El campo CustomerID es obligatorio")]
        public int CustomerID { get; set; }

        public int? SalesPersonID { get; set; }

        public int? TerritoryID { get; set; }

        [Required(ErrorMessage = "El campo BillToAddressID es obligatorio")]
        public int BillToAddressID { get; set; }

        [Required(ErrorMessage = "El campo ShipToAddressID es obligatorio")]
        public int ShipToAddressID { get; set; }

        [Required(ErrorMessage = "El campo ShipMethodID es obligatorio")]
        public int ShipMethodID { get; set; }

        public int? CreditCardID { get; set; }

        [StringLength(15, ErrorMessage = "El CreditCardApprovalCode no puede exceder los 15 caracteres")]
        public string CreditCardApprovalCode { get; set; }

        public int? CurrencyRateID { get; set; }

        [Required(ErrorMessage = "El campo SubTotal es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El SubTotal debe ser mayor que 0")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "El campo TaxAmt es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El TaxAmt no puede ser negativo")]
        public decimal TaxAmt { get; set; }

        [Required(ErrorMessage = "El campo Freight es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El Freight no puede ser negativo")]
        public decimal Freight { get; set; }

        [StringLength(128, ErrorMessage = "El Comment no puede exceder los 128 caracteres")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "Se requieren detalles de la orden")]
        public List<SalesOrderDetailCreateDto> OrderDetails { get; set; }
    }

    public class SalesOrderDetailCreateDto
    {
        [StringLength(25, ErrorMessage = "El CarrierTrackingNumber no puede exceder los 25 caracteres")]
        public string CarrierTrackingNumber { get; set; }

        [Required(ErrorMessage = "El campo OrderQty es obligatorio")]
        [Range(1, short.MaxValue, ErrorMessage = "La cantidad debe ser mayor que 0")]
        public short OrderQty { get; set; }

        [Required(ErrorMessage = "El campo ProductID es obligatorio")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "El campo SpecialOfferID es obligatorio")]
        public int SpecialOfferID { get; set; }

        [Required(ErrorMessage = "El campo UnitPrice es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor que 0")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "El campo UnitPriceDiscount es obligatorio")]
        [Range(0, 1, ErrorMessage = "El descuento debe estar entre 0 y 1")]
        public decimal UnitPriceDiscount { get; set; }
    }

    // DTOs para actualización (Anulación de orden)
    public class SalesOrderUpdateStatusDto
    {
        [Required(ErrorMessage = "El campo SalesOrderID es obligatorio")]
        public int SalesOrderID { get; set; }

        [Required(ErrorMessage = "El campo Status es obligatorio")]
        [Range(1, 8, ErrorMessage = "El Status debe estar entre 1 y 8")]
        public byte Status { get; set; }

        [StringLength(128, ErrorMessage = "El Comment no puede exceder los 128 caracteres")]
        public string Comment { get; set; }
    }

    // DTOs para salida
    public class SalesOrderHeaderReadDto
    {
        public int SalesOrderID { get; set; }
        public byte RevisionNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        public bool OnlineOrderFlag { get; set; }
        public string SalesOrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string AccountNumber { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } // Campo adicional para mostrar el nombre del cliente
        public int? SalesPersonID { get; set; }
        public string SalesPersonName { get; set; } // Campo adicional para mostrar el nombre del vendedor
        public int? TerritoryID { get; set; }
        public int BillToAddressID { get; set; }
        public int ShipToAddressID { get; set; }
        public int ShipMethodID { get; set; }
        public int? CreditCardID { get; set; }
        public string CreditCardApprovalCode { get; set; }
        public int? CurrencyRateID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public decimal TotalDue { get; set; }
        public string Comment { get; set; }
        public DateTime ModifiedDate { get; set; }
        public List<SalesOrderDetailReadDto> OrderDetails { get; set; }
    }

    public class SalesOrderDetailReadDto
    {
        public int SalesOrderID { get; set; }
        public int SalesOrderDetailID { get; set; }
        public string CarrierTrackingNumber { get; set; }
        public short OrderQty { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } // Campo adicional para mostrar el nombre del producto
        public int SpecialOfferID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
        public decimal LineTotal { get; set; }
    }

    // DTO para filtrar órdenes por rango de fecha
    public class SalesOrderDateRangeDto
    {
        [Required(ErrorMessage = "La fecha inicial es obligatoria")]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "La fecha final es obligatoria")]
        public DateTime EndDate { get; set; }
    }

    // DTO para procedimiento almacenado de creación de órdenes
    public class SpCreateOrderDto
    {
        // Parámetros para el encabezado
        public byte RevisionNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        public bool OnlineOrderFlag { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public int CustomerID { get; set; }
        public int? SalesPersonID { get; set; }
        public int? TerritoryID { get; set; }
        public int BillToAddressID { get; set; }
        public int ShipToAddressID { get; set; }
        public int ShipMethodID { get; set; }
        public int? CreditCardID { get; set; }
        public string CreditCardApprovalCode { get; set; }
        public int? CurrencyRateID { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal Freight { get; set; }
        public string Comment { get; set; }

        // Parámetro de tabla para los detalles (se convertirá en XML o tabla temporal)
        public List<SpOrderDetailDto> OrderDetails { get; set; }
    }

    public class SpOrderDetailDto
    {
        public string CarrierTrackingNumber { get; set; }
        public short OrderQty { get; set; }
        public int ProductID { get; set; }
        public int SpecialOfferID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; }
    }
}