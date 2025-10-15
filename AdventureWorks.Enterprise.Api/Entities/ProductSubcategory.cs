using System;
using System.Collections.Generic;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class ProductSubcategory
    {
        public int ProductSubcategoryID { get; set; }
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navegación
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}