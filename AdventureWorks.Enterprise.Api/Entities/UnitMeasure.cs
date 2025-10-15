using System;

namespace AdventureWorks.Enterprise.Api.Entities
{
    public class UnitMeasure
    {
        public string UnitMeasureCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DateTime ModifiedDate { get; set; }
    }
}