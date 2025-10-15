using System;
using System.Collections.Generic;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class ProductCategory
    {
        public int ProductCategoryID { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Navigation
        public ICollection<ProductSubcategory> ProductSubcategories { get; set; } = new List<ProductSubcategory>();
    }
}