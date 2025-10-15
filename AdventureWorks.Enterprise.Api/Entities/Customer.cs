using System;
using System.Collections.Generic;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public int? PersonID { get; set; }
        public int? StoreID { get; set; }
        public int? TerritoryID { get; set; }
        public string AccountNumber { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navegación
        public ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}