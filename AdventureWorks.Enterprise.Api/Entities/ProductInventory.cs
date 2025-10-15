using System;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class ProductInventory
    {
        public int ProductID { get; set; }
        public short LocationID { get; set; }
        public string Shelf { get; set; } = string.Empty;
        public byte Bin { get; set; }
        public short Quantity { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navegación
        public Product Product { get; set; } = null!;
    }
}