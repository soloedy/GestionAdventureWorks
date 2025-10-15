using System;
using System.Collections.Generic;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class SalesPerson
    {
        public int BusinessEntityID { get; set; }
        public int? TerritoryID { get; set; }
        public decimal? SalesQuota { get; set; }
        public decimal Bonus { get; set; }
        public decimal CommissionPct { get; set; }
        public decimal SalesYTD { get; set; }
        public decimal SalesLastYear { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navegación
        public ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}