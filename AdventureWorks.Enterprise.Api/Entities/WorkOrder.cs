using System;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class WorkOrder
    {
        public int WorkOrderID { get; set; }
        public int ProductID { get; set; }
        public int OrderQty { get; set; }
        public int StockedQty { get; set; } // Computed column
        public short ScrappedQty { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime DueDate { get; set; }
        public short? ScrapReasonID { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navegación
        public Product Product { get; set; } = null!;
    }
}