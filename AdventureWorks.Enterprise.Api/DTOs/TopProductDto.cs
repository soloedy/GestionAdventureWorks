using System;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    /// <summary>
    /// DTO for Top 10 best selling products (column names aligned with frontend expectations)
    /// </summary>
    public class TopProductDto
    {
        /// <summary>
        /// Product ID
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Product Name
        /// </summary>
        public string ProductName { get; set; } = string.Empty;

        /// <summary>
        /// Unit Price
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total Quantity Sold
        /// </summary>
        public int TotalQuantitySold { get; set; }

        /// <summary>
        /// Total Sales Amount
        /// </summary>
        public decimal TotalSalesAmount { get; set; }

        /// <summary>
        /// Category Name
        /// </summary>
        public string? CategoryName { get; set; }

        /// <summary>
        /// Subcategory Name
        /// </summary>
        public string? SubCategoryName { get; set; }
    }
}