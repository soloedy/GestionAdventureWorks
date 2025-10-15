using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    /// <summary>
    /// DTO para lectura de �rdenes de trabajo
    /// </summary>
    public class WorkOrderReadDto
    {
        /// <summary>
        /// ID de la orden de trabajo
        /// </summary>
        public int WorkOrderID { get; set; }
        
        /// <summary>
        /// ID del producto
        /// </summary>
        public int ProductID { get; set; }
        
        /// <summary>
        /// Nombre del producto
        /// </summary>
        public string ProductName { get; set; } = string.Empty;
        
        /// <summary>
        /// N�mero del producto
        /// </summary>
        public string ProductNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Cantidad ordenada
        /// </summary>
        public int OrderQty { get; set; }
        
        /// <summary>
        /// Cantidad en stock
        /// </summary>
        public int StockedQty { get; set; }
        
        /// <summary>
        /// Cantidad descartada
        /// </summary>
        public short ScrappedQty { get; set; }
        
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Fecha de finalizaci�n
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// ID de la raz�n de descarte
        /// </summary>
        public short? ScrapReasonID { get; set; }
        
        /// <summary>
        /// Fecha de modificaci�n
        /// </summary>
        public DateTime ModifiedDate { get; set; }
        
        /// <summary>
        /// Estado de la orden (calculado)
        /// </summary>
        public string Status { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// DTO para crear una nueva orden de trabajo
    /// </summary>
    public class WorkOrderCreateDto
    {
        /// <summary>
        /// ID del producto
        /// </summary>
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        public int ProductID { get; set; }
        
        /// <summary>
        /// Cantidad ordenada
        /// </summary>
        [Required(ErrorMessage = "La cantidad ordenada es obligatoria")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        public int OrderQty { get; set; }
        
        /// <summary>
        /// Cantidad descartada (por defecto es 0)
        /// </summary>
        public short ScrappedQty { get; set; } = 0;
        
        /// <summary>
        /// Fecha de inicio (por defecto es la fecha actual)
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Fecha de finalizaci�n (opcional)
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Fecha de vencimiento (requerida)
        /// </summary>
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// ID de la raz�n de descarte (opcional)
        /// </summary>
        public short? ScrapReasonID { get; set; }
    }
    
    /// <summary>
    /// DTO para actualizar una orden de trabajo
    /// </summary>
    public class WorkOrderUpdateDto
    {
        /// <summary>
        /// ID de la orden de trabajo
        /// </summary>
        [Required(ErrorMessage = "El ID de la orden de trabajo es obligatorio")]
        public int WorkOrderID { get; set; }
        
        /// <summary>
        /// Cantidad ordenada
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        public int OrderQty { get; set; }
        
        /// <summary>
        /// Cantidad descartada
        /// </summary>
        public short ScrappedQty { get; set; }
        
        /// <summary>
        /// Fecha de finalizaci�n
        /// </summary>
        public DateTime? EndDate { get; set; }
        
        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        public DateTime DueDate { get; set; }
        
        /// <summary>
        /// ID de la raz�n de descarte
        /// </summary>
        public short? ScrapReasonID { get; set; }
    }
    
    /// <summary>
    /// DTO para filtrar �rdenes de trabajo
    /// </summary>
    public class WorkOrderFilterDto
    {
        /// <summary>
        /// ID del producto para filtrar
        /// </summary>
        public int? ProductID { get; set; }
        
        /// <summary>
        /// Estado para filtrar (pendiente, completada, cancelada)
        /// </summary>
        public string? Status { get; set; }
        
        /// <summary>
        /// Fecha de inicio m�nima para filtrar
        /// </summary>
        public DateTime? StartDateFrom { get; set; }
        
        /// <summary>
        /// Fecha de inicio m�xima para filtrar
        /// </summary>
        public DateTime? StartDateTo { get; set; }
        
        /// <summary>
        /// Fecha de vencimiento m�nima para filtrar
        /// </summary>
        public DateTime? DueDateFrom { get; set; }
        
        /// <summary>
        /// Fecha de vencimiento m�xima para filtrar
        /// </summary>
        public DateTime? DueDateTo { get; set; }
        
        /// <summary>
        /// P�gina actual para la paginaci�n
        /// </summary>
        public int Page { get; set; } = 1;
        
        /// <summary>
        /// Tama�o de p�gina para la paginaci�n
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}